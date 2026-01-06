using ISEMES.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Text;
using System.Xml.Serialization;

namespace ISEMES.Repositories
{
    public class DeviceMasterRepository : IDeviceMasterRepository
    {
        private readonly IConfiguration _configuration;
        private const string ConnectionStringName = "InventoryTFS_Prod2Connection";

        public DeviceMasterRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private string GetConnectionString()
        {
            return _configuration.GetConnectionString(ConnectionStringName) 
                ?? throw new InvalidOperationException($"Connection string '{ConnectionStringName}' not found.");
        }

        public async Task<int> AddUpdateDeviceFamily(DeviceFamilyRequest request)
        {
            using (var connection = new SqlConnection(GetConnectionString()))
            {
                await connection.OpenAsync();
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
                    return (int)(returnCodeParam.Value ?? 0);
                }
            }
        }

        public async Task<int> AddUpdateDevice(DeviceMasterRequest request)
        {
            using (var connection = new SqlConnection(GetConnectionString()))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("PRD_Device_P_AddOrUpdate", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    
                    // Clean up nullable fields before serialization to prevent DBNull casting errors
                    CleanupNullableFields(request);
                    
                    // Serialize the request object to XML (matching TFS implementation)
                    string deviceXml = SerializeToXml(request);
                    
                    // Log XML for debugging (can be removed in production)
                    // System.Diagnostics.Debug.WriteLine($"Device XML: {deviceXml}");
                    
                    command.Parameters.AddWithValue("@DeviceXML", deviceXml);
                    
                    var returnCodeParam = new SqlParameter("@ReturnCode", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(returnCodeParam);

                    var returnStrParam = new SqlParameter("@ReturnStr", SqlDbType.VarChar, 8000)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(returnStrParam);

                    await command.ExecuteNonQueryAsync();
                    return (int)(returnCodeParam.Value ?? 0);
                }
            }
        }

        private void CleanupNullableFields(DeviceMasterRequest request)
        {
            // Ensure nullable int fields that should be excluded are set to null
            // This works in conjunction with ShouldSerialize methods to prevent DBNull casting errors
            // Fields with invalid values (< 0) are set to null so they won't be serialized
            if (request.CountryOfOriginId.HasValue && request.CountryOfOriginId.Value <= 0)
                request.CountryOfOriginId = null;
            if (request.MaterialDescriptionId.HasValue && request.MaterialDescriptionId.Value <= 0)
                request.MaterialDescriptionId = null;
            if (request.USHTSCodeId.HasValue && request.USHTSCodeId.Value <= 0)
                request.USHTSCodeId = null;
            if (request.ECCNId.HasValue && request.ECCNId.Value <= 0)
                request.ECCNId = null;
            if (request.LicenseExceptionId.HasValue && request.LicenseExceptionId.Value <= 0)
                request.LicenseExceptionId = null;
            // RestrictedCountriesToShipId is a string, not an int, so handle it differently
            if (!string.IsNullOrEmpty(request.RestrictedCountriesToShipId) && 
                (request.RestrictedCountriesToShipId == "-1" || request.RestrictedCountriesToShipId == "0"))
                request.RestrictedCountriesToShipId = null;
            if (request.MSLId.HasValue && request.MSLId.Value <= 0)
                request.MSLId = null;
            if (request.PeakPackageBodyTemperatureId.HasValue && request.PeakPackageBodyTemperatureId.Value <= 0)
                request.PeakPackageBodyTemperatureId = null;
            if (request.ShelfLifeMonthId.HasValue && request.ShelfLifeMonthId.Value <= 0)
                request.ShelfLifeMonthId = null;
            if (request.FloorLifeId.HasValue && request.FloorLifeId.Value <= 0)
                request.FloorLifeId = null;
            if (request.PBFreeId.HasValue && request.PBFreeId.Value <= 0)
                request.PBFreeId = null;
            if (request.PBFreeStickerId.HasValue && request.PBFreeStickerId.Value <= 0)
                request.PBFreeStickerId = null;
            if (request.ROHSId.HasValue && request.ROHSId.Value <= 0)
                request.ROHSId = null;
            if (request.TrayTubeStrappingId.HasValue && request.TrayTubeStrappingId.Value <= 0)
                request.TrayTubeStrappingId = null;
            if (request.TrayStackingId.HasValue && request.TrayStackingId.Value <= 0)
                request.TrayStackingId = null;
            // LockId: set to null if it's -1 (invalid/default value)
            if (request.LockId.HasValue && request.LockId.Value == -1)
                request.LockId = null;
        }

        private string SerializeToXml(DeviceMasterRequest request)
        {
            try
            {
                var serializer = new XmlSerializer(typeof(DeviceMasterRequest));
                using (var stringWriter = new StringWriter())
                {
                    serializer.Serialize(stringWriter, request);
                    return stringWriter.ToString();
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error serializing DeviceMasterRequest to XML: {ex.Message}", ex);
            }
        }

        public async Task<DataTable> SearchDeviceFamily(int? customerID, string? deviceFamilyName, bool? active)
        {
            var dataTable = new DataTable();
            using (var connection = new SqlConnection(GetConnectionString()))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("PRD_Device_P_SearchDeviceFamily", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    
                    command.Parameters.AddWithValue("@CustomerId", 
                        customerID.HasValue && customerID.Value != -1 ? (object)customerID.Value : DBNull.Value);
                    command.Parameters.AddWithValue("@DeviceFamily", 
                        !string.IsNullOrEmpty(deviceFamilyName) ? (object)deviceFamilyName : DBNull.Value);
                    command.Parameters.AddWithValue("@Active", 
                        active.HasValue ? (object)active.Value : DBNull.Value);

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
            using (var connection = new SqlConnection(GetConnectionString()))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("PRD_Device_P_Search", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    
                    command.Parameters.AddWithValue("@CustomerId", 
                        customerID.HasValue && customerID.Value != -1 ? (object)customerID.Value : DBNull.Value);
                    command.Parameters.AddWithValue("@DeviceFamilyId", 
                        deviceFamilyId.HasValue && deviceFamilyId.Value != -1 ? (object)deviceFamilyId.Value : DBNull.Value);
                    command.Parameters.AddWithValue("@DeviceId", DBNull.Value);
                    command.Parameters.AddWithValue("@Active", 
                        active.HasValue ? (object)active.Value : DBNull.Value);
                    command.Parameters.AddWithValue("@DeviceName", 
                        !string.IsNullOrEmpty(deviceName) ? (object)deviceName : DBNull.Value);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        dataTable.Load(reader);
                    }
                }
            }
            
            return dataTable;
        }

        public async Task<List<Country>> GetRestrictedCountries()
        {
            var countries = new List<Country>();
            using (var connection = new SqlConnection(GetConnectionString()))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("tfsp_GetRestrictedCountries", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            countries.Add(new Country
                            {
                                CountryID = reader.GetInt32(reader.GetOrdinal("CountryId")),
                                CountryName = reader.GetString(reader.GetOrdinal("Country"))
                            });
                        }
                    }
                }
            }
            return countries;
        }

        public async Task<List<CustomerLabel>> GetCustomerLabelList(string customerName)
        {
            var labels = new List<CustomerLabel>();
            using (var connection = new SqlConnection(GetConnectionString()))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("PRD_Label_P_GetCustomer_LabelList", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Customer", string.IsNullOrEmpty(customerName) ? DBNull.Value : customerName);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            labels.Add(new CustomerLabel
                            {
                                LID = reader.GetInt32(reader.GetOrdinal("LID")),
                                LName = reader.GetString(reader.GetOrdinal("LName"))
                            });
                        }
                    }
                }
            }
            return labels;
        }

        public async Task<LabelDetailsResponse> GetLabelDetails(int customerId, int deviceId, string labelName, string lotNum)
        {
            var response = new LabelDetailsResponse();
            var labelDetails = new List<LabelMappingDetail>();
            var ldValues = new List<Prd_LDValues>();
            
            using (var connection = new SqlConnection(GetConnectionString()))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("PRD_Label_P_GetLabelDetails", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CustomerId", customerId > 0 ? customerId : DBNull.Value);
                    command.Parameters.AddWithValue("@DeviceId", deviceId > 0 ? deviceId : DBNull.Value);
                    command.Parameters.AddWithValue("@LabelName", string.IsNullOrEmpty(labelName) ? DBNull.Value : labelName);
                    command.Parameters.AddWithValue("@LotNum", string.IsNullOrEmpty(lotNum) ? DBNull.Value : lotNum);
                    
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        int GetInt32Safe(string columnName, int defaultValue = 0)
                        {
                            try
                            {
                                int ordinal = reader.GetOrdinal(columnName);
                                return reader.IsDBNull(ordinal) ? defaultValue : reader.GetInt32(ordinal);
                            }
                            catch
                            {
                                return defaultValue;
                            }
                        }

                        string GetStringSafe(string columnName, string defaultValue = "")
                        {
                            try
                            {
                                int ordinal = reader.GetOrdinal(columnName);
                                return reader.IsDBNull(ordinal) ? defaultValue : reader.GetString(ordinal);
                            }
                            catch
                            {
                                return defaultValue;
                            }
                        }

                        bool GetBooleanSafe(string columnName, bool defaultValue = false)
                        {
                            try
                            {
                                int ordinal = reader.GetOrdinal(columnName);
                                return reader.IsDBNull(ordinal) ? defaultValue : reader.GetBoolean(ordinal);
                            }
                            catch
                            {
                                return defaultValue;
                            }
                        }

                        bool HasColumn(string columnName)
                        {
                            try
                            {
                                reader.GetOrdinal(columnName);
                                return true;
                            }
                            catch
                            {
                                return false;
                            }
                        }

                        if (HasColumn("ErrorNumber"))
                        {
                            throw new Exception("Stored procedure returned error result set");
                        }

                        while (await reader.ReadAsync())
                        {
                            labelDetails.Add(new LabelMappingDetail
                            {
                                LabelId = GetInt32Safe("LabelId", 0),
                                LFName = GetStringSafe("LFName", string.Empty),
                                LDType = GetStringSafe("LDType", string.Empty),
                                LDText = GetStringSafe("LDText", string.Empty),
                                LDValue = GetStringSafe("LDValue", string.Empty),
                                ImageVisible = GetStringSafe("ImageVisible", string.Empty),
                                IsEnable = GetBooleanSafe("IsEnable", true),
                                isEdit = GetBooleanSafe("isEdit", false),
                                cmbvisibility = GetStringSafe("cmbvisibility", "Visible"),
                                txtvisibility = GetStringSafe("txtvisibility", "Visible"),
                                LdTypecmbvisibility = GetStringSafe("LdTypecmbvisibility", "Visible"),
                                Imgcmbvisibility = GetStringSafe("Imgcmbvisibility", "Collapsed"),
                                LDTypeId = GetInt32Safe("LDTypeId", 0)
                            });
                        }

                        await reader.NextResultAsync();

                        if (await reader.NextResultAsync())
                        {
                            ldValues.Add(new Prd_LDValues { LDValueId = 0, LDValue = "--Select--" });
                            
                            while (await reader.ReadAsync())
                            {
                                try
                                {
                                    int masterListItemId = GetInt32Safe("MasterListItemId", 0);
                                    string itemText = GetStringSafe("ItemText", string.Empty);
                                    
                                    if (!string.IsNullOrEmpty(itemText))
                                    {
                                        ldValues.Add(new Prd_LDValues
                                        {
                                            LDValueId = masterListItemId,
                                            LDValue = itemText
                                        });
                                    }
                                }
                                catch (Exception ex)
                                {
                                    throw new InvalidOperationException($"Error reading LDValue row: {ex.Message}", ex);
                                }
                            }
                        }

                        await reader.NextResultAsync();
                    }
                }
            }
            
            response.LabelDetails = labelDetails;
            response.LDValues = ldValues;
            return response;
        }

        public async Task<int> AddOrUpdateLabelDetails(int customerId, int deviceId, string labelName, string input)
        {
            using (var connection = new SqlConnection(GetConnectionString()))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("PRD_Labels_P_Update_LABELDETAIL_X_DEVICE", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CustomerId", customerId);
                    command.Parameters.AddWithValue("@DeviceId", deviceId);
                    command.Parameters.AddWithValue("@LABEL", labelName);
                    command.Parameters.AddWithValue("@INPUT", input);
                    
                    var returnCodeParam = new SqlParameter("@RETURNCODE", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(returnCodeParam);
                    
                    await command.ExecuteNonQueryAsync();
                    return (int)(returnCodeParam.Value ?? 0);
                }
            }
        }

        public async Task<DataSet> GetDeviceInfo(int deviceId)
        {
            var dataSet = new DataSet();
            using (var connection = new SqlConnection(GetConnectionString()))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("PRD_Device_P_Get", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@DeviceId", deviceId > 0 ? deviceId : DBNull.Value);
                    
                    using (var adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dataSet);
                    }
                }
            }
            return dataSet;
        }
    }
}
