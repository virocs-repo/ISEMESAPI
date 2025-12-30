using ISEMES.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;

namespace ISEMES.Repositories
{
    public class DeviceMasterRepository : IDeviceMasterRepository
    {
        private readonly TFSDbContext _context;
        private readonly IConfiguration _configuration;

        public DeviceMasterRepository(TFSDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<int> AddUpdateDeviceFamily(DeviceFamilyRequest request)
        {
            int retVal = 0;
            using (var connection = new SqlConnection(_configuration.GetConnectionString("InventoryTFS_Prod2Connection")))
            {
                connection.Open();
                using (var command = new SqlCommand("PRD_Device_P_AddOrUpdateDeviceFamily", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@DeviceFamilyId", request.DeviceFamilyId);
                    command.Parameters.AddWithValue("@DeviceFamily", request.DeviceFamilyName);
                    command.Parameters.AddWithValue("@CustomerId", request.CustomerID);
                    command.Parameters.AddWithValue("@UserId", request.CreatedBy);
                    command.Parameters.AddWithValue("@Active", request.IsActive);
                    
                    var returnCodeParam = new SqlParameter("@ReturnCode", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(returnCodeParam);

                    await command.ExecuteNonQueryAsync();
                    retVal = (int)returnCodeParam.Value;
                }
            }
            return retVal;
        }

        public async Task<int> AddUpdateDevice(DeviceMasterRequest request)
        {
            int retVal = 0;
            using (var connection = new SqlConnection(_configuration.GetConnectionString("InventoryTFS_Prod2Connection")))
            {
                connection.Open();
                using (var command = new SqlCommand("PRD_Device_P_AddOrUpdate", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@DeviceFamilyId", request.DeviceFamilyId);
                    command.Parameters.AddWithValue("@DeviceId", request.DeviceId);
                    command.Parameters.AddWithValue("@Device", request.DeviceName);
                    command.Parameters.AddWithValue("@UserId", request.CreatedBy);
                    command.Parameters.AddWithValue("@Active", request.IsActive);
                    
                    var returnCodeParam = new SqlParameter("@ReturnCode", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(returnCodeParam);

                    await command.ExecuteNonQueryAsync();
                    retVal = (int)returnCodeParam.Value;
                }
            }
            return retVal;
        }

        public async Task<DataTable> SearchDeviceFamily(int? customerID, string? deviceFamilyName, bool? active)
        {
            var dataTable = new DataTable();
            using (var connection = new SqlConnection(_configuration.GetConnectionString("InventoryTFS_Prod2Connection")))
            {
                connection.Open();
                using (var command = new SqlCommand("PRD_Device_P_SearchDeviceFamily", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    
                    if (customerID.HasValue && customerID.Value != -1)
                        command.Parameters.AddWithValue("@CustomerId", customerID.Value);
                    else
                        command.Parameters.AddWithValue("@CustomerId", DBNull.Value);
                    
                    if (!string.IsNullOrEmpty(deviceFamilyName))
                        command.Parameters.AddWithValue("@DeviceFamily", deviceFamilyName);
                    else
                        command.Parameters.AddWithValue("@DeviceFamily", DBNull.Value);

                    if (active.HasValue)
                        command.Parameters.AddWithValue("@Active", active.Value);
                    else
                        command.Parameters.AddWithValue("@Active", DBNull.Value);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        dataTable.Load(reader);
                    }
                }
            }
            return dataTable;
        }

        public async Task<DataTable> SearchDevice(int? customerID, int? deviceFamilyId, string? deviceName, bool? active)
        {
            var dataTable = new DataTable();
            using (var connection = new SqlConnection(_configuration.GetConnectionString("InventoryTFS_Prod2Connection")))
            {
                connection.Open();
                using (var command = new SqlCommand("PRD_Device_P_Search", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    
                    if (customerID.HasValue && customerID.Value != -1)
                        command.Parameters.AddWithValue("@CustomerId", customerID.Value);
                    else
                        command.Parameters.AddWithValue("@CustomerId", DBNull.Value);
                    
                    if (deviceFamilyId.HasValue && deviceFamilyId.Value != -1)
                        command.Parameters.AddWithValue("@DeviceFamilyId", deviceFamilyId.Value);
                    else
                        command.Parameters.AddWithValue("@DeviceFamilyId", DBNull.Value);
                    
                    // DeviceId parameter - using -1 for search (will be converted to DBNull)
                    command.Parameters.AddWithValue("@DeviceId", DBNull.Value);

                    if (active.HasValue)
                        command.Parameters.AddWithValue("@Active", active.Value);
                    else
                        command.Parameters.AddWithValue("@Active", DBNull.Value);

                    // DeviceName parameter - pass to stored procedure
                    if (!string.IsNullOrEmpty(deviceName))
                        command.Parameters.AddWithValue("@DeviceName", deviceName);
                    else
                        command.Parameters.AddWithValue("@DeviceName", DBNull.Value);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        dataTable.Load(reader);
                    }
                }
            }
            
            // Note: Device name filtering is now handled by the stored procedure via @DeviceName parameter
            // The client-side filtering below is kept as fallback but should not be needed
            if (!string.IsNullOrEmpty(deviceName) && dataTable.Rows.Count > 0 && dataTable.Columns.Contains("Device"))
            {
                var filteredTable = dataTable.Clone();
                foreach (DataRow row in dataTable.Rows)
                {
                    var deviceValue = row["Device"]?.ToString();
                    if (!string.IsNullOrEmpty(deviceValue) && 
                        deviceValue.Contains(deviceName, StringComparison.OrdinalIgnoreCase))
                    {
                        filteredTable.ImportRow(row);
                    }
                }
                return filteredTable;
            }
            
            return dataTable;
        }

        public Task<int> AddUpdateDevice(DeviceRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
