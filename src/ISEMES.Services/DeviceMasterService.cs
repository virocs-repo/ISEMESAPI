using ISEMES.Models;
using ISEMES.Repositories;
using System.Data;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace ISEMES.Services
{
    public class DeviceMasterService : IDeviceMasterService
    {
        private readonly IDeviceMasterRepository _repository;
        private readonly ILogger<DeviceMasterService> _logger;

        public DeviceMasterService(IDeviceMasterRepository repository, ILogger<DeviceMasterService> logger)
        {
            _repository = repository;
            _logger = logger;
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
                    DeviceFamilyId = SafeGetInt32(row, "DeviceFamilyId"),
                    DeviceFamilyName = SafeGetString(row, "DeviceFamily"),
                    CustomerDeviceFamily = SafeGetString(row, "CustomerDeviceFamily"),
                    CustomerID = SafeGetInt32(row, "CustomerId"),
                    CustomerName = SafeGetString(row, "CustomerName"),
                    Active = SafeGetBoolean(row, "Active"),
                    CreatedOn = SafeGetString(row, "CreatedOn"),
                    ModifiedOn = SafeGetString(row, "ModifiedOn")
                });
            }

            return result;
        }

        public async Task<List<DeviceResponse>> SearchDevice(int? customerID, int? deviceFamilyId, string? deviceName, bool? active)
        {
            var dataTable = await _repository.SearchDevice(customerID, deviceFamilyId, deviceName, active);
            var result = new List<DeviceResponse>();

            if (dataTable != null && dataTable.Rows != null)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    var aliasNames = SafeGetString(row, "AliasNames") ?? SafeGetString(row, "aliasNames");
                    var customerDevice = SafeGetString(row, "CustomerDevice") ?? SafeGetString(row, "customerDevice");
                    var deviceAlias = SafeGetString(row, "DeviceAlias") ?? SafeGetString(row, "deviceAlias");
                    
                    if (string.IsNullOrEmpty(deviceAlias) && string.IsNullOrEmpty(customerDevice))
                    {
                        deviceAlias = SafeGetString(row, "Alias") ?? SafeGetString(row, "alias");
                        customerDevice = SafeGetString(row, "CustomerDeviceName") ?? SafeGetString(row, "customerDeviceName");
                    }
                    
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
                        DeviceAlias = finalDeviceAlias,
                        UnitCost = SafeGetNullableDecimal(row, "UnitCost"),
                        DeviceTypeId = SafeGetNullableInt(row, "DeviceTypeId") ?? SafeGetNullableInt(row, "DeviceType") ?? SafeGetNullableInt(row, "LotTypeId"),
                        COOId = SafeGetNullableInt(row, "COOId") ?? SafeGetNullableInt(row, "CountryOfOriginId") ?? SafeGetNullableInt(row, "COO"),
                        PartTypeId = SafeGetNullableInt(row, "PartTypeId") ?? SafeGetNullableInt(row, "PartType"),
                        MaterialDescriptionId = SafeGetNullableInt(row, "MaterialDescriptionId"),
                        // USHTSCodeId: Stored procedure selects D.USHTSCodeId same as D.ECCNId
                        // Use same retrieval as ECCNId, convert 0 to null (valid IDs like 122 are preserved)
                        USHTSCodeId = SafeGetNullableIntOrZeroAsNull(row, "USHTSCodeId"),
                        ECCNId = SafeGetNullableInt(row, "ECCNId"),
                        LicenseExceptionId = SafeGetNullableInt(row, "LicenseExceptionId"),
                        RestrictedCountriesIds = SafeGetString(row, "RestrictedCountriesIds"),
                        ScheduleB = SafeGetBoolean(row, "ScheduleB"),
                        MSLId = SafeGetNullableInt(row, "MSLId") ?? SafeGetNullableInt(row, "MSL"),
                        PeakPackageBodyTemperatureId = SafeGetNullableInt(row, "PeakPackageBodyTemperatureId") ?? SafeGetNullableInt(row, "PeakPacckageBody"),
                        ShelfLifeMonthId = SafeGetNullableInt(row, "ShelfLifeId") ?? SafeGetNullableInt(row, "ShelfLife"),
                        FloorLifeId = SafeGetNullableInt(row, "FloorLifeId") ?? SafeGetNullableInt(row, "FloorLife"),
                        PBFreeId = SafeGetNullableInt(row, "PBFreeId") ?? SafeGetNullableInt(row, "PBFree"),
                        PBFreeStickerId = SafeGetNullableInt(row, "PBFreeStickerId") ?? SafeGetNullableInt(row, "PBFreeSticker"),
                        ROHSId = SafeGetNullableInt(row, "ROHSId") ?? SafeGetNullableInt(row, "ROHS"),
                        TrayTubeStrappingId = SafeGetNullableInt(row, "TrayStrappingId") ?? SafeGetNullableInt(row, "TrayStrapping"),
                        TrayStackingId = SafeGetNullableInt(row, "TrayStackingId") ?? SafeGetNullableInt(row, "TrayStacking"),
                        IsLabelMapped = SafeGetBoolean(row, "IsLabelMapped"),
                        IsDeviceBasedTray = SafeGetBoolean(row, "IsDeviceBasedTray"),
                        SKU = SafeGetString(row, "SKU")
                    });
                }
            }

            return result;
        }

        private int SafeGetInt32(DataRow row, string columnName)
        {
            if (row.Table.Columns.Contains(columnName) && row[columnName] != DBNull.Value)
            {
                if (int.TryParse(row[columnName].ToString(), out int value))
                {
                    return value;
                }
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
            // First try exact match (case-sensitive)
            if (row.Table.Columns.Contains(columnName))
            {
                if (row[columnName] != DBNull.Value)
                {
                    if (int.TryParse(row[columnName].ToString(), out int value))
                    {
                        return value;
                    }
                }
                // Column exists but value is DBNull or can't parse - return null
                return null;
            }
            
            // Fallback: try case-insensitive match (handles stored procedure column name variations)
            foreach (DataColumn col in row.Table.Columns)
            {
                if (string.Equals(col.ColumnName, columnName, StringComparison.OrdinalIgnoreCase))
                {
                    if (row[col.ColumnName] != DBNull.Value)
                    {
                        if (int.TryParse(row[col.ColumnName].ToString(), out int value))
                        {
                            return value;
                        }
                    }
                    // Found matching column but value is DBNull or can't parse - return null
                    return null;
                }
            }
            
            // Column not found - return null
            return null;
        }

        private int? SafeGetNullableIntOrZeroAsNull(DataRow row, string columnName)
        {
            // For USHTSCodeId: Convert 0 to null to match TFS behavior where 0 means "not set"
            // TFS treats 0 as "not set" and displays -1 (--Select--) in the dropdown
            // Valid IDs (> 0) are returned as-is
            var value = SafeGetNullableInt(row, columnName);
            // Only convert 0 to null; preserve valid IDs and null values
            if (value.HasValue && value.Value == 0)
                return null;
            return value;
        }

        private int? ConvertZeroToNull(int? value)
        {
            // Convert 0 to null for display (frontend shows --Select-- when null)
            // Valid IDs (> 0) are preserved as-is
            if (value.HasValue && value.Value == 0)
                return null;
            return value;
        }

        private int? SafeGetNullableIntOrZeroAsNullWithFallback(DataRow row, params string[] columnNames)
        {
            // Try each column name until we find one that exists in the table (case-insensitive)
            // Once we find an existing column, use its value (even if it's 0, which gets converted to null)
            foreach (var columnName in columnNames)
            {
                // First try exact match (case-sensitive)
                if (row.Table.Columns.Contains(columnName))
                {
                    var value = SafeGetNullableIntOrZeroAsNull(row, columnName);
                    // Return the value (could be null if 0, or the actual ID if > 0)
                    return value;
                }
                
                // Then try case-insensitive match
                foreach (DataColumn col in row.Table.Columns)
                {
                    if (string.Equals(col.ColumnName, columnName, StringComparison.OrdinalIgnoreCase))
                    {
                        var value = SafeGetNullableIntOrZeroAsNull(row, col.ColumnName);
                        return value;
                    }
                }
            }
            
            // Last resort: search all columns for any that contain "USHTSCode" or "USHTS" (case-insensitive)
            // This handles cases where the column name might be slightly different (e.g., "US_HTS_Code_Id")
            foreach (DataColumn col in row.Table.Columns)
            {
                var colName = col.ColumnName;
                if (colName.IndexOf("USHTSCode", StringComparison.OrdinalIgnoreCase) >= 0 ||
                    colName.IndexOf("USHTS", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    // Read the raw value first to see what we get
                    var rawValue = SafeGetNullableInt(row, colName);
                    if (rawValue.HasValue)
                    {
                        // Apply zero-to-null conversion
                        return rawValue.Value == 0 ? null : rawValue;
                    }
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

        private bool? SafeGetNullableBoolean(DataRow row, string columnName)
        {
            if (row.Table.Columns.Contains(columnName) && row[columnName] != DBNull.Value)
            {
                if (bool.TryParse(row[columnName].ToString(), out bool value))
                {
                    return value;
                }
                var strValue = row[columnName].ToString()?.ToLower();
                if (strValue == "true" || strValue == "1")
                {
                    return true;
                }
                if (strValue == "false" || strValue == "0")
                {
                    return false;
                }
            }
            return null;
        }

        private DateTime? SafeGetNullableDateTime(DataRow row, string columnName)
        {
            if (row.Table.Columns.Contains(columnName) && row[columnName] != DBNull.Value)
            {
                if (DateTime.TryParse(row[columnName].ToString(), out DateTime value))
                {
                    return value;
                }
            }
            return null;
        }

        public async Task<List<Country>> GetRestrictedCountries()
        {
            return await _repository.GetRestrictedCountries();
        }

        public async Task<List<CustomerLabel>> GetCustomerLabelList(string customerName)
        {
            return await _repository.GetCustomerLabelList(customerName);
        }

        public async Task<LabelDetailsResponse> GetLabelDetails(int customerId, int deviceId, string labelName, string lotNum)
        {
            return await _repository.GetLabelDetails(customerId, deviceId, labelName, lotNum);
        }

        public async Task<int> AddOrUpdateLabelDetails(int customerId, int deviceId, string labelName, string input)
        {
            return await _repository.AddOrUpdateLabelDetails(customerId, deviceId, labelName, input);
        }

        public async Task<DeviceInfoResponse> GetDeviceInfo(int deviceId)
        {
            var dataSet = await _repository.GetDeviceInfo(deviceId);
            var response = new DeviceInfoResponse();
            
            // Map device fields from Table[0] - #GetDevices
            if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                var row = dataSet.Tables[0].Rows[0];
                var table = dataSet.Tables[0];
                
                // CanEdit flags
                response.CanEdit = SafeGetBoolean(row, "CanEdit");
                response.CanEditlotType = SafeGetBoolean(row, "CanEditlotType");
                response.LastModifiedOn = SafeGetStringNullable(row, "LastModifiedOn");
                
                // Device basic info
                response.DeviceId = SafeGetNullableInt(row, "DeviceId");
                response.DeviceFamilyID = SafeGetNullableInt(row, "DeviceFamilyID");
                response.DeviceFamily = SafeGetStringNullable(row, "DeviceFamily");
                response.Device = SafeGetStringNullable(row, "Device");
                response.TestDevice = SafeGetStringNullable(row, "TestDevice");
                response.ReliabilityDevice = SafeGetStringNullable(row, "ReliabilityDevice");
                
                // Device IDs
                response.PartTypeId = SafeGetNullableInt(row, "PartTypeId");
                response.MaterialDescriptionId = SafeGetNullableInt(row, "MaterialDescriptionId");
                
                // Debug logging: Check what columns exist and their values
                var availableColumns = string.Join(", ", row.Table.Columns.Cast<DataColumn>().Select(c => c.ColumnName));
                _logger.LogInformation($"GetDeviceInfo - Available columns: {availableColumns}");
                
                // Check raw values before conversion
                var rawUSHTSCodeId = SafeGetNullableInt(row, "USHTSCodeId");
                var rawECCNId = SafeGetNullableInt(row, "ECCNId");
                _logger.LogInformation($"GetDeviceInfo - Raw USHTSCodeId: {rawUSHTSCodeId}, Raw ECCNId: {rawECCNId}");
                
                // USHTSCodeId: Stored procedure selects D.USHTSCodeId same as D.ECCNId
                // Use same retrieval as ECCNId, convert 0 to null (valid IDs like 122 are preserved)
                response.USHTSCodeId = SafeGetNullableIntOrZeroAsNull(row, "USHTSCodeId");
                response.ECCNId = SafeGetNullableInt(row, "ECCNId");
                
                _logger.LogInformation($"GetDeviceInfo - Final USHTSCodeId: {response.USHTSCodeId}, Final ECCNId: {response.ECCNId}");
                response.ECCN = SafeGetStringNullable(row, "ECCN");
                response.USHTSCodeId = SafeGetNullableIntOrZeroAsNull(row, "USHTSCodeId");
                response.LicenseExceptionId = SafeGetNullableInt(row, "LicenseExceptionId");
                response.RestrictedCountriesIds = SafeGetStringNullable(row, "RestrictedCountriesIds");
                response.ScheduleB = SafeGetNullableBoolean(row, "ScheduleB");
                response.Active = SafeGetNullableBoolean(row, "Active");
                response.UnitCost = SafeGetNullableDecimal(row, "UnitCost");
                response.IsLabelMapped = SafeGetNullableBoolean(row, "IsLabelMapped");
                response.SKU = SafeGetStringNullable(row, "SKU");
                response.IsDeviceBasedTray = SafeGetNullableBoolean(row, "IsDeviceBasedTray");
                
                // Pack&Label Info IDs
                response.MSL = SafeGetNullableInt(row, "MSL");
                response.PeakPacckageBody = SafeGetNullableInt(row, "PeakPacckageBody");
                response.ShelfLife = SafeGetNullableInt(row, "ShelfLife");
                response.FloorLife = SafeGetNullableInt(row, "FloorLife");
                response.PBFree = SafeGetNullableInt(row, "PBFree");
                response.PBFreeSticker = SafeGetNullableInt(row, "PBFreeSticker");
                response.ROHS = SafeGetNullableInt(row, "ROHS");
                response.TrayStrapping = SafeGetNullableInt(row, "TrayStrapping");
                response.TrayStacking = SafeGetNullableInt(row, "TrayStacking");
                response.DeviceTypeId = SafeGetNullableInt(row, "DeviceTypeId");
                response.COOId = SafeGetNullableInt(row, "COOId");
                
                // Audit fields
                response.CreatedOn = SafeGetNullableDateTime(row, "CreatedOn");
                response.CreatedBy = SafeGetStringNullable(row, "CreatedBy");
                response.LastModifiedBy = SafeGetStringNullable(row, "LastModifiedBy");
            }
            
            // Map label details from Table[5] - @LabelDetails
            if (dataSet.Tables.Count > 5 && dataSet.Tables[5].Rows.Count > 0)
            {
                var labelRow = dataSet.Tables[5].Rows[0];
                response.LabelCustomer = SafeGetStringNullable(labelRow, "Customer");
                response.LabelDevice = SafeGetStringNullable(labelRow, "Device");
                response.Label1 = SafeGetStringNullable(labelRow, "label1");
                response.Label2 = SafeGetStringNullable(labelRow, "label2");
                response.Label3 = SafeGetStringNullable(labelRow, "label3");
                response.Label4 = SafeGetStringNullable(labelRow, "label4");
                response.Label5 = SafeGetStringNullable(labelRow, "label5");
                
                // Update CanEditLabel flags from label details if available
                response.CanEditLabel1 = SafeGetBoolean(labelRow, "CanEditlabel1");
                response.CanEditLabel2 = SafeGetBoolean(labelRow, "CanEditlabel2");
                response.CanEditLabel3 = SafeGetBoolean(labelRow, "CanEditlabel3");
                response.CanEditLabel4 = SafeGetBoolean(labelRow, "CanEditlabel4");
                response.CanEditLabel5 = SafeGetBoolean(labelRow, "CanEditlabel5");
            }
            
            // Map usage data tables
            if (dataSet.Tables.Count > 1 && dataSet.Tables[1].Rows.Count > 0)
            {
                response.DQP = ConvertDataTableToList(dataSet.Tables[1]);
            }
            
            if (dataSet.Tables.Count > 2 && dataSet.Tables[2].Rows.Count > 0)
            {
                response.MF = ConvertDataTableToList(dataSet.Tables[2]);
            }
            
            if (dataSet.Tables.Count > 3 && dataSet.Tables[3].Rows.Count > 0)
            {
                response.TRV = ConvertDataTableToList(dataSet.Tables[3]);
            }
            
            if (dataSet.Tables.Count > 4 && dataSet.Tables[4].Rows.Count > 0)
            {
                response.Boards = ConvertDataTableToList(dataSet.Tables[4]);
            }
            
            if (dataSet.Tables.Count > 6 && dataSet.Tables[6].Rows.Count > 0)
            {
                response.DeviceLabelInfo = ConvertDataTableToList(dataSet.Tables[6]);
            }
            
            return response;
        }

        private List<Dictionary<string, object>> ConvertDataTableToList(DataTable dataTable)
        {
            var list = new List<Dictionary<string, object>>();
            foreach (DataRow row in dataTable.Rows)
            {
                var dict = new Dictionary<string, object>();
                foreach (DataColumn column in dataTable.Columns)
                {
                    if (row[column] != DBNull.Value)
                    {
                        dict[column.ColumnName] = row[column];
                    }
                    else
                    {
                        dict[column.ColumnName] = null;
                    }
                }
                list.Add(dict);
            }
            return list;
        }
    }
}
