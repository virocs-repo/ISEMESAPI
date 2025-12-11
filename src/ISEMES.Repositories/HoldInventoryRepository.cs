using ISEMES.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace ISEMES.Repositories
{
    public class HoldInventoryRepository : IHoldInventoryRepositories
    {
        private readonly IConfiguration _configuration;

        public HoldInventoryRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<InventoryHolds>> GetAllSearchHoldAsync(DateTime? fromDate, DateTime? toDate)
        {
            var inventoryHolds = new List<InventoryHolds>();
            using (var connection = new SqlConnection(_configuration.GetConnectionString("InventoryConnection")))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("usp_SearchHold", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@FromDate", fromDate ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@ToDate", toDate ?? (object)DBNull.Value);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            inventoryHolds.Add(new InventoryHolds
                            {
                                InventoryID = reader.GetInt32(0),
                                IseLotNumber = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                                CustomerName = reader.IsDBNull(7) ? string.Empty : reader.GetString(7),
                                Device = reader.IsDBNull(8) ? string.Empty : reader.GetString(8),
                                Status = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                                CommitSOD = reader.IsDBNull(13) ? (DateTime?)null : reader.GetDateTime(13),
                                HoldDate = reader.IsDBNull(14) ? (DateTime?)null : reader.GetDateTime(14),
                                ElapsedTimeOfHold = reader.IsDBNull(15) ? string.Empty : reader.GetString(15),
                            });
                        }
                    }
                }
            }
            return inventoryHolds;
        }

        public async Task<IEnumerable<HoldTypeResponse>> GetHoldTypesAsync(int? inventoryId)
        {
            var holdTypeList = new List<HoldTypeResponse>();
            using (var connection = new SqlConnection(_configuration.GetConnectionString("InventoryConnection")))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("usp_GetTFSHoldCodes", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@InventoryId", (object)inventoryId ?? DBNull.Value);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            holdTypeList.Add(new HoldTypeResponse
                            {
                                HoldTypeId = reader.GetInt32(0),
                                HoldType = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                            });
                        }
                    }
                }
            }
            return holdTypeList;
        }

        public async Task<IEnumerable<Hold>> GetHoldCodesAsync(int? inventoryId)
        {
            var holdcodeList = new List<Hold>();
            using (var connection = new SqlConnection(_configuration.GetConnectionString("InventoryConnection")))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("usp_GetTFSHoldCodes", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@InventoryId", (object)inventoryId ?? DBNull.Value);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync()) { }
                        reader.NextResult();
                        while (await reader.ReadAsync())
                        {
                            holdcodeList.Add(new Hold
                            {
                                HoldCodeId = reader.GetInt32(0),
                                HoldCode = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                                Source = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                                HoldReason = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                                Description = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                                HoldTypeId = reader.GetInt32(5),
                                GroupName = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                                IsReceiving = reader.GetBoolean(7),
                                IsService = reader.GetBoolean(8),
                                ServiceId = reader.IsDBNull(9) ? (int?)null : reader.GetInt32(9),
                                IsShipping = reader.GetBoolean(10),
                                ProcessTypeId = reader.IsDBNull(11) ? (int?)null : reader.GetInt32(11),
                            });
                        }
                    }
                }
            }
            return holdcodeList;
        }

        public async Task<IEnumerable<InventoryHold>> GetAllHoldsAsync(int? inventoryId)
        {
            var holdList = new List<InventoryHold>();
            using (var connection = new SqlConnection(_configuration.GetConnectionString("InventoryConnection")))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("INV_Inventory_GetAllHolds", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@InventoryId", (object)inventoryId ?? DBNull.Value);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            holdList.Add(new InventoryHold
                            {
                                InventoryXHoldId = reader.GetInt32(0),
                                InventoryID = reader.GetInt32(1),
                                HoldType = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                                HoldCode = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                                HoldComments = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                                HoldDate = reader.IsDBNull(6) ? (DateTime?)null : reader.GetDateTime(6),
                                OffHoldComments = reader.IsDBNull(7) ? string.Empty : reader.GetString(7),
                                OffHoldDate = reader.IsDBNull(8) ? (DateTime?)null : reader.GetDateTime(8),
                                Reason = reader.IsDBNull(9) ? string.Empty : reader.GetString(9),
                                HoldBy = reader.IsDBNull(10) ? string.Empty : reader.GetString(10),
                                OffHoldBy = reader.IsDBNull(11) ? string.Empty : reader.GetString(11),
                                TFSHold = reader.IsDBNull(12) ? 0 : reader.GetInt32(12),
                                Source = reader.IsDBNull(13) ? string.Empty : reader.GetString(13),
                                OnHold = reader.GetString(14),
                            });
                        }
                    }
                }
            }
            return holdList;
        }

        public async Task<int> UpsertHoldAsync(HoldRequest holdRequest)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("InventoryConnection")))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("Usp_UpsertHold", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@InventoryXHoldId", (object?)holdRequest.InventoryXHoldId ?? DBNull.Value);
                    command.Parameters.AddWithValue("@InventoryId", holdRequest.InventoryID);
                    command.Parameters.AddWithValue("@Reason", holdRequest.Reason);
                    command.Parameters.AddWithValue("@HoldComments", holdRequest.HoldComments);
                    command.Parameters.AddWithValue("@HoldType", (object?)holdRequest.HoldType ?? DBNull.Value);
                    command.Parameters.AddWithValue("@GroupName", (object?)holdRequest.GroupName ?? DBNull.Value);
                    command.Parameters.AddWithValue("@HoldCodeId", (object?)holdRequest.HoldCodeId ?? DBNull.Value);
                    command.Parameters.AddWithValue("@OffHoldComments", (object?)holdRequest.OffHoldComments ?? DBNull.Value);
                    command.Parameters.AddWithValue("@UserId", holdRequest.UserId);
                    command.Parameters.AddWithValue("@ConfirmedQty", (object?)holdRequest.ConfirmedQty ?? DBNull.Value);

                    var outputParam = new SqlParameter("@InsertedHoldID", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    command.Parameters.Add(outputParam);
                    await command.ExecuteNonQueryAsync();
                    return (int)(outputParam.Value ?? 0);
                }
            }
        }

        public async Task<IEnumerable<HoldDetails>> GetHoldDetailsAsync(int? inventoryXHoldId)
        {
            var holdDetailsList = new List<HoldDetails>();
            using (var connection = new SqlConnection(_configuration.GetConnectionString("InventoryConnection")))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("INV_GetHoldDetails", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@InventoryXHoldId", (object)inventoryXHoldId ?? DBNull.Value);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            holdDetailsList.Add(new HoldDetails
                            {
                                InventoryXHoldId = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                                InventoryID = reader.IsDBNull(1) ? 0 : reader.GetInt32(1),
                                Reason = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                                HoldComments = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                                HoldType = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                                GroupName = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                                HoldCodeId = reader.IsDBNull(6) ? 0 : reader.GetInt32(6),
                                OffHoldComments = reader.IsDBNull(7) ? null : reader.GetString(7),
                                OffHoldDate = reader.IsDBNull(8) ? DateTime.MinValue : reader.GetDateTime(8),
                                OffHoldBy = reader.IsDBNull(9) ? string.Empty : reader.GetString(9),
                                CreatedBy = reader.IsDBNull(10) ? string.Empty : reader.GetString(10),
                                CreatedOn = reader.IsDBNull(11) ? DateTime.MinValue : reader.GetDateTime(11),
                                Active = reader.IsDBNull(12) ? false : reader.GetBoolean(12),
                                TFSHold = reader.IsDBNull(13) ? 0 : reader.GetInt32(13),
                                HoldCode = reader.IsDBNull(14) ? string.Empty : reader.GetString(14),
                                ExpectedQty = reader.IsDBNull(15) ? 0 : reader.GetInt32(15),
                                AvailableQty = reader.IsDBNull(16) ? 0 : reader.GetInt32(16),
                                ConfirmedQty = reader.IsDBNull(17) ? 0 : reader.GetInt32(17),
                            });
                        }
                    }
                }
            }
            return holdDetailsList;
        }

        public async Task<List<HoldComment>> GetHoldCommentsAsync()
        {
            var inventoryHoldComments = new List<HoldComment>();
            using (var connection = new SqlConnection(_configuration.GetConnectionString("InventoryConnection")))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("INV_GetHoldComments", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            inventoryHoldComments.Add(new HoldComment
                            {
                                HoldCommentId = reader.GetInt32(0),
                                HoldComments = reader.IsDBNull(1) ? string.Empty : reader.GetString(1)
                            });
                        }
                    }
                }
            }
            return inventoryHoldComments;
        }

        public async Task<IEnumerable<HoldCustomerDetails>> GetCustomerDetailsAsync(int? inventoryId)
        {
            var customerDeatilsList = new List<HoldCustomerDetails>();
            using (var connection = new SqlConnection(_configuration.GetConnectionString("InventoryConnection")))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("INV_Inventory_GetAllHolds", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@InventoryId", (object)inventoryId ?? DBNull.Value);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.NextResultAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                customerDeatilsList.Add(new HoldCustomerDetails
                                {
                                    InventoryId = reader.GetInt32(0),
                                    CustomerName = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                                    Device = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                                    HoldTime = reader.IsDBNull(3) ? string.Empty : reader.GetString(3)
                                });
                            }
                        }
                    }
                }
            }
            return customerDeatilsList;
        }

        public async Task<IEnumerable<OperaterAttachements>> GetOperaterAttachmentsAsync(int? TFSHoldId)
        {
            var operaterAttachementsList = new List<OperaterAttachements>();
            using (var connection = new SqlConnection(_configuration.GetConnectionString("InventoryTFS_Prod2Connection")))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("PRD_GetOperatorAttachments", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@TFSHoldId", (object)TFSHoldId ?? DBNull.Value);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            operaterAttachementsList.Add(new OperaterAttachements
                            {
                                AttachmentId = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                                TRVStepId = reader.IsDBNull(1) ? 0 : reader.GetInt32(1),
                                TransactionId = reader.IsDBNull(2) ? 0 : reader.GetInt32(2),
                                AttachedFile = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                                AttachmentTypeId = reader.IsDBNull(4) ? 0 : reader.GetInt32(4),
                                AttachmentType = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                                AttachedById = reader.IsDBNull(6) ? 0 : reader.GetInt32(6),
                                AttachedBy = reader.IsDBNull(7) ? string.Empty : reader.GetString(7),
                                UpdatedBy = reader.IsDBNull(8) ? string.Empty : reader.GetString(8),
                                AttachedOn = reader.GetDateTime(9),
                                UpdatedOn = reader.GetDateTime(10),
                                Active = reader.IsDBNull(11) ? false : reader.GetBoolean(11)
                            });
                        }
                    }
                }
            }
            return operaterAttachementsList;
        }
    }
}

