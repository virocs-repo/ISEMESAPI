using ISEMES.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Text;
using System.Text.Json;
using System.Xml.Serialization;

namespace ISEMES.Repositories
{
    public class SplitMergeRepository : ISplitMergeRepository
    {
        private readonly string? _connectString;
        private readonly string? _inventoryConnectString;

        public SplitMergeRepository(IConfiguration configuration)
        {
            _connectString = configuration.GetConnectionString("InventoryTFS_Prod2Connection");
            _inventoryConnectString = configuration.GetConnectionString("InventoryConnection");
        }

        public async Task<List<MasterList>> GetMasterListItems(string listName, int? serviceId)
        {
            List<MasterList> result = new List<MasterList>();
            using (var connection = new SqlConnection(_inventoryConnectString))
            {
                using (var command = new SqlCommand("PRD_P_GetMasterListItems", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@ListName", listName);
                    command.Parameters.AddWithValue("@ServiceId", serviceId);
                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var record = new MasterList();
                            record.MasterListItemId = reader.GetInt32(0);
                            record.MasterListId = reader.GetInt32(1);
                            record.ItemText = reader.GetString(2);
                            record.ItemValue = reader.GetString(3);
                            result.Add(record);
                        }
                    }
                }
            }
            return result;
        }

        public async Task<List<MasterList>> GetLotStatus()
        {
            return await GetMasterListItems("LotStatus", null);
        }

        public async Task<List<TFSCustomer>> GetTFSCustomers()
        {
            List<TFSCustomer> result = new List<TFSCustomer>();
            using (var connection = new SqlConnection(_connectString))
            {
                using (var command = new SqlCommand("PRD_P_GetCustomers", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    await connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            result.Add(new TFSCustomer
                            {
                                CustomerID = reader.GetInt32(reader.GetOrdinal("CustomerId")),
                                CustomerName = reader.IsDBNull(reader.GetOrdinal("CustomerName")) ? "" : reader.GetString(reader.GetOrdinal("CustomerName"))
                            });
                        }
                    }
                }
            }
            return result;
        }

        public async Task<List<TFSDeviceFamily>> GetDeviceFamilies(int customerId)
        {
            List<TFSDeviceFamily> result = new List<TFSDeviceFamily>();
            using (var connection = new SqlConnection(_connectString))
            {
                using (var command = new SqlCommand("PRD_P_GetDeviceFamilies", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CustomerId", customerId);
                    await connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            result.Add(new TFSDeviceFamily
                            {
                                DeviceFamilyId = reader.GetInt32(reader.GetOrdinal("DeviceFamilyId")),
                                DeviceFamilyName = reader.IsDBNull(reader.GetOrdinal("DeviceFamilyName")) ? "" : reader.GetString(reader.GetOrdinal("DeviceFamilyName"))
                            });
                        }
                    }
                }
            }
            return result;
        }

        public async Task<List<TFSDevice>> GetDevices(int customerId, int? deviceFamiltyId, int? deviceId)
        {
            List<TFSDevice> result = new List<TFSDevice>();
            using (var connection = new SqlConnection(_connectString))
            {
                using (var command = new SqlCommand("PRD_P_GetDevices", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CustomerId", customerId);
                    command.Parameters.AddWithValue("@DeviceFamilyId", deviceFamiltyId ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@DeviceId", deviceId ?? (object)DBNull.Value);
                    await connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            result.Add(new TFSDevice
                            {
                                DeviceId = reader.GetInt32(reader.GetOrdinal("DeviceId")),
                                Device = reader.IsDBNull(reader.GetOrdinal("Device")) ? "" : reader.GetString(reader.GetOrdinal("Device"))
                            });
                        }
                    }
                }
            }
            return result;
        }

        public async Task<List<TFSDeviceAlias>> GetDeviceAlias(int customerId, int deviceFamiltyId, int deviceId, string? source)
        {
            List<TFSDeviceAlias> result = new List<TFSDeviceAlias>();
            using (var connection = new SqlConnection(_connectString))
            {
                using (var command = new SqlCommand("PRD_P_GetDeviceAlias", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CustomerId", customerId);
                    command.Parameters.AddWithValue("@DeviceFamilyId", deviceFamiltyId);
                    command.Parameters.AddWithValue("@DeviceId", deviceId);
                    command.Parameters.AddWithValue("@Source", source ?? (object)DBNull.Value);
                    await connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            result.Add(new TFSDeviceAlias
                            {
                                AliasId = reader.GetInt32(reader.GetOrdinal("AliasId")),
                                AliasName = reader.IsDBNull(reader.GetOrdinal("AliasName")) ? "" : reader.GetString(reader.GetOrdinal("AliasName"))
                            });
                        }
                    }
                }
            }
            return result;
        }

        public async Task<List<LotOwner>> GetLotOwners()
        {
            List<LotOwner> result = new List<LotOwner>();
            using (var connection = new SqlConnection(_connectString))
            {
                using (var command = new SqlCommand("PRD_P_GetLotOwners", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    await connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            result.Add(new LotOwner
                            {
                                OwnerId = reader.GetInt32(reader.GetOrdinal("OwnerId")),
                                EmployeeName = reader.IsDBNull(reader.GetOrdinal("EmployeeName")) ? "" : reader.GetString(reader.GetOrdinal("EmployeeName"))
                            });
                        }
                    }
                }
            }
            return result;
        }

        public async Task<List<LotSearch>> InventoryLotSearch(string? travellerStatusIds, string? lotStatusIds, DateTime? fromDate, DateTime? toDate)
        {
            List<LotSearch> result = new List<LotSearch>();
            using (var connection = new SqlConnection(_inventoryConnectString))
            {
                using (var command = new SqlCommand("Inv_Lot_Search", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@TravStatusIds", travellerStatusIds);
                    command.Parameters.AddWithValue("@LotStatusIds", lotStatusIds);
                    command.Parameters.AddWithValue("@FromDate", fromDate);
                    command.Parameters.AddWithValue("@ToDate", toDate);
                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var record = new LotSearch();
                            record.LotId = reader.GetInt32("LotId");
                            record.CustomerName = reader.IsDBNull(reader.GetOrdinal("CustomerName")) ? "" : reader.GetString("CustomerName");
                            record.ISELotNumber = reader.IsDBNull(reader.GetOrdinal("ISELotNumber")) ? "" : reader.GetString("ISELotNumber");
                            record.DeviceFamily = reader.IsDBNull(reader.GetOrdinal("DeviceFamily")) ? "" : reader.GetString("DeviceFamily");
                            record.Device = reader.IsDBNull(reader.GetOrdinal("Device")) ? "" : reader.GetString("Device");
                            record.TravelerStatus = reader.IsDBNull(reader.GetOrdinal("TravelerStatus")) ? "" : reader.GetString("TravelerStatus");
                            record.LotStatus = reader.IsDBNull(reader.GetOrdinal("LotStatus")) ? "" : reader.GetString("LotStatus");
                            record.CustomerLotNumber = reader.IsDBNull(reader.GetOrdinal("CustomerLotNumber")) ? "" : reader.GetString("CustomerLotNumber");
                            record.ExpectedCount = reader.IsDBNull(reader.GetOrdinal("ExpectedCount")) ? null : reader.GetInt32("ExpectedCount");
                            record.RunningCount = reader.IsDBNull(reader.GetOrdinal("RunningCount")) ? null : reader.GetInt32("RunningCount");
                            record.LotQty = reader.IsDBNull(reader.GetOrdinal("LotQty")) ? null : reader.GetInt32("LotQty");
                            record.ExpectedDate = reader.IsDBNull(reader.GetOrdinal("ExpectedDate")) ? "" : reader.GetString("ExpectedDate");
                            record.Identifier = reader.IsDBNull(reader.GetOrdinal("Identifier")) ? "" : reader.GetString("Identifier");
                            record.ReceivingNo = reader.IsDBNull(reader.GetOrdinal("ReceivingNo")) ? "" : reader.GetString("ReceivingNo");
                            record.ShippingMethod = reader.IsDBNull(reader.GetOrdinal("ShippingMethod")) ? "" : reader.GetString("ShippingMethod");
                            record.SOStatus = reader.IsDBNull(reader.GetOrdinal("SOStatus")) ? "" : reader.GetString("SOStatus");
                            record.CurrentStep = reader.IsDBNull(reader.GetOrdinal("CurrentStep")) ? "" : reader.GetString("CurrentStep");
                            record.NextStep = reader.IsDBNull(reader.GetOrdinal("NextStep")) ? "" : reader.GetString("NextStep");
                            record.CurrentLocation = reader.IsDBNull(reader.GetOrdinal("CurrentLocation")) ? "" : reader.GetString("CurrentLocation");

                            result.Add(record);
                        }
                    }
                }
            }
            return result;
        }

        public async Task<LotDetail> GetInventoryLot(int lotId, string source)
        {
            LotDetail result = new LotDetail();
            using (var connection = new SqlConnection(_connectString))
            {
                using (var command = new SqlCommand("PRD_P_GetInventoryLot", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@LotId", lotId);
                    command.Parameters.AddWithValue("@Source", source);
                    await connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            result.ISELotNumber = reader.IsDBNull(reader.GetOrdinal("ISELotNumber")) ? null : reader.GetString(reader.GetOrdinal("ISELotNumber"));
                        }
                    }
                }
            }
            return result;
        }

        public async Task<List<MergeLot>> GetMergeLots(int lotId)
        {
            List<MergeLot> result = new List<MergeLot>();
            return await Task.FromResult(result);
        }

        public async Task<List<IcrSearch>> IcrDashboardSearch(int? customerId, string? travellerStatusIds, string? lotStatusIds, DateTime? fromDate, DateTime? toDate, string? requestTypeIds)
        {
            List<IcrSearch> result = new List<IcrSearch>();
            return await Task.FromResult(result);
        }

        public async Task<List<MergedLots>> GetMatchedLots(int trvStepId)
        {
            List<MergedLots> result = new List<MergedLots>();
            return await Task.FromResult(result);
        }

        public async Task<bool> SaveMergeLots(LotMerge request)
        {
            return await Task.FromResult(true);
        }

        public async Task<int> AddOrUpdateMergeAsync(int? mergeId, int trvStepId, string? lotIds, int userId)
        {
            return await Task.FromResult(0);
        }

        public async Task<List<FutureSplitBin>> GetFutureSplitBinsAsync(int trvStepId)
        {
            List<FutureSplitBin> result = new List<FutureSplitBin>();
            return await Task.FromResult(result);
        }

        public async Task<List<SplitBin>> GetSplitBinsAsync(int lotId, int trvStepId, bool rejectBins)
        {
            List<SplitBin> result = new List<SplitBin>();
            return await Task.FromResult(result);
        }

        public async Task<List<FutureSplit>> GetFutureSplitsAsync(int trvStepId)
        {
            List<FutureSplit> result = new List<FutureSplit>();
            return await Task.FromResult(result);
        }

        public async Task<List<TotalSplitDto>> GetSplitsAsync(int trvStepId)
        {
            List<TotalSplitDto> result = new List<TotalSplitDto>();
            return await Task.FromResult(result);
        }

        public async Task<bool> AddOrUpdateSplit(SplitRequest splitRequest)
        {
            return await Task.FromResult(true);
        }

        public async Task<bool> AddOrUpdateFutureSplit(SplitRequest splitRequest)
        {
            return await Task.FromResult(true);
        }

        public async Task<SplitPreviewBOOutPut> GetPreviewDetails(int trvStepId, int lotId)
        {
            return await Task.FromResult(new SplitPreviewBOOutPut());
        }

        public async Task<SplitPreviewBOOutPut> GetFSPreviewDetails(int trvStepId)
        {
            return await Task.FromResult(new SplitPreviewBOOutPut());
        }

        public async Task<string> GenerateFutureSplit(int trvStepId)
        {
            return await Task.FromResult(string.Empty);
        }

        public async Task<string> GenerateSplit(int trvStepId, int userId)
        {
            return await Task.FromResult(string.Empty);
        }
    }
}
