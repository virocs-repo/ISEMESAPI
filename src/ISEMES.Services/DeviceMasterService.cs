using ISEMES.Models;
using ISEMES.Repositories;
using System.Data;

namespace ISEMES.Services
{
    public class DeviceMasterService : IDeviceMasterService
    {
        private readonly IDeviceMasterRepository _repository;

        public DeviceMasterService(IDeviceMasterRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> AddUpdateDeviceFamily(DeviceFamilyRequest request)
        {
            return await _repository.AddUpdateDeviceFamily(request);
        }

        public async Task<int> AddUpdateDevice(DeviceMasterRequest request)
        {
            return await _repository.AddUpdateDevice(request);
        }

        public async Task<List<DeviceFamilyResponse>> SearchDeviceFamily(int? customerID, string? deviceFamilyName, bool? active)
        {
            var dataTable = await _repository.SearchDeviceFamily(customerID, deviceFamilyName, active);
            var result = new List<DeviceFamilyResponse>();

            foreach (DataRow row in dataTable.Rows)
            {
                result.Add(new DeviceFamilyResponse
                {
                    DeviceFamilyId = row.Table.Columns.Contains("DeviceFamilyId") && row["DeviceFamilyId"] != DBNull.Value ? Convert.ToInt32(row["DeviceFamilyId"]) : 0,
                    DeviceFamilyName = row.Table.Columns.Contains("DeviceFamily") && row["DeviceFamily"] != DBNull.Value ? row["DeviceFamily"].ToString() ?? string.Empty : string.Empty,
                    CustomerDeviceFamily = row.Table.Columns.Contains("CustomerDeviceFamily") && row["CustomerDeviceFamily"] != DBNull.Value ? row["CustomerDeviceFamily"].ToString() : string.Empty,
                    CustomerID = row.Table.Columns.Contains("CustomerId") && row["CustomerId"] != DBNull.Value ? Convert.ToInt32(row["CustomerId"]) : 0,
                    CustomerName = row.Table.Columns.Contains("CustomerName") && row["CustomerName"] != DBNull.Value ? row["CustomerName"].ToString() ?? string.Empty : string.Empty,
                    Active = row.Table.Columns.Contains("Active") && row["Active"] != DBNull.Value && Convert.ToBoolean(row["Active"]),
                    CreatedOn = row.Table.Columns.Contains("CreatedOn") && row["CreatedOn"] != DBNull.Value ? row["CreatedOn"].ToString() ?? string.Empty : string.Empty,
                    ModifiedOn = row.Table.Columns.Contains("ModifiedOn") && row["ModifiedOn"] != DBNull.Value ? row["ModifiedOn"].ToString() ?? string.Empty : string.Empty
                });
            }

            return result;
        }

        public async Task<List<DeviceResponse>> SearchDevice(int? customerID, int? deviceFamilyId, string? deviceName, bool? active)
        {
            try
            {
                var dataTable = await _repository.SearchDevice(customerID, deviceFamilyId, deviceName, active);
                var result = new List<DeviceResponse>();

                if (dataTable != null && dataTable.Rows != null)
                {
                    // Log available columns for debugging
                    if (dataTable.Rows.Count > 0)
                    {
                        var columns = string.Join(", ", dataTable.Columns.Cast<DataColumn>().Select(c => c.ColumnName));
                        Console.WriteLine($"Available columns in Device search result: {columns}");
                    }

                    foreach (DataRow row in dataTable.Rows)
                    {
                        try
                        {
                            // Read AliasNames from stored procedure (comma-separated string)
                            // The stored procedure PRD_Device_P_Search returns AliasNames column
                            var aliasNames = SafeGetString(row, "AliasNames") ?? SafeGetString(row, "aliasNames");
                            
                            // Try multiple possible column names for device alias (fallback)
                            var customerDevice = SafeGetString(row, "CustomerDevice") ?? SafeGetString(row, "customerDevice");
                            var deviceAlias = SafeGetString(row, "DeviceAlias") ?? SafeGetString(row, "deviceAlias");
                            
                            // If still empty, try other possible column names
                            if (string.IsNullOrEmpty(deviceAlias) && string.IsNullOrEmpty(customerDevice))
                            {
                                deviceAlias = SafeGetString(row, "Alias") ?? SafeGetString(row, "alias");
                                customerDevice = SafeGetString(row, "CustomerDeviceName") ?? SafeGetString(row, "customerDeviceName");
                            }
                            
                            // Use AliasNames from stored procedure if available, otherwise fallback to DeviceAlias or CustomerDevice
                            var finalDeviceAlias = !string.IsNullOrEmpty(aliasNames) ? aliasNames : 
                                                  (!string.IsNullOrEmpty(deviceAlias) ? deviceAlias : customerDevice);
                            
                            result.Add(new DeviceResponse
                            {
                                DeviceId = SafeGetInt32(row, "DeviceId"),
                                Device = SafeGetString(row, "Device"),
                                DeviceFamilyId = SafeGetNullableInt(row, "DeviceFamilyId"),
                                DeviceFamily = SafeGetString(row, "DeviceFamily"),
                                CustomerDevice = customerDevice,
                                CustomerId = SafeGetNullableInt(row, "CustomerId"),
                                CustomerName = SafeGetString(row, "CustomerName"),
                                Active = SafeGetBoolean(row, "Active"),
                                CreatedOn = SafeGetString(row, "CreatedOn"),
                                ModifiedOn = SafeGetString(row, "ModifiedOn"),
                                TestDevice = SafeGetString(row, "TestDevice"),
                                ReliabilityDevice = SafeGetString(row, "ReliabilityDevice"),
                                // Use AliasNames from stored procedure (comma-separated), or fallback to DeviceAlias/CustomerDevice
                                DeviceAlias = finalDeviceAlias,
                                UnitCost = SafeGetNullableDecimal(row, "UnitCost")
                            });
                        }
                        catch (Exception ex)
                        {
                            // Log row-level error and continue with next row
                            Console.WriteLine($"Error processing row: {ex.Message}");
                            continue;
                        }
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SearchDevice: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                throw;
            }
        }

        private int SafeGetInt32(DataRow row, string columnName)
        {
            if (row.Table.Columns.Contains(columnName) && row[columnName] != DBNull.Value)
            {
                int.TryParse(row[columnName].ToString(), out int value);
                return value;
            }
            return 0;
        }

        private string SafeGetString(DataRow row, string columnName)
        {
            if (row.Table.Columns.Contains(columnName) && row[columnName] != DBNull.Value)
            {
                return row[columnName].ToString() ?? string.Empty;
            }
            return string.Empty;
        }

        private string? SafeGetStringNullable(DataRow row, string columnName)
        {
            if (row.Table.Columns.Contains(columnName) && row[columnName] != DBNull.Value)
            {
                var value = row[columnName].ToString();
                return string.IsNullOrEmpty(value) ? null : value;
            }
            return null;
        }

        private int? SafeGetNullableInt(DataRow row, string columnName)
        {
            if (row.Table.Columns.Contains(columnName) && row[columnName] != DBNull.Value)
            {
                if (int.TryParse(row[columnName].ToString(), out int value))
                {
                    return value;
                }
            }
            return null;
        }

        private bool SafeGetBoolean(DataRow row, string columnName)
        {
            if (row.Table.Columns.Contains(columnName) && row[columnName] != DBNull.Value)
            {
                if (bool.TryParse(row[columnName].ToString(), out bool value))
                {
                    return value;
                }
                // Try converting 1/0 or "true"/"false" strings
                var strValue = row[columnName].ToString()?.ToLower();
                return strValue == "true" || strValue == "1";
            }
            return false;
        }

        private decimal? SafeGetNullableDecimal(DataRow row, string columnName)
        {
            if (row.Table.Columns.Contains(columnName) && row[columnName] != DBNull.Value)
            {
                if (decimal.TryParse(row[columnName].ToString(), out decimal value))
                {
                    return value;
                }
            }
            return null;
        }
    }
}
