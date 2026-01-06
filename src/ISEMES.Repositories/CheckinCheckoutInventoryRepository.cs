using ISEMES.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Text.Json;

namespace ISEMES.Repositories
{
    public class CheckinCheckoutInventoryRepository : ICheckinCheckoutInventoryRepository
    {
        private readonly IConfiguration _configuration;
   
        public CheckinCheckoutInventoryRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<InventoryCheckinCheckout>> GetAllInventoryCheckinCheckoutStatusAsync(DateTime? fromDate, DateTime? toDate)
        {
            var inventoryMoveStatus = new List<InventoryCheckinCheckout>();
            using (var connection = new SqlConnection(_configuration.GetConnectionString("InventoryConnection")))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("usp_SearchInventoryCheckinout", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@FromDate", fromDate ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@ToDate", toDate ?? (object)DBNull.Value);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var detail = new InventoryCheckinCheckout
                            {
                                InventoryId = reader.GetInt32(0),
                                LotNum = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                                Location = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                                ReceivedFromId = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                                ReceivedFrom = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                                Person = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                                Qty = reader.IsDBNull(7) ? 0 : reader.GetInt32(7),
                                CheckinCheckOutQTY = reader.IsDBNull(8) ? 0 : reader.GetInt32(8),
                                SystemUser = reader.IsDBNull(9) ? string.Empty : reader.GetString(9),
                                InventoryStatusId = reader.IsDBNull(10) ? 0 : reader.GetInt32(10),
                                InventoryStatus = reader.IsDBNull(11) ? string.Empty : reader.GetString(11),
                                GoodsType = reader.IsDBNull(12) ? string.Empty : reader.GetString(12),
                                Area = reader.IsDBNull(14) ? string.Empty : reader.GetString(14),
                                WIPLocation = reader.IsDBNull(15) ? string.Empty : reader.GetString(15),
                                CustomerName = reader.IsDBNull(16) ? string.Empty : reader.GetString(16),
                                Device = reader.IsDBNull(17) ? string.Empty : reader.GetString(17),
                                ReceivedFromCheckOut = reader.IsDBNull(18) ? string.Empty : reader.GetString(18),
                                CheckInOutTime = reader.IsDBNull(19) ? (DateTime?)null : reader.GetDateTime(19),
                                SystemCheckInOutPerson = reader.IsDBNull(20) ? string.Empty : reader.GetString(20),
                                CurrentLocation = reader.IsDBNull(21) ? string.Empty : reader.GetString(21),
                                CustomerLotNumber = reader.IsDBNull(22) ? string.Empty : reader.GetString(22),
                            };
                            inventoryMoveStatus.Add(detail);
                        }
                    }
                }
            }
            return inventoryMoveStatus;
        }

        public async Task<IEnumerable<InventoryCheckinCheckoutLocation>> GetInventoryCheckinCheckoutLocationAsync()
        {
            var inventoryLocation = new List<InventoryCheckinCheckoutLocation>();
            using (var connection = new SqlConnection(_configuration.GetConnectionString("InventoryConnection")))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("usp_GetInventoryLocation", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var inventoryStatus = new InventoryCheckinCheckoutLocation
                            {
                                InventoryLocationID = reader.GetInt32(0),
                                Location = reader.GetString(1)
                            };
                            inventoryLocation.Add(inventoryStatus);
                        }
                    }
                }
            }
            return inventoryLocation;
        }

        public async Task<IEnumerable<InventoryCheckinCheckoutStatuses>> GetInventoryCheckinCheckoutStatusAsync()
        {
            var inventoryStatuses = new List<InventoryCheckinCheckoutStatuses>();
            using (var connection = new SqlConnection(_configuration.GetConnectionString("InventoryConnection")))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("usp_GetInventoryStatus", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var inventoryStatus = new InventoryCheckinCheckoutStatuses
                            {
                                InventoryStatusID = reader.GetInt32(0),
                                InventoryStatus = reader.GetString(1)
                            };
                            inventoryStatuses.Add(inventoryStatus);
                        }
                    }
                }
            }
            return inventoryStatuses;
        }

        public async Task<IEnumerable<InventoryCheckinCheckoutItem>> GetInventoryCheckinCheckoutAsync(string lotNumber)
        {
            var inventoryList = new List<InventoryCheckinCheckoutItem>();
            using (var connection = new SqlConnection(_configuration.GetConnectionString("InventoryConnection")))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("usp_GetInventoryCheckinout", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@LotNum", (object)lotNumber ?? DBNull.Value);                   
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var inventory = new InventoryCheckinCheckoutItem
                            {
                                InventoryId = reader.GetInt32(0),
                                LotNum = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                                LocationId = reader.IsDBNull(2) ? 0 : reader.GetInt32(2),
                                Location = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                                ReceivedFromId = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                                ReceivedFrom = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                                Person = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                                Qty = reader.IsDBNull(7) ? 0 : reader.GetInt32(7),
                                SystemUser = reader.IsDBNull(8) ? string.Empty : reader.GetString(8),
                                InventoryStatusId = reader.IsDBNull(9) ? 0 : reader.GetInt32(9),
                                Status = reader.IsDBNull(10) ? string.Empty : reader.GetString(10),
                                GoodsType = reader.IsDBNull(11) ? string.Empty : reader.GetString(11)
                            };
                            inventoryList.Add(inventory);
                        }
                    }
                }
            }
            return inventoryList;
        }

        public async Task UpsertInventoryCheckinCheckoutStatusAsync(InventoryCheckinCheckoutRequest request)
        {
            string jsonInput = JsonSerializer.Serialize(request);

            using (var connection = new SqlConnection(_configuration.GetConnectionString("InventoryConnection")))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("usp_UpsertInventoryCheckinout", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@InputJSON", SqlDbType.NVarChar)
                    {
                        Value = jsonInput
                    });
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<IEnumerable<CheckinCheckoutStatus>> GetCheckinCheckoutAsync(string lotNumber)
        {
            var inventoryList = new List<CheckinCheckoutStatus>();
            using (var connection = new SqlConnection(_configuration.GetConnectionString("InventoryConnection")))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("usp_GetInventoryCheckinout", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@LotNum", (object)lotNumber ?? DBNull.Value);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var inventory = new CheckinCheckoutStatus
                            {
                                InventoryStatus = reader.IsDBNull(10) ? string.Empty : reader.GetString(10),
                            };
                            inventoryList.Add(inventory);
                        }
                    }
                }
            }
            return inventoryList;
        }
    }
}



