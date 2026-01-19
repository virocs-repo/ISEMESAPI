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

        public async Task<EmployeeDetails?> ValidateBadge(string badge)
        {
            EmployeeDetails? employeeDetails = null;
            using (var connection = new SqlConnection(_configuration.GetConnectionString("InventoryConnection")))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("tfsp_ValidateBatchCode", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@Application", SqlDbType.NVarChar)
                    {
                        Value = "Ticketing"
                    });
                    command.Parameters.Add(new SqlParameter("@BadgeCode", SqlDbType.NVarChar)
                    {
                        Value = badge
                    });
                    using var reader = await command.ExecuteReaderAsync();
                    if (reader.Read()) // read only first record
                    {
                        employeeDetails = new EmployeeDetails
                        {
                            EmployeeId = reader["EmployeeId"] != DBNull.Value ? Convert.ToInt32(reader["EmployeeId"]) : 0,
                            CustomerLoginId = reader["CustomerLoginId"] != DBNull.Value ? Convert.ToInt32(reader["CustomerLoginId"]) : 0,
                            UserName = reader["UserName"] as string ?? string.Empty
                        };
                        return employeeDetails;
                    }
                }
            }
            return employeeDetails;
        }

        public async Task<List<CheckInCheckOutLotDetails>> GetCheckInCheckOutLotDetailsAsync(string? lotNumber, int employeeId, int customerLoginId, string requestType, int? count)
        {
            List<CheckInCheckOutLotDetails> checkInCheckOutLotDetails = new List<CheckInCheckOutLotDetails>();
            using (var connection = new SqlConnection(_configuration.GetConnectionString("InventoryConnection")))
            {
                await connection.OpenAsync();
                string checkInCheckOutProc = requestType.Equals("CheckIn", StringComparison.OrdinalIgnoreCase) ? "Ticketing_GetLotsForCheckIn" : "Ticketing_GetLotsForCheckOut";
                using (var command = new SqlCommand(checkInCheckOutProc, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@InputNumber", SqlDbType.NVarChar)
                    {
                        Value = lotNumber == null ? DBNull.Value : lotNumber
                    });
                    command.Parameters.Add(new SqlParameter("@EmployeeId", SqlDbType.Int)
                    {
                        Value = employeeId
                    });
                    command.Parameters.Add(new SqlParameter("@CustomerLoginId", SqlDbType.Int)
                    {
                        Value = customerLoginId
                    });
                    using var reader = await command.ExecuteReaderAsync();
                    do
                    {
                        checkInCheckOutLotDetails = new List<CheckInCheckOutLotDetails>();
                        while (await reader.ReadAsync())
                        {
                            try
                            {
                                var checkInCheckOutLotDetail = new CheckInCheckOutLotDetails
                                {
                                    //    RequestId = reader["RequestId"] != DBNull.Value ? Convert.ToInt32(reader["RequestId"]) : (int?)null,
                                    //    RequestedBy = reader["RequestedBy"] != DBNull.Value ? reader["RequestedBy"].ToString() : null,
                                    //    RequestedOn = reader["RequestedOn"] != DBNull.Value ? Convert.ToDateTime(reader["RequestedOn"]) : DateTime.MinValue,
                                    CRId = reader["CRId"] != DBNull.Value ? Convert.ToInt32(reader["CRId"]) : (int?)null,
                                    LotId = reader["LotId"] != DBNull.Value ? Convert.ToInt32(reader["LotId"]) : (int?)null,
                                    ReceivingNo = reader["ReceivingNo"] != DBNull.Value ? reader["ReceivingNo"].ToString() : null,
                                    CustomerName = reader["CustomerName"] != DBNull.Value ? reader["CustomerName"].ToString() : null,
                                    ISELotNumber = reader["ISELotNumber"] != DBNull.Value ? reader["ISELotNumber"].ToString() : null,
                                    TotalCount = reader["TotalCount"] != DBNull.Value ? Convert.ToInt32(reader["TotalCount"]) : (int?)null,
                                    AvailableCount = reader["AvailableCount"] != DBNull.Value ? Convert.ToInt32(reader["AvailableCount"]) : (int?)null,
                                    RunningCount = reader["RunningCount"] != DBNull.Value ? Convert.ToInt32(reader["RunningCount"]) : (int?)null,
                                    GoodsType = reader["GoodsType"] != DBNull.Value ? reader["GoodsType"].ToString() : null,
                                    State = reader["State"] != DBNull.Value ? reader["State"].ToString() : null,
                                    AreaCode = reader["AreaCode"] != DBNull.Value ? reader["AreaCode"].ToString() : null,
                                    LocationName = reader["LocationName"] != DBNull.Value ? reader["LocationName"].ToString() : null,
                                    CanCheckIn = reader["CanCheckIn"] != DBNull.Value ? Convert.ToBoolean(reader["CanCheckIn"]) : (bool?)null,
                                    CanEditCheckIn = reader["CanEditCheckIn"] != DBNull.Value ? Convert.ToBoolean(reader["CanEditCheckIn"]) : (bool?)null,
                                    Hold = reader["Hold"] != DBNull.Value ? Convert.ToBoolean(reader["Hold"]) : (bool?)null,
                                    CurrentLocation = reader["CurrentLocation"] != DBNull.Value ? reader["CurrentLocation"].ToString() : null,
                                    CustomerLotNumber = reader["CustomerLotNumber"] != DBNull.Value ? Convert.ToString(reader["CustomerLotNumber"]) : null,
                                    Device = reader["Device"] != DBNull.Value ? reader["Device"].ToString() : null
                                };
                                checkInCheckOutLotDetails.Add(checkInCheckOutLotDetail);
                            }
                            catch
                            {
                                throw;
                            }
                        }
                        if (count == null)
                        {
                            return checkInCheckOutLotDetails;
                        }
                    } while (await reader.NextResultAsync());
                }
            }
            return checkInCheckOutLotDetails!;
        }

        public async Task<List<CheckInCheckOutLotDetails>> GetLastTenCheckOutLotDetailsAsync()
        {
            List<CheckInCheckOutLotDetails> checkInCheckOutLotDetails = new List<CheckInCheckOutLotDetails>();
            using (var connection = new SqlConnection(_configuration.GetConnectionString("InventoryConnection")))
            {
                await connection.OpenAsync();
                string checkOutQuery = "select top 10 *from PRD_CR";
                using (var command = new SqlCommand(checkOutQuery, connection))
                {
                    command.CommandType = CommandType.Text;
                    using var reader = await command.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        try
                        {
                            var checkInCheckOutLotDetail = new CheckInCheckOutLotDetails
                            {
                                //    RequestId = reader["RequestId"] != DBNull.Value ? Convert.ToInt32(reader["RequestId"]) : (int?)null,
                                //    RequestedBy = reader["RequestedBy"] != DBNull.Value ? reader["RequestedBy"].ToString() : null,
                                //    RequestedOn = reader["RequestedOn"] != DBNull.Value ? Convert.ToDateTime(reader["RequestedOn"]) : DateTime.MinValue,
                                CRId = reader["CRId"] != DBNull.Value ? Convert.ToInt32(reader["CRId"]) : (int?)null,
                                LotId = reader["LotId"] != DBNull.Value ? Convert.ToInt32(reader["LotId"]) : (int?)null,
                                ReceivingNo = reader["ReceivingOrMailId"] != DBNull.Value ? reader["ReceivingOrMailId"].ToString() : null,
                                CustomerName = reader["CustomerName"] != DBNull.Value ? reader["CustomerName"].ToString() : null,
                                ISELotNumber = reader["ISELotNumber"] != DBNull.Value ? reader["ISELotNumber"].ToString() : null,
                                TotalCount = reader["TotalCount"] != DBNull.Value ? Convert.ToInt32(reader["TotalCount"]) : (int?)null,
                                AvailableCount = reader["AvailableCount"] != DBNull.Value ? Convert.ToInt32(reader["AvailableCount"]) : (int?)null,
                                RunningCount = reader["RunningCount"] != DBNull.Value ? Convert.ToInt32(reader["RunningCount"]) : (int?)null,
                                GoodsType = reader["GoodsType"] != DBNull.Value ? reader["GoodsType"].ToString() : null,
                                State = reader["State"] != DBNull.Value ? reader["State"].ToString() : null,
                                AreaCode = reader["AreaCode"] != DBNull.Value ? reader["AreaCode"].ToString() : null,
                                LocationName = reader["LocationName"] != DBNull.Value ? reader["LocationName"].ToString() : null,
                                CanCheckIn = reader["CanCheckIn"] != DBNull.Value ? Convert.ToBoolean(reader["CanCheckIn"]) : (bool?)null,
                                CanEditCheckIn = reader["CanEditCheckIn"] != DBNull.Value ? Convert.ToBoolean(reader["CanEditCheckIn"]) : (bool?)null,
                                Hold = reader["Hold"] != DBNull.Value ? Convert.ToBoolean(reader["Hold"]) : (bool?)null,
                                CurrentLocation = reader["CurrentLocation"] != DBNull.Value ? reader["CurrentLocation"].ToString() : null,
                                CustomerLotNumber = reader["CustomerLotNumber"] != DBNull.Value ? Convert.ToString(reader["CustomerLotNumber"]) : null,
                                Device = reader["Device"] != DBNull.Value ? reader["Device"].ToString() : null
                            };
                            checkInCheckOutLotDetails.Add(checkInCheckOutLotDetail);
                        }
                        catch
                        {
                            throw;
                        }
                    }
                }
            }
            return checkInCheckOutLotDetails!;
        }

        public async Task<bool> SaveCheckInCheckOutRequest(string inputData, string requestType)
        {
            string? result = string.Empty;
            int requestId = 0;
            var checkInCheckOutRequest = requestType == "checkin" ? "SaveCheckInRequest" : requestType == "checkout" ? "SaveCheckOutRequest" : "";
            using (var connection = new SqlConnection(_configuration.GetConnectionString("InventoryConnection")))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand(checkInCheckOutRequest, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@JsonData", SqlDbType.NVarChar)
                    {
                        Value = inputData
                    });
                    try
                    {
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (reader.Read())
                            {
                                result = reader["Result"] != DBNull.Value ? reader["Result"].ToString() : "";
                                requestId = Convert.ToInt32(reader["RequestId"]);
                            }
                        }
                    }
                    catch
                    {
                        throw;
                    }
                }
            }
            return requestId > 0;
        }

        public async Task<List<CheckInCheckOutLotDetails>> GetLotsPendingApprovalAsync(string requestType, int requestId)
        {
            List<CheckInCheckOutLotDetails> checkInCheckOutLotDetails = new List<CheckInCheckOutLotDetails>();
            using (var connection = new SqlConnection(_configuration.GetConnectionString("InventoryConnection")))
            {
                await connection.OpenAsync();
                string checkInCheckOutProc = requestType.Equals("CheckIn", StringComparison.OrdinalIgnoreCase) ? "ICR_GetCheckInLots" : requestType.Equals("CheckOut", StringComparison.OrdinalIgnoreCase) ? "ICR_GetCheckOutLots" : "";
                using (var command = new SqlCommand(checkInCheckOutProc, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@RequestId", SqlDbType.Int)
                    {
                        Value = requestId
                    });
                    using var reader = await command.ExecuteReaderAsync();
                    checkInCheckOutLotDetails = new List<CheckInCheckOutLotDetails>();
                    List<CheckInCheckOutLocation> checkInCheckOutLocations = new List<CheckInCheckOutLocation>();
                    while (await reader.ReadAsync())
                    {
                        try
                        {
                            var checkInCheckOutLotDetail = new CheckInCheckOutLotDetails
                            {
                                RequestId = reader["RequestId"] != DBNull.Value ? Convert.ToInt32(reader["RequestId"]) : (int?)null,
                                RequestedBy = reader["Requestor"] != DBNull.Value ? reader["Requestor"].ToString() : null,
                                LotId = reader["LotId"] != DBNull.Value ? Convert.ToInt32(reader["LotId"]) : (int?)null,
                                CustomerName = reader["CustomerName"] != DBNull.Value ? reader["CustomerName"].ToString() : null,
                                ISELotNumber = reader["ISELotNumber"] != DBNull.Value ? reader["ISELotNumber"].ToString() : null,
                                Quantity = reader["OurCount"] != DBNull.Value ? Convert.ToInt32(reader["OurCount"]) : (int?)null,
                                LocationId = requestType == "CheckIn" ? reader["LocationId"] != DBNull.Value ? Convert.ToInt32(reader["LocationId"]) : null : null,
                                AreaCode = requestType == "CheckIn" ? reader["AreaCode"] != DBNull.Value ? reader["AreaCode"].ToString() : null : null,
                                LotRequestId = reader["LotRequestId"] != DBNull.Value ? Convert.ToInt32(reader["LotRequestId"]) : (int?)null,
                            };
                            checkInCheckOutLotDetails.Add(checkInCheckOutLotDetail);
                        }
                        catch
                        {

                        }
                    }
                }
            }
            return checkInCheckOutLotDetails!;
        }

        public async Task<List<CheckInCheckOutLocation>> GetCheckInLocationsAsync(string requestType, int requestId)
        {
            List<CheckInCheckOutLocation> checkInCheckOutLocations = new List<CheckInCheckOutLocation>();
            using (var connection = new SqlConnection(_configuration.GetConnectionString("InventoryConnection")))
            {
                await connection.OpenAsync();
                string checkInLocationsProc = "ICR_GetCheckInLots";
                using (var command = new SqlCommand(checkInLocationsProc, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@RequestId", SqlDbType.Int)
                    {
                        Value = requestId
                    });
                    using var reader = await command.ExecuteReaderAsync();
                    if (await reader.NextResultAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            CheckInCheckOutLocation location = new CheckInCheckOutLocation()
                            {
                                AreaCode = reader["AreaCode"]?.ToString(),
                                LocationId = Convert.ToInt32(reader["LocationId"]),
                                LocationName = reader["LocationName"]?.ToString(),
                                LotId = Convert.ToInt32(reader["LotId"])
                            };
                            checkInCheckOutLocations.Add(location);
                        }
                    }
                }
            }
            return checkInCheckOutLocations;
        }

        public async Task<bool> ApproveCheckInCheckOut(string requestType, string inputData)
        {
            string? result = string.Empty;
            int rows = 0;
            var checkInCheckOutApproval = requestType == "CheckIn" ? "Ticketing_ApproveCheckIn" : requestType == "CheckOut" ? "Ticketing_ApproveCheckOut" : "";
            using (var connection = new SqlConnection(_configuration.GetConnectionString("InventoryConnection")))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand(checkInCheckOutApproval, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@JsonInput", SqlDbType.NVarChar)
                    {
                        Value = inputData
                    });
                    try
                    {
                        rows = await command.ExecuteNonQueryAsync();
                    }
                    catch
                    {
                        throw;
                    }
                }
            }
            return rows > 0;
        }
    }
}



