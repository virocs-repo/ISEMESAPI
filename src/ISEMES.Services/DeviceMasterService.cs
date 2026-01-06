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
                        UnitCost = SafeGetNullableDecimal(row, "UnitCost")
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
            
            if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                var row = dataSet.Tables[0].Rows[0];
                var table = dataSet.Tables[0];
                response.CanEdit = SafeGetBoolean(row, "CanEdit");
                
                if (table.Columns.Contains("LastModifiedOn"))
                {
                    response.LastModifiedOn = SafeGetString(row, "LastModifiedOn");
                }
                
                if (table.Columns.Contains("CanEditlotType"))
                {
                    response.CanEditlotType = SafeGetBoolean(row, "CanEditlotType");
                }
                
                for (int i = 1; i <= 5; i++)
                {
                    var columnName = $"CanEditlabel{i}";
                    if (table.Columns.Contains(columnName))
                    {
                        switch (i)
                        {
                            case 1: response.CanEditLabel1 = SafeGetBoolean(row, columnName); break;
                            case 2: response.CanEditLabel2 = SafeGetBoolean(row, columnName); break;
                            case 3: response.CanEditLabel3 = SafeGetBoolean(row, columnName); break;
                            case 4: response.CanEditLabel4 = SafeGetBoolean(row, columnName); break;
                            case 5: response.CanEditLabel5 = SafeGetBoolean(row, columnName); break;
                        }
                    }
                }
            }
            
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
