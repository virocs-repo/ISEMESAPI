using ISEMES.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace ISEMES.Repositories
{
    public class OtherInventoryRepository : IOtherInventoryRepository
    {
        private readonly AppDbContext _context;

        public OtherInventoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<KeyValueData>> GetOtherInventoryStatusAsync()
        {
            var statusTypes = new List<KeyValueData>();
            using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("usp_GetOtherInventoryStatus", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            statusTypes.Add(new KeyValueData
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1)
                            });
                        }
                    }
                }
            }
            return statusTypes;
        }

        public async Task<List<AnotherShipment>> GetOtherInventoryShipmentsAsync(int? customerId, int? employeeId, int? statusId, DateTime? fromDate, DateTime? toDate)
        {
            return await _context.ListOtherInventoryShipment.FromSqlRaw("EXEC usp_SearchOtherInventoryShipment @CustomerId = {0}, @EmployeeId = {1}, @StatusId = {2}, @FromDate = {3}, @ToDate = {4}",
                customerId.HasValue ? customerId : DBNull.Value,
                employeeId.HasValue ? employeeId : DBNull.Value,
                statusId.HasValue ? statusId : DBNull.Value,
                fromDate.HasValue ? fromDate : DBNull.Value,
                toDate.HasValue ? toDate : DBNull.Value).ToListAsync();
        }

        public async Task<AnotherShipmentDetail> GetOtherInventoryShipmentAsync(int anotherShippingId)
        {
            AnotherShipmentDetail anotherShipmentDetail = new AnotherShipmentDetail();
            using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("usp_GetAnotherShippingDetails", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@AnotherShippingID", anotherShippingId);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            anotherShipmentDetail.AnotherShipmentID = reader.GetInt32(0);
                            anotherShipmentDetail.RequestorID = reader.GetInt32(1);
                            anotherShipmentDetail.Email = reader.GetString(2);
                            anotherShipmentDetail.RecipientName = reader.GetString(3);
                            anotherShipmentDetail.PhoneNo = reader.GetInt64(4).ToString();
                            anotherShipmentDetail.CustomerTypeID = reader.GetInt32(5);
                            anotherShipmentDetail.CustomerVendorID = reader.GetInt32(6);
                            anotherShipmentDetail.BehalfID = reader.GetInt32(7);
                            anotherShipmentDetail.Address1 = reader.GetString(8);
                            anotherShipmentDetail.Address2 = reader.GetString(9);
                            anotherShipmentDetail.City = reader.GetString(10);
                            anotherShipmentDetail.State = reader.GetString(11);
                            anotherShipmentDetail.Zip = reader.GetString(12);
                            anotherShipmentDetail.Country = reader.GetString(13);
                            anotherShipmentDetail.Instructions = reader.GetString(14);
                            anotherShipmentDetail.Status = reader.GetString(15);
                            anotherShipmentDetail.RecordStatus = reader.GetString(16);
                            anotherShipmentDetail.ApproverID = reader[17] == DBNull.Value ? null : reader.GetInt32(17);
                            anotherShipmentDetail.ApprovedBy = reader[18] == DBNull.Value ? null : reader.GetInt32(18);
                            anotherShipmentDetail.ApprovedON = reader.GetDateTime(19);
                            anotherShipmentDetail.ServiceTypeID = reader.GetInt32(20);
                            anotherShipmentDetail.AccountNo = reader[21] == DBNull.Value ? string.Empty : reader.GetString(21);
                        }

                        reader.NextResult();
                        anotherShipmentDetail.AnotherShipLineItems = new List<AnotherShipmentLineItem>();
                        while (await reader.ReadAsync())
                        {
                            anotherShipmentDetail.AnotherShipLineItems.Add(new AnotherShipmentLineItem
                            {
                                LineItemID = reader.GetInt32(0),
                                InventoryID = reader.GetInt32(1),
                                Description = reader.GetString(2),
                                Quantity = reader.GetInt32(3),
                                Value = reader.GetInt32(4),
                                Status = reader.GetInt32(5)
                            });
                        }
                    }
                }
            }
            return anotherShipmentDetail;
        }

        public async Task<int> UpsertAntherInventoryShipmentAsync(string anotherShipJson)
        {
            var parameters = new[] { new SqlParameter("@InputJSON", anotherShipJson) };
            return await _context.Database.ExecuteSqlRawAsync("EXEC usp_UpsertAnotherShipping @InputJSON", parameters);
        }

        public async Task<List<KeyValueData>> GetServiceTypesAsync()
        {
            var statusTypes = new List<KeyValueData>();
            using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("usp_GetServiceType", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            statusTypes.Add(new KeyValueData
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1)
                            });
                        }
                    }
                }
            }
            return statusTypes;
        }

        public async Task<int> VoidAnotherShippingAsync(int anotherShippingID)
        {
            var parameters = new[] { new SqlParameter("@AnotherShippingID", anotherShippingID) };
            return await _context.Database.ExecuteSqlRawAsync("EXEC usp_VoidOtherShipping @AnotherShippingID", parameters);
        }

        public async Task<List<KeyValueData>> GetAnotherInventoryLots(int customerTypeId, int customerVendorId)
        {
            var lots = new List<KeyValueData>();
            using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("usp_GetOtherInventoryLot", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CustomerVendorID", customerVendorId);
                    command.Parameters.AddWithValue("@CustomerTypeID", customerTypeId);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            lots.Add(new KeyValueData
                            {
                                Name = reader.GetString(0),
                                Id = reader.GetInt32(1)
                            });
                        }
                    }
                }
            }
            return lots;
        }
    }
}

