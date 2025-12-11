using ISEMES.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace ISEMES.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly string? _connectString;

        public TicketRepository(IConfiguration configuration)
        {
            _connectString = configuration.GetConnectionString("InventoryConnection");
        }

        public async Task<List<SearchTicket>> SearchTickets(DateTime? fromDate, DateTime? toDate)
        {
            var result = new List<SearchTicket>();
            try
            {
                using (var connection = new SqlConnection(_connectString))
                {
                    using (var command = new SqlCommand("dbo.usp_Search_CTK_Tickets", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@FromDate", fromDate);
                        command.Parameters.AddWithValue("@ToDate", toDate);
                        await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                                var ticket = new SearchTicket();
                                ticket.ID = "Tkt-" + reader.GetInt32(0);
                                ticket.TicketID = reader.GetInt32(0);
                                ticket.TicketTypeID = reader.GetInt32(1);
                                ticket.TicketType = reader.GetString(2);
                                ticket.RequestorID = reader.GetInt32(3);
                                ticket.Requestor = reader.GetString(4);
                                ticket.RequestDetails = reader.GetString(5);
                                ticket.ASLotString = reader.GetString(6);
                                ticket.TicketStatus = reader.GetString(7);
                                ticket.AcceptedTime = reader[8] == DBNull.Value ? null : reader.GetDateTime(8).ToString("yyyy-MM-dd HH:mm:ss");
                                ticket.TicketStatusID = reader.GetInt32(9);
                                ticket.CreatedOn = reader.GetDateTime(10).ToString("yyyy-MM-dd HH:mm:ss");
                                ticket.DueDate = reader.GetDateTime(11).ToString("yyyy-MM-dd HH:mm:ss");
                                result.Add(ticket);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public async Task<List<TicketType>> GetTicketType()
        {
            var reqTypes = new List<TicketType>();
            using (var connection = new SqlConnection(_connectString))
            {
                using (var command = new SqlCommand("dbo.usp_GetCTK_TicketType", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    await connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            reqTypes.Add(new TicketType
                            {
                                TicketTypeID = reader.GetInt32(0),
                                TicketTypeName = reader.GetString(1),
                                MultiSelect = reader.GetBoolean(2)
                            });
                        }
                    }
                }
            }
            return reqTypes;
        }

        public async Task<List<TicketLot>> GetTicketLots()
        {
            var lots = new List<TicketLot>();
            using (var connection = new SqlConnection(_connectString))
            {
                using (var command = new SqlCommand("dbo.usp_Get_CTK_InventoryLot", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    await connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            lots.Add(new TicketLot
                            {
                                InventoryID = reader.GetInt32(0),
                                LotNum = reader.GetString(1)
                            });
                        }
                    }
                }
            }
            return lots;
        }

        public async Task<List<TicketLotDetail>> GetTicketLineItemLots(string lotNumbers)
        {
            var lots = new List<TicketLotDetail>();
            using (var connection = new SqlConnection(_connectString))
            {
                using (var command = new SqlCommand("dbo.usp_GetCTK_lineitemDetails", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    if (!string.IsNullOrEmpty(lotNumbers))
                    {
                        command.Parameters.AddWithValue("@CustLotStr", lotNumbers);
                    }
                    await connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            lots.Add(new TicketLotDetail
                            {
                                InventoryID = reader.GetInt32(0),
                                ISELot = reader.GetString(1),
                                CustomerLot = reader.GetString(2),
                                DeviceType = reader.GetString(3),
                                Qty = reader.GetInt32(4),
                                ICRLocation = reader[5] == DBNull.Value ? null : reader.GetString(5),
                                CustomerName = reader.GetString(6)
                            });
                        }
                    }
                }
            }
            return lots;
        }

        public async Task<bool> UpsertTicket(string upsertJson, string? ticketAttachments, string? reviewerAttachments)
        {
            using (var connection = new SqlConnection(_connectString))
            {
                using (var command = new SqlCommand("dbo.usp_Upsert_Ctk_Ticket", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@InputJSON", upsertJson);
                    command.Parameters.AddWithValue("@TicketAttachments", ticketAttachments);
                    command.Parameters.AddWithValue("@CommentAttachments", reviewerAttachments);
                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                    return true;
                }
            }
        }

        public async Task<TicketDetail> GetTicketDetail(int ticketId)
        {
            var result = new TicketDetail();
            using (var connection = new SqlConnection(_connectString))
            {
                using (var command = new SqlCommand("usp_GetCTK_TicketDetailsByID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@TicketID", ticketId);
                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            result.TicketID = reader.GetInt32(0);
                            result.TicketTypeID = reader.GetInt32(1);
                            result.RequestorID = reader.GetInt32(2);
                            result.RequestDetails = reader.GetString(3);
                            result.DueDate = reader.GetDateTime(4).ToString("yyyy-MM-dd HH:mm:ss");
                            result.StatusID = reader.GetInt32(5);
                            result.UserID = reader.GetInt32(8);
                            result.Active = reader.GetInt32(10);
                        }

                        reader.NextResult();
                        result.LotDetails = new List<TicketLotDetail>();
                        while (await reader.ReadAsync())
                        {
                            result.LotDetails.Add(new TicketLotDetail
                            {
                                InventoryID = reader.GetInt32(1),
                                ISELot = reader.GetString(2),
                                CustomerLot = reader.GetString(3),
                                DeviceType = reader.GetString(4),
                                Qty = reader.GetInt32(5),
                                ICRLocation = reader[6] == DBNull.Value ? null : reader.GetString(6),
                                CustomerName = reader.GetString(7),
                                ScanLotNum = reader[8] == DBNull.Value ? null : reader.GetString(8)
                            });
                        }

                        reader.NextResult();
                        result.Comments = new List<TicketComments>();
                        while (await reader.ReadAsync())
                        {
                            result.Comments.Add(new TicketComments
                            {
                                CommentId = reader.GetInt32(0),
                                ReviewerComments = reader.GetString(2),
                                RequestorComments = reader.GetString(3),
                                CreatedBy = reader.GetString(4),
                                CreatedOn = reader.GetDateTime(5).ToString("yyyy-MM-dd HH:mm:ss")
                            });
                        }

                        reader.NextResult();
                        result.CommentsAttachments = new List<CommentAttachment>();
                        while (await reader.ReadAsync())
                        {
                            result.CommentsAttachments.Add(new CommentAttachment
                            {
                                CommentId = reader.GetInt32(0),
                                FileName = reader.GetString(2)
                            });
                        }

                        reader.NextResult();
                        result.TicketAttachments = new List<TicketAttachment>();
                        while (await reader.ReadAsync())
                        {
                            result.TicketAttachments.Add(new TicketAttachment
                            {
                                FileName = reader.GetString(2),
                                Active = true
                            });
                        }
                    }
                }
            }
            return result;
        }

        public async Task<bool> VoidTicket(int ticketID)
        {
            using (var connection = new SqlConnection(_connectString))
            {
                using (var command = new SqlCommand("dbo.usp_Void_CTK_Ticket", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@TicketID", ticketID);
                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                    return true;
                }
            }
        }
    }
}

