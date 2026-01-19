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
        private readonly string? _connectString = string.Empty;
        public SplitMergeRepository(IConfiguration configuration)
        {
            _connectString = configuration.GetConnectionString("InventoryConnection");
        }

        public async Task<List<MasterList>> GetMasterListItems(string listName, int? serviceId)
        {
            List<MasterList> result = new List<MasterList>();
            using (var connection = new SqlConnection(_connectString))
            {
                using (var command = new SqlCommand("PRD_P_GetMasterListItems", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@ListName", listName);
                    command.Parameters.AddWithValue("@ServiceId", serviceId);
                    //command.Parameters.AddWithValue("@ListName", "TravelerStatus");
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
            List<MasterList> result = new List<MasterList>();
            try
            {
                using (var connection = new SqlConnection(_connectString))
                {
                    using (var command = new SqlCommand("PRD_ScreenFilters_P_GetList", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@FilterName", "LotStatus");
                        command.Parameters.AddWithValue("@Screen", "Traveler");
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
            }
            catch (Exception ex)
            {

            }
            return result;
        }
        public async Task<List<TFSCustomer>> GetTFSCustomers()
        {
            List<TFSCustomer> result = new List<TFSCustomer>();
            try
            {
                using (var connection = new SqlConnection(_connectString))
                {
                    using (var command = new SqlCommand("tfsp_GetCustomers", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        await connection.OpenAsync();
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var record = new TFSCustomer();
                                record.CustomerID = reader.GetInt32("CustomerId");
                                record.CustomerName = reader.GetString("CustomerName");
                                record.Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString("Email");
                                record.IsCreditHold = reader.GetBoolean("IsCreditHold");
                                record.PaymentTerm = reader.IsDBNull(reader.GetOrdinal("PaymentTerm")) ? null : reader.GetString("PaymentTerm");
                                record.IsTBR = reader.GetBoolean("IsTBR");
                                record.ShowInphiFamilyID = reader.GetBoolean("ShowInphiFamilyID");
                                record.CanAddVerifyStep = reader.GetBoolean("CanAddVerifyStep");
                                result.Add(record);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }
        public async Task<List<TFSDeviceFamily>> GetDeviceFamilies(int customerId)
        {
            List<TFSDeviceFamily> result = new List<TFSDeviceFamily>();
            try
            {
                using (var connection = new SqlConnection(_connectString))
                {
                    using (var command = new SqlCommand("PRD_Device_Family_P_GetDeviceFamilies", connection))
                    {
                        command.Parameters.AddWithValue("@CustomerId", customerId);
                        command.CommandType = CommandType.StoredProcedure;
                        await connection.OpenAsync();
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var record = new TFSDeviceFamily();
                                record.DeviceFamilyId = reader.GetInt32("DeviceFamilyId");
                                record.DeviceFamilyName = reader.GetString("DeviceFamily");
                                record.CustomerDeviceFamily = reader.GetString("CustomerDeviceFamily");
                                record.CustomerID = reader.GetInt32("CustomerId");
                                record.CustomerName = reader.GetString("CustomerName");
                                record.Active = reader.GetBoolean("IsTBR");
                                result.Add(record);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }
        public async Task<List<TFSDevice>> GetDevices(int customerId, int? deviceFamiltyId, int? deviceId)
        {
            List<TFSDevice> result = new List<TFSDevice>();
            try
            {
                using (var connection = new SqlConnection(_connectString))
                {
                    using (var command = new SqlCommand("PRD_Device_P_GetDevices", connection))
                    {
                        command.Parameters.AddWithValue("@CustomerId", customerId);
                        command.Parameters.AddWithValue("@DeviceFamilyId", deviceFamiltyId);
                        command.Parameters.AddWithValue("@DeviceId", deviceId);
                        command.CommandType = CommandType.StoredProcedure;
                        await connection.OpenAsync();
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var record = new TFSDevice();
                                record.DeviceId = reader.GetInt32("DeviceId");
                                record.Device = reader.GetString("Device");
                                record.DeviceFamilyId = reader.IsDBNull(reader.GetOrdinal("DeviceFamilyId")) ? null : reader.GetInt32("DeviceFamilyId");
                                record.DeviceFamily = reader.IsDBNull(reader.GetOrdinal("DeviceFamily")) ? null : reader.GetString("DeviceFamily");
                                record.CustomerDevice = reader.IsDBNull(reader.GetOrdinal("CustomerDevice")) ? null : reader.GetString("CustomerDevice");
                                record.CustomerId = reader.IsDBNull(reader.GetOrdinal("CustomerId")) ? null : reader.GetInt32("CustomerId");
                                record.CustomerName = reader.GetString("CustomerName");
                                record.Active = reader.GetBoolean("Active");
                                result.Add(record);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }
        public async Task<List<TFSDeviceAlias>> GetDeviceAlias(int customerId, int deviceFamiltyId, int deviceId, string? source)
        {
            List<TFSDeviceAlias> result = new List<TFSDeviceAlias>();
            try
            {
                using (var connection = new SqlConnection(_connectString))
                {
                    using (var command = new SqlCommand("PRD_Device_Alias_P_Get", connection))
                    {
                        command.Parameters.AddWithValue("@CustomerId", customerId);
                        command.Parameters.AddWithValue("@DeviceFamilyId", deviceFamiltyId);
                        command.Parameters.AddWithValue("@DeviceId", deviceId);
                        command.Parameters.AddWithValue("@Source", source);
                        command.CommandType = CommandType.StoredProcedure;
                        await connection.OpenAsync();
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var record = new TFSDeviceAlias();
                                record.AliasId = reader.GetInt32("DeviceFamilyId");
                                record.AliasName = reader.GetString("DeviceFamily");
                                record.DeviceId = reader.IsDBNull(reader.GetOrdinal("DeviceId")) ? null : reader.GetInt32("DeviceId");
                                record.Device = reader.IsDBNull(reader.GetOrdinal("Device")) ? null : reader.GetString("Device");
                                record.DeviceFamilyId = reader.IsDBNull(reader.GetOrdinal("DeviceFamilyId")) ? null : reader.GetInt32("DeviceFamilyId");
                                result.Add(record);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public async Task<List<LotOwner>> GetLotOwners()
        {
            List<LotOwner> result = new List<LotOwner>();
            try
            {
                using (var connection = new SqlConnection(_connectString))
                {
                    using (var command = new SqlCommand("PRD_Lot_P_GetLotOwnersList", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        await connection.OpenAsync();
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var record = new LotOwner();
                                record.OwnerId = reader.GetInt32("OwnerId");
                                record.EmployeeId = reader.IsDBNull(reader.GetOrdinal("EmployeeId")) ? null : reader.GetInt32("EmployeeId");
                                record.EmployeeName = reader.IsDBNull(reader.GetOrdinal("EmployeeName")) ? null : reader.GetString("EmployeeName");
                                result.Add(record);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public async Task<List<LotSearch>> InventoryLotSearch(string? travellerStatusIds, string? lotStatusIds, DateTime? fromDate, DateTime? toDate)
        {
            List<LotSearch> result = new List<LotSearch>();
            using (var connection = new SqlConnection(_connectString))
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
            var lotDetail = new LotDetail();
            lotDetail.TRVSteps = new List<TRVStep>();

            using (var connection = new SqlConnection(_connectString))
            using (var command = new SqlCommand("INV_TRV_P_Get", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@LotId", lotId);
                command.Parameters.AddWithValue("@Source", source);

                await connection.OpenAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    // ? First result set: Single row
                    if (await reader.ReadAsync())
                    {
                        lotDetail.ISELotNumber = reader["ISELotNumber"] as string;
                        lotDetail.CustomerId = reader.IsDBNull("CustomerId") ? null : reader.GetInt32(reader.GetOrdinal("CustomerId"));
                        lotDetail.IsPackage = reader.IsDBNull("IsPackage") ? null : reader.GetBoolean(reader.GetOrdinal("IsPackage"));
                        lotDetail.DeviceFamilyId = reader["DeviceFamilyId"]?.ToString();
                        lotDetail.DeviceId = reader.IsDBNull("DeviceId") ? null : reader.GetInt32(reader.GetOrdinal("DeviceId"));
                        lotDetail.DeviceAliasId = reader.IsDBNull("DeviceAliasId") ? null : reader.GetInt32(reader.GetOrdinal("DeviceAliasId"));
                        lotDetail.OurCount = reader.IsDBNull("OurCount") ? null : reader.GetInt32(reader.GetOrdinal("OurCount"));
                        lotDetail.CurrentCount = reader.IsDBNull("CurrentCount") ? null : reader.GetInt32(reader.GetOrdinal("CurrentCount"));
                        lotDetail.RunningCount = reader.IsDBNull("RunningCount") ? 0 : reader.GetInt32(reader.GetOrdinal("RunningCount"));
                        lotDetail.ShipLocation = reader.IsDBNull("ShipLocation") ? null : reader.GetInt32(reader.GetOrdinal("ShipLocation"));
                        lotDetail.IQANotRequired = reader.IsDBNull("IQANotRequired") ? null : reader.GetBoolean(reader.GetOrdinal("IQANotRequired"));
                        lotDetail.DeviceTypeId = reader.IsDBNull("DeviceTypeId") ? null : reader.GetInt32(reader.GetOrdinal("DeviceTypeId"));
                        lotDetail.COOId = reader.IsDBNull("COOId") ? null : reader.GetInt32(reader.GetOrdinal("COOId"));
                        lotDetail.DateCode = reader["DateCode"] as string;
                        lotDetail.PartType = reader["PartType"] as string;
                        lotDetail.CustomerLotNumber = reader["CustomerLotNumber"] as string;
                        lotDetail.ServiceCategoryId = reader.IsDBNull("ServiceCategoryId") ? null : reader.GetInt32(reader.GetOrdinal("ServiceCategoryId"));
                        lotDetail.AssemblyId = reader.IsDBNull("AssemblyId") ? null : reader.GetInt32(reader.GetOrdinal("AssemblyId"));
                        lotDetail.ContactInfo = reader["ContactInfo"] as string;
                        lotDetail.Trays = reader["Trays"] as string;
                        lotDetail.LotMissingQty = reader.IsDBNull("LotMissingQty") ? null : reader.GetInt32(reader.GetOrdinal("LotMissingQty"));
                        lotDetail.UnitsOnReel = reader.IsDBNull("UnitsOnReel") ? null : reader.GetBoolean(reader.GetOrdinal("UnitsOnReel"));
                        lotDetail.ShipToScrap = reader.IsDBNull("ShipToScrap") ? null : reader.GetBoolean(reader.GetOrdinal("ShipToScrap"));
                        lotDetail.Expedite = reader.IsDBNull("Expedite") ? null : reader.GetBoolean(reader.GetOrdinal("Expedite"));
                        lotDetail.IsClosed = reader.IsDBNull("IsClosed") ? null : reader.GetBoolean(reader.GetOrdinal("IsClosed"));
                        lotDetail.BomId = reader.IsDBNull("BomId") ? null : reader.GetInt32(reader.GetOrdinal("BomId"));
                        lotDetail.ISEOwnerId = reader.IsDBNull("ISEOwnerId") ? null : reader.GetInt32(reader.GetOrdinal("ISEOwnerId"));
                        lotDetail.DataCategoryId = reader.IsDBNull("DataCategoryId") ? null : reader.GetInt32(reader.GetOrdinal("DataCategoryId"));
                        lotDetail.Identifier = reader["Identifier"] as string;
                    }

                    // ? Second result set: Multiple rows
                    if (await reader.NextResultAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var step = new TRVStep();// need to check this part

                            step.ID = reader.GetInt32(reader.GetOrdinal("ID"));
                            step.TRVStepId = reader.GetInt32(reader.GetOrdinal("TRVStepId"));
                            step.ParentTRVStepId = reader["ParentTRVStepId"] as int?;
                            step.LotId = reader.GetInt32(reader.GetOrdinal("LotId"));
                            step.LotNum = reader["LotNum"]?.ToString();
                            step.Description = reader["Description"]?.ToString();
                            step.StepName = reader["StepName"]?.ToString();
                            step.ServiceId = reader["ServiceId"] as int?;
                            step.ParentLotSplitStepServiceId = reader["ParentLotSplitStepServiceId"] as int?;
                            step.ParentSplitService = reader["ParentSplitService"]?.ToString();
                            step.ParentServiceId = reader["ParentServiceId"] as int?;
                            step.SubServiceId = reader["SubServiceId"] as int?;
                            step.SourceId = reader["SourceId"] as int?;
                            step.Source = reader["Source"]?.ToString();
                            step.SourceReferenceId = reader["SourceReferenceId"] as int?;
                            step.SetupProgramId = reader["SetupProgramId"] as int?;
                            step.SetupSiteType = reader["SetupSiteType"]?.ToString();
                            step.AlertComments = reader["AlertComments"]?.ToString();
                            step.StepType = reader["StepType"]?.ToString();
                            step.DeviationCode = reader["DeviationCode"]?.ToString();
                            step.DeviationComments = reader["DeviationComments"]?.ToString();

                            step.Sequence = reader.GetDecimal(reader.GetOrdinal("Sequence"));
                            step.SequenceGroup = reader.GetInt32(reader.GetOrdinal("SequenceGroup"));
                            step.StepSequence = reader.GetDecimal(reader.GetOrdinal("StepSequence"));
                            step.TotalQty = reader.GetInt32(reader.GetOrdinal("TotalQty"));
                            step.StepTotalQty = reader["StepTotalQty"] as int?;
                            step.ProcessedQty = reader["ProcessedQty"] as int?;
                            step.RemainingQty = reader["RemainingQty"] as int?;
                            step.MissingQty = reader["MissingQty"] as int?;
                            step.StepRemainingQty = reader["StepRemainingQty"] as int?;
                            step.GoodQty = reader["GoodQty"] as int?;
                            step.RejectQty = reader["RejectQty"] as int?;
                            step.QualifiedRejectQty = reader["QualifiedRejectQty"] as int?;
                            step.StartTime = reader.IsDBNull(reader.GetOrdinal("StartTime"))
     ? null
     : reader.GetString(reader.GetOrdinal("StartTime"));
                            step.EndTime = reader.IsDBNull(reader.GetOrdinal("EndTime"))
    ? null
    : reader.GetString(reader.GetOrdinal("EndTime"));
                            step.Status = reader["Status"]?.ToString();
                            step.OverAllYield = reader.IsDBNull(reader.GetOrdinal("OverAllYeild")) ? (decimal?)null : (decimal)reader.GetDouble(reader.GetOrdinal("OverAllYeild"));
                            step.ActualYeild = reader.IsDBNull(reader.GetOrdinal("ActualYeild")) ? (decimal?)null : (decimal)reader.GetDouble(reader.GetOrdinal("ActualYeild"));
                            step.BatchNo = reader["BatchNo"]?.ToString();
                            step.BatchInvoiceStatus = reader["BatchInvoiceStatus"]?.ToString();
                            step.IspartialInvoice = reader["IspartialInvoice"] as int?;
                            step.ShowFS = reader.IsDBNull(reader.GetOrdinal("ShowFS"))
    ? (int?)null
    : Convert.ToInt32(reader["ShowFS"]);

                            step.EditFS = reader.IsDBNull(reader.GetOrdinal("EditFS"))
                                ? (int?)null
                                : Convert.ToInt32(reader["EditFS"]);

                            step.ShowMerge = reader.IsDBNull(reader.GetOrdinal("ShowMerge"))
                                ? (int?)null
                                : Convert.ToInt32(reader["ShowMerge"]);

                            lotDetail.TRVSteps.Add(step);
                            step.SplitAvailable = reader.IsDBNull(reader.GetOrdinal("SplitAvailable"))
                                ? (int?)null
                                : Convert.ToInt32(reader["SplitAvailable"]);
                            step.EditSplit = reader.IsDBNull(reader.GetOrdinal("EditSplit"))
                                   ? (int?)null
                                   : Convert.ToInt32(reader["EditSplit"]);
                        }
                    }
                }
            }

            return lotDetail;
        }
        public async Task<List<MergeLot>> GetMergeLots(int lotId)
        {
            List<MergeLot> result = new List<MergeLot>();
            using (var connection = new SqlConnection(_connectString))
            {
                using (var command = new SqlCommand("PRD_TRV_P_GetMatchedMergeLots", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@LotId", lotId);
                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var record = new MergeLot();

                            record.LotId = reader.GetInt32(reader.GetOrdinal("LotId"));
                            record.DeviceId = reader.GetInt32(reader.GetOrdinal("DeviceId"));
                            record.IsPackage = reader.GetBoolean(reader.GetOrdinal("IsPackage"));
                            record.CustomerLotNumber = reader.GetString(reader.GetOrdinal("CustomerLotNumber"));
                            record.ISELotNumber = reader.GetString(reader.GetOrdinal("ISELotNumber"));
                            record.ISEOwnerId = reader.GetInt32(reader.GetOrdinal("ISEOwnerId"));
                            record.RunningCount = reader.GetInt32(reader.GetOrdinal("RunningCount"));
                            record.IsReceived = reader.GetBoolean(reader.GetOrdinal("IsReceived"));
                            record.TravelerStatusId = reader.GetInt32(reader.GetOrdinal("TravelerStatusId"));
                            record.TravelerStatus = reader.GetString(reader.GetOrdinal("TravelerStatus"));
                            record.StepCount = reader.GetInt32(reader.GetOrdinal("StepCount"));
                            record.Steps = reader.GetString(reader.GetOrdinal("Steps"));
                            result.Add(record);
                        }
                    }
                }
            }
            return result;

        }
        public async Task<List<IcrSearch>> IcrDashboardSearch(int? customerId, string? travellerStatusIds, string? lotStatusIds, DateTime? fromDate, DateTime? toDate, string? requestTypeIds)
        {
            List<IcrSearch> result = new List<IcrSearch>();
            using (var connection = new SqlConnection(_connectString))
            {
                using (var command = new SqlCommand("PRD_ICR_DashoardSearch", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@CustomerId", customerId);
                    command.Parameters.AddWithValue("@TravStatusIds", travellerStatusIds);
                    command.Parameters.AddWithValue("@LotStatusIds", lotStatusIds);
                    command.Parameters.AddWithValue("@FromDate", fromDate);
                    command.Parameters.AddWithValue("@ToDate", toDate);
                    command.Parameters.AddWithValue("@RequestTypeIds", requestTypeIds);
                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var record = new IcrSearch();
                            record.LotId = reader.IsDBNull(reader.GetOrdinal("LotId")) ? null : reader.GetInt32(reader.GetOrdinal("LotId"));
                            record.ControlRoomLocation = reader.IsDBNull(reader.GetOrdinal("ControlRoomLocation")) ? null : reader.GetString(reader.GetOrdinal("ControlRoomLocation"));
                            record.ISELotNumber = reader.IsDBNull(reader.GetOrdinal("ISELotNumber")) ? null : reader.GetString(reader.GetOrdinal("ISELotNumber"));
                            record.CustomerName = reader.IsDBNull(reader.GetOrdinal("CustomerName")) ? null : reader.GetString(reader.GetOrdinal("CustomerName"));
                            record.Requestor = reader.IsDBNull(reader.GetOrdinal("Requestor")) ? null : reader.GetString(reader.GetOrdinal("Requestor"));
                            record.RequestType = reader.IsDBNull(reader.GetOrdinal("RequestType")) ? null : reader.GetString(reader.GetOrdinal("RequestType"));
                            record.LotStatus = reader.IsDBNull(reader.GetOrdinal("LotStatus")) ? null : reader.GetString(reader.GetOrdinal("LotStatus"));
                            record.TravelerStatus = reader.IsDBNull(reader.GetOrdinal("TravelerStatus")) ? null : reader.GetString(reader.GetOrdinal("TravelerStatus"));
                            record.Status = reader.IsDBNull(reader.GetOrdinal("Status")) ? null : reader.GetString(reader.GetOrdinal("Status"));
                            record.BinBox = reader.IsDBNull(reader.GetOrdinal("BinBox")) ? null : reader.GetString(reader.GetOrdinal("BinBox"));
                            record.NoOfBinbox = reader["NoOfBinbox"] as int?;
                            record.CurrentLocation = reader.IsDBNull(reader.GetOrdinal("CurrentLocation")) ? null : reader.GetString(reader.GetOrdinal("CurrentLocation"));
                            record.CurrentLotLocation = reader.IsDBNull(reader.GetOrdinal("CurrentLotLocation")) ? null : reader.GetString(reader.GetOrdinal("CurrentLotLocation"));
                            record.PreviousStep = reader.IsDBNull(reader.GetOrdinal("PreviousStep")) ? null : reader.GetString(reader.GetOrdinal("PreviousStep"));
                            record.RequestedOn = reader.GetDateTime(reader.GetOrdinal("RequestedOn"));
                            record.TRVStepId = reader.IsDBNull(reader.GetOrdinal("TRVStepId")) ? null : reader.GetInt32(reader.GetOrdinal("TRVStepId"));
                            record.RequestId = reader.IsDBNull(reader.GetOrdinal("RequestId")) ? 0 : reader.GetInt32(reader.GetOrdinal("RequestId"));
                            result.Add(record);
                        }
                    }
                }
            }
            return result;

        }
        public async Task<List<MergedLots>> GetMatchedLots(int trvStepId)
        {
            List<MergedLots> result = new List<MergedLots>();
            using (var connection = new SqlConnection(_connectString))
            {
                using (var command = new SqlCommand("PRD_TRV_P_GetMergedLots", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@TRVStepId", trvStepId);
                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var record = new MergedLots();
                            record.MergeId = reader.GetInt32(reader.GetOrdinal("MergeId"));
                            record.TRVStepId = reader.GetInt32(reader.GetOrdinal("TRVStepId"));
                            record.LotId = reader.GetInt32(reader.GetOrdinal("LotId"));
                            record.MergedLotIds = reader.IsDBNull(reader.GetOrdinal("MergedLotIds")) ? null : reader.GetString(reader.GetOrdinal("MergedLotIds"));
                            record.MergedLotNumbers = reader.IsDBNull(reader.GetOrdinal("MergedLotNumbers")) ? null : reader.GetString(reader.GetOrdinal("MergedLotNumbers"));
                            record.IsCompleted = reader.GetBoolean(reader.GetOrdinal("IsCompleted"));
                            record.Active = reader.GetBoolean(reader.GetOrdinal("Active"));

                            result.Add(record);
                        }
                    }
                }
            }
            return result;

        }
        public async Task<bool> SaveMergeLots(LotMerge request)
        {
            using (var connection = new SqlConnection(_connectString))
            using (var command = new SqlCommand("PRD_TRV_P_Merge", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                // Add parameters
                command.Parameters.AddWithValue("@LotId", request.LotId);
                command.Parameters.AddWithValue("@MergeLotIdStr", request.MergeLotIdStr);
                command.Parameters.AddWithValue("@UserId", request.UserId);
                command.Parameters.AddWithValue("@IsConsolidateSplit", request.IsConsolidateSplit);
                var outParam = new SqlParameter("@ReturnCode", SqlDbType.Int);
                outParam.Direction = ParameterDirection.Output;
                command.Parameters.Add(outParam);

                await connection.OpenAsync();

                try
                {
                    await command.ExecuteNonQueryAsync();
                    var result = Convert.ToInt32(outParam.Value);
                    return result > 0;
                }
                catch (SqlException ex)
                {
                    return false;
                }
            }
        }
        public async Task<int> AddOrUpdateMergeAsync(int? mergeId, int trvStepId, string? lotIds, int userId)
        {
            int returnCode = -1;

            using (var connection = new SqlConnection(_connectString))
            using (var command = new SqlCommand("PRD_TRV_P_AddorUpdateMerge", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@MergeId", (object?)mergeId ?? DBNull.Value);
                command.Parameters.AddWithValue("@TRVStepId", trvStepId);
                command.Parameters.AddWithValue("@LotIds", lotIds ?? "");
                command.Parameters.AddWithValue("@UserId", userId);

                var outputParam = new SqlParameter("@ReturnCode", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(outputParam);

                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();

                returnCode = (int)(outputParam.Value ?? -1);
            }

            return returnCode;
        }
        public async Task<List<FutureSplitBin>> GetFutureSplitBinsAsync(int trvStepId)
        {
            var result = new List<FutureSplitBin>();

            using (var connection = new SqlConnection(_connectString))
            using (var command = new SqlCommand("PRD_TRV_Step_P_GetFutureSplitBins", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TRVStepId", trvStepId);

                await connection.OpenAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var bin = new FutureSplitBin
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            BinId = reader.IsDBNull(reader.GetOrdinal("BinId")) ? null : reader.GetString(reader.GetOrdinal("BinId")),
                            BinCode = reader.IsDBNull(reader.GetOrdinal("BinCode")) ? null : reader.GetString(reader.GetOrdinal("BinCode")),
                            Condition = reader.IsDBNull(reader.GetOrdinal("Condition")) ? null : reader.GetString(reader.GetOrdinal("Condition")),
                            Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description"))
                        };

                        result.Add(bin);
                    }
                }
            }

            return result;
        }
        public async Task<List<SplitBin>> GetSplitBinsAsync(int lotId, int trvStepId, bool rejectBins)
        {
            var result = new List<SplitBin>();

            using (var connection = new SqlConnection(_connectString))
            using (var command = new SqlCommand("PRD_TRV_Step_P_GetSplitBins", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@LotId", lotId);
                command.Parameters.AddWithValue("@TRVStepId", trvStepId);
                command.Parameters.AddWithValue("@RejectBins", rejectBins);

                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var bin = new SplitBin
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            BinId = reader.IsDBNull(reader.GetOrdinal("BinId")) ? null : reader.GetString(reader.GetOrdinal("BinId")),
                            Qty = reader.IsDBNull(reader.GetOrdinal("Qty")) ? null : reader.GetString(reader.GetOrdinal("Qty")),
                            BinCode = reader.IsDBNull(reader.GetOrdinal("BinCode")) ? null : reader.GetString(reader.GetOrdinal("BinCode")),
                            Condition = reader.IsDBNull(reader.GetOrdinal("Condition")) ? null : reader.GetString(reader.GetOrdinal("Condition")),
                            Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                            HasOtherRejects = reader.IsDBNull(reader.GetOrdinal("HasOtherRejects")) ? false : reader.GetBoolean(reader.GetOrdinal("HasOtherRejects")),
                            SplitTotalRejects = reader.IsDBNull(reader.GetOrdinal("SplitTotalRejects")) ? false : reader.GetBoolean(reader.GetOrdinal("SplitTotalRejects"))
                        };
                        if (bin.HasOtherRejects)
                        {
                            if (bin.BinCode == "1")
                                bin.DisplayBinCode = "VB-1";
                            if (bin.BinCode == "2")
                                bin.DisplayBinCode = "VB-2";
                            if (bin.BinCode == "3")
                                bin.DisplayBinCode = "VB-3";
                            if (bin.BinCode == "4")
                                bin.DisplayBinCode = "VB-4";
                            if (bin.BinCode == "5")
                                bin.DisplayBinCode = "VB-5";
                        }

                        result.Add(bin);
                    }
                }
            }

            return result;
        }
        public async Task<List<FutureSplit>> GetFutureSplitsAsync(int trvStepId)
        {
            var result = new List<FutureSplit>();

            using (var connection = new SqlConnection(_connectString))
            using (var command = new SqlCommand("PRD_TRV_Steps_P_GetFS", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TRVStepId", trvStepId);

                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        try
                        {
                            var splitXML = reader.IsDBNull(reader.GetOrdinal("SplitXML")) ? null : reader.GetString(reader.GetOrdinal("SplitXML"));
                            List<TotalSplitDto> totalSplits = new List<TotalSplitDto>();
                            if (!splitXML.IsNullOrEmpty())
                            {
                                var serializer = new XmlSerializer(typeof(SplitStepDto));

                                using var xmlReader = new StringReader(splitXML);
                                var dto = serializer.Deserialize(xmlReader) as SplitStepDto;
                                if (dto != null && dto.LstSplits != null)
                                {
                                    totalSplits = dto.LstSplits.TotalSplits;
                                }
                            }
                            var fs = new FutureSplit
                            {
                                FSId = reader.GetInt32(reader.GetOrdinal("FSId")),
                                TRVStepId = reader.GetInt32(reader.GetOrdinal("TRVStepId")),
                                SplitNo = reader.GetInt32(reader.GetOrdinal("SplitNo")),
                                TotalSplits = totalSplits,
                                Active = reader.IsDBNull(reader.GetOrdinal("Active")) ? false : reader.GetBoolean(reader.GetOrdinal("Active")),
                                Source = reader.IsDBNull(reader.GetOrdinal("Source")) ? null : reader.GetString(reader.GetOrdinal("Source")),
                                LotId = reader.GetInt32(reader.GetOrdinal("LotId")),
                                SourceId = reader.IsDBNull(reader.GetOrdinal("SourceId")) ? null : reader.GetInt32(reader.GetOrdinal("SourceId"))
                            };

                            result.Add(fs);
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
            }

            return result;
        }

        public async Task<List<TotalSplitDto>> GetSplitsAsync(int trvStepId)
        {
            var result = new List<TotalSplitDto>();

            using (var connection = new SqlConnection(_connectString))
            using (var command = new SqlCommand("PRD_TRV_Steps_P_GetSplit", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TRVStepId", trvStepId);

                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var splitXML = reader.IsDBNull(reader.GetOrdinal("SplitXML")) ? null : reader.GetString(reader.GetOrdinal("SplitXML"));
                        if (!splitXML.IsNullOrEmpty())
                        {
                            var serializer = new XmlSerializer(typeof(SplitStepDto));

                            using var xmlReader = new StringReader(splitXML);
                            var dto = serializer.Deserialize(xmlReader) as SplitStepDto;
                            if (dto != null && dto.LstSplits != null)
                            {
                                return dto.LstSplits.TotalSplits;
                            }
                        }
                    }
                }
            }

            return result;
        }

        public async Task<bool> AddOrUpdateSplit(SplitRequest splitRequest)
        {
            int returnCode = -1;

            if (splitRequest != null)
            {
                string splitXML = "";
                if (splitRequest.AddOrUpdateSplits != null && splitRequest.AddOrUpdateSplits.Count > 0)
                {
                    LstSplitsDto lstSplitsDto = new();
                    lstSplitsDto.TotalSplits = new();
                    foreach (var request in splitRequest.AddOrUpdateSplits)
                    {
                        List<SplitBinDto> splitBins = new();
                        foreach (var field in request.ExtraFields)
                        {
                            if (field.Value.ValueKind == System.Text.Json.JsonValueKind.Object)
                            {
                                try
                                {
                                    var options = new JsonSerializerOptions
                                    {
                                        PropertyNameCaseInsensitive = true
                                    };

                                    var raw = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(field.Value, options)!;
                                    if (raw != null)
                                    {
                                        var result = new Dictionary<string, Dictionary<string, SplitBinDto>>();

                                        foreach (var item in raw)
                                        {
                                            if (item.Value.ValueKind == JsonValueKind.Object)
                                            {
                                                var inner = System.Text.Json.JsonSerializer.Deserialize<SplitBinDto>(item.Value.GetRawText(), options);

                                                if (inner != null)
                                                {
                                                    splitBins.Add(new SplitBinDto()
                                                    {
                                                        BinId = inner.BinId,
                                                        Condition = inner.Condition ?? "",
                                                        SplitQty = inner.SplitQty
                                                    });
                                                }
                                            }
                                        }
                                    }

                                }
                                catch (Exception ex)
                                {

                                }
                            }
                        }
                        TotalSplitDto totalSplit = new()
                        {
                            Bins = new BinsDto() { SplitBins = splitBins },
                            IsCopySteps = request.CopySteps,
                            IsCopyShippingInfo = request.CopyShipping,
                            IsDeleted = false,
                            SplitNo = 0,
                            SplitId = request.Id
                        };

                        lstSplitsDto.TotalSplits.Add(totalSplit);

                    }
                    SplitStepDto splitStepDto = new()
                    {
                        TrvStepId = splitRequest.TrvStepId,
                        LstSplits = lstSplitsDto,

                    };
                    var serializer = new XmlSerializer(typeof(SplitStepDto));
                    var settings = new System.Xml.XmlWriterSettings
                    {
                        Encoding = Encoding.UTF8,
                        Indent = true,
                        OmitXmlDeclaration = false
                    };

                    using var sw = new StringWriter();
                    using var writer = System.Xml.XmlWriter.Create(sw, settings);
                    var ns = new XmlSerializerNamespaces();
                    ns.Add(string.Empty, string.Empty);
                    serializer.Serialize(writer, splitStepDto, ns);
                    splitXML = sw.ToString();

                    using (var connection = new SqlConnection(_connectString))
                    using (var command = new SqlCommand("PRD_TRV_Step_P_AddOrUpdateSplit", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@SplitXML", splitXML);
                        command.Parameters.AddWithValue("@UserId", splitRequest.UserId);
                        var outputParam1 = new SqlParameter("@ReturnStr", SqlDbType.VarChar)
                        {
                            Direction = ParameterDirection.Output,
                            Size = 1000
                        };
                        var outputParam2 = new SqlParameter("@ReturnCode", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(outputParam1);
                        command.Parameters.Add(outputParam2);

                        await connection.OpenAsync();
                        await command.ExecuteNonQueryAsync();

                        returnCode = (int)(outputParam2.Value ?? -1);
                    }
                }
                else
                {
                    return await DeleteRequestSplit(splitRequest.TrvStepId, splitRequest.UserId, DateTime.Now);
                }
            }
            return returnCode > 0 ? true : false;
        }

        private async Task<bool> DeleteRequestSplit(int TRVStepId, int UserId, DateTime Date)
        {
            try
            {
                int returnCode = -1;
                using (var connection = new SqlConnection(_connectString))
                using (var command = new SqlCommand("PRD_TRV_Steps_P_DeleteSplit", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@TRVStepId", TRVStepId);
                    command.Parameters.AddWithValue("@UserId", UserId);
                    var outputParam = new SqlParameter("@ReturnCode", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(outputParam);
                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                    returnCode = (int)(outputParam.Value ?? -1);
                    return returnCode > 0 ? true : false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> AddOrUpdateFutureSplit(SplitRequest splitRequest)
        {
            int returnCode = -1;

            if (splitRequest != null)
            {
                string splitXML = "";

                if (splitRequest.AddOrUpdateSplits != null && splitRequest.AddOrUpdateSplits.Count > 0)
                {
                    var fsIdsList = splitRequest.AddOrUpdateSplits.Where(s => s.IsDeleted).Select(s => s.FSId).ToList();
                    if (fsIdsList.Any())
                    {
                        var fsIds = string.Join(",", fsIdsList);
                        return await DeleteFutureSplits(fsIds, splitRequest.UserId);
                    }
                    var addUpdatedList = splitRequest.AddOrUpdateSplits.Where(s => !s.IsDeleted).ToList();
                    if (addUpdatedList.Any())
                    {
                        foreach (var request in addUpdatedList)
                        {
                            LstSplitsDto lstSplitsDto = new();
                            lstSplitsDto.TotalSplits = new();
                            List<SplitBinDto> splitBins = new();
                            foreach (var field in request.ExtraFields)
                            {
                                if (field.Value.ValueKind == System.Text.Json.JsonValueKind.Object)
                                {
                                    try
                                    {
                                        var options = new JsonSerializerOptions
                                        {
                                            PropertyNameCaseInsensitive = true
                                        };

                                        var raw = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(field.Value, options)!;
                                        if (raw != null)
                                        {
                                            var result = new Dictionary<string, Dictionary<string, SplitBinDto>>();

                                            foreach (var item in raw)
                                            {
                                                if (item.Value.ValueKind == JsonValueKind.Object)
                                                {
                                                    var inner = System.Text.Json.JsonSerializer.Deserialize<SplitBinDto>(item.Value.GetRawText(), options);

                                                    if (inner != null)
                                                    {
                                                        splitBins.Add(new SplitBinDto()
                                                        {
                                                            FSId = request.FSId,
                                                            BinId = inner.BinId,
                                                            Condition = inner.Condition ?? "",
                                                            SplitQty = inner.SplitQty
                                                        });
                                                    }
                                                }
                                            }
                                        }

                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                }
                            }
                            TotalSplitDto totalSplit = new()
                            {
                                Bins = new BinsDto() { SplitBins = splitBins },
                                IsCopySteps = request.CopySteps,
                                IsCopyShippingInfo = request.CopyShipping,
                                IsDeleted = request.IsDeleted,
                                SplitNo = 0,
                                SplitId = request.Id
                            };

                            lstSplitsDto.TotalSplits.Add(totalSplit);


                            SplitStepDto splitStepDto = new()
                            {
                                FSId = request.FSId,
                                TrvStepId = splitRequest.TrvStepId,
                                LstSplits = lstSplitsDto,

                            };
                            var serializer = new XmlSerializer(typeof(SplitStepDto));
                            var settings = new System.Xml.XmlWriterSettings
                            {
                                Encoding = Encoding.UTF8,
                                Indent = true,
                                OmitXmlDeclaration = false
                            };

                            using var sw = new StringWriter();
                            using var writer = System.Xml.XmlWriter.Create(sw, settings);
                            var ns = new XmlSerializerNamespaces();
                            ns.Add(string.Empty, string.Empty);
                            serializer.Serialize(writer, splitStepDto, ns);
                            splitXML = sw.ToString();

                            using (var connection = new SqlConnection(_connectString))
                            using (var command = new SqlCommand("PRD_TRV_Steps_P_AddOrUpdateFS", connection))
                            {
                                command.CommandType = CommandType.StoredProcedure;
                                command.Parameters.AddWithValue("@SplitXML", splitXML);
                                command.Parameters.AddWithValue("@StepsXML", "");
                                command.Parameters.AddWithValue("@UserId", splitRequest.UserId);
                                command.Parameters.AddWithValue("@FSId", request.FSId);
                                var outputParam1 = new SqlParameter("@ReturnStr", SqlDbType.VarChar)
                                {
                                    Direction = ParameterDirection.Output,
                                    Size = 1000
                                };
                                var outputParam2 = new SqlParameter("@ReturnCode", SqlDbType.Int)
                                {
                                    Direction = ParameterDirection.Output
                                };
                                command.Parameters.Add(outputParam1);
                                command.Parameters.Add(outputParam2);

                                await connection.OpenAsync();
                                await command.ExecuteNonQueryAsync();

                                returnCode = (int)(outputParam2.Value ?? -1);
                            }
                        }
                    }
                }
            }
            return returnCode > 0 ? true : false;
        }

        private async Task<bool> DeleteFutureSplits(string FSIds, int userId)
        {
            try
            {
                int returnCode = -1;
                using (var connection = new SqlConnection(_connectString))
                using (var command = new SqlCommand("PRD_TRV_Steps_P_DeleteFS", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@FSIds", FSIds);
                    command.Parameters.AddWithValue("@UserId", userId);
                    await connection.OpenAsync();
                    using (var adapter = new SqlDataAdapter(command))
                    {
                        var ds = new DataSet();
                        adapter.Fill(ds);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            if (ds.Tables.Count > 0 &&
                                ds.Tables[0].Rows.Count > 0 &&
                                ds.Tables[0].Rows[0].Field<int?>("ReturnCode") > 0)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            catch
            {
                return false;
            }

            return false;
        }

        public async Task<SplitPreviewBOOutPut> GetPreviewDetails(int trvStepId, int lotId)
        {
            SplitPreviewBOOutPut splitPreviewBOOut = new SplitPreviewBOOutPut();
            using (var connection = new SqlConnection(_connectString))
            using (var command = new SqlCommand("PRD_TRV_Steps_P_GetSplit", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TRVStepId", trvStepId);

                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var splitXML = reader.IsDBNull(reader.GetOrdinal("SplitXML")) ? null : reader.GetString(reader.GetOrdinal("SplitXML"));
                        if (!splitXML.IsNullOrEmpty())
                        {
                            using (var connection2 = new SqlConnection(_connectString))
                            using (var command2 = new SqlCommand("PRD_TRV_P_Split_Preview", connection2))
                            {
                                command2.CommandType = CommandType.StoredProcedure;
                                command2.Parameters.AddWithValue("@TRVStepId", trvStepId);
                                command2.Parameters.AddWithValue("@SplitXML", splitXML);
                                command2.Parameters.AddWithValue("@LotId", lotId);
                                await connection2.OpenAsync();
                                using (var adapter = new SqlDataAdapter(command2))
                                {
                                    var ds = new DataSet();
                                    adapter.Fill(ds);
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        splitPreviewBOOut = await GetPreviewDetails(ds);
                                    }
                                }

                            }

                        }
                    }
                }
            }
            return splitPreviewBOOut;
        }

        public async Task<SplitPreviewBOOutPut> GetFSPreviewDetails(int trvStepId)
        {
            try
            {
                using (var connection = new SqlConnection(_connectString))
                using (var command = new SqlCommand("PRD_TRV_Steps_P_PreviewFS", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@TRVStepId", trvStepId);
                    command.Parameters.AddWithValue("@VoidFSIds", string.Empty);
                    await connection.OpenAsync();
                    using (var adapter = new SqlDataAdapter(command))
                    {
                        var ds = new DataSet();
                        adapter.Fill(ds);
                        return await GetPreviewDetails(ds);
                    }
                }
            }
            catch
            {
                return new SplitPreviewBOOutPut();
            }

        }
        public async Task<string> GenerateFutureSplit(int trvStepId)
        {
            try
            {
                int returnCode = -1;
                using (var connection = new SqlConnection(_connectString))
                using (var command = new SqlCommand("PRD_TRV_Steps_P_GenerateFS", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@TRVStepId", trvStepId);
                    var outputParam = new SqlParameter("@ReturnCode", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    var outputParam2 = new SqlParameter("@ReturnStr", SqlDbType.VarChar)
                    {
                        Direction = ParameterDirection.Output,
                        Size = 1000
                    };
                    command.Parameters.Add(outputParam);
                    command.Parameters.Add(outputParam2);
                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                    returnCode = (int)(outputParam.Value ?? -1);
                    return Convert.ToString(outputParam2.Value) ?? "Error occurred while FS generation.";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<string> GenerateSplit(int trvStepId, int userId)
        {
            using (var connection = new SqlConnection(_connectString))
            using (var command = new SqlCommand("PRD_TRV_Steps_P_GetSplit", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TRVStepId", trvStepId);

                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var splitXML = reader.IsDBNull(reader.GetOrdinal("SplitXML")) ? null : reader.GetString(reader.GetOrdinal("SplitXML"));
                        if (!splitXML.IsNullOrEmpty())
                        {
                            using (var connection2 = new SqlConnection(_connectString))
                            using (var command2 = new SqlCommand("PRD_TRV_Steps_P_GenerateSplit", connection2))
                            {
                                command2.CommandType = CommandType.StoredProcedure;
                                command2.Parameters.AddWithValue("@TRVStepId", trvStepId);
                                command2.Parameters.AddWithValue("@SplitXML", splitXML);
                                command2.Parameters.AddWithValue("@UserId", userId);
                                await connection2.OpenAsync();
                                using (var adapter = new SqlDataAdapter(command2))
                                {
                                    var ds = new DataSet();
                                    adapter.Fill(ds);
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        if (ds.Tables.Count > 0 &&
                                            ds.Tables[0].Rows.Count > 0)
                                        {
                                            if (ds.Tables[0].Columns.Contains("ReturnCode"))
                                            {
                                                if (ds.Tables[0].Columns.Contains("ReturnStr"))
                                                    return Convert.ToString(ds.Tables[0].Rows[0]["ReturnStr"]) ?? string.Empty;
                                                else return "Error in DB,please try later.";
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return "Error in DB,please try later.";
        }

        private async Task<SplitPreviewBOOutPut> GetPreviewDetails(DataSet ds)
        {
            string returnMessage = string.Empty;
            try
            {
                SplitPreviewBO objSplitPreviewBO = new SplitPreviewBO();
                DataTable dt1 = new DataTable();
                DataTable dt2 = new DataTable();
                if (ds.Tables.Count == 2)
                {
                    dt1 = ds.Tables[0];
                    dt2 = ds.Tables[1];
                }
                else if (ds.Tables.Count == 3)
                {
                    dt1 = ds.Tables[1];
                    dt2 = ds.Tables[2];
                }
                else if (ds.Tables.Count == 1)
                {
                    if (ds.Tables[0].Columns.Contains("ReturnCode"))
                    {
                        if (ds.Tables[0].Columns.Contains("ReturnStr"))
                            returnMessage = Convert.ToString(ds.Tables[0].Rows[0]["ReturnStr"]) ?? string.Empty;
                        else returnMessage = "Error in DB,please try later.";

                        return await Task.FromResult(new SplitPreviewBOOutPut() { ReturnMessage = returnMessage, SplitPreviewBO = objSplitPreviewBO });
                    }
                    else
                    {
                        returnMessage = "Error in DB,please try later.";
                        return await Task.FromResult(new SplitPreviewBOOutPut() { ReturnMessage = returnMessage, SplitPreviewBO = objSplitPreviewBO });
                    }

                }
                if (ds.Tables[0].Columns.Contains("ReturnCode") && Convert.ToInt32(ds.Tables[0].Rows[0]["ReturnCode"]) == -1)
                {
                    if (ds.Tables[0].Columns.Contains("ReturnStr"))
                        returnMessage = Convert.ToString(ds.Tables[0].Rows[0]["ReturnStr"]) ?? string.Empty;
                    else returnMessage = "Error in DB,please try later.";
                    return await Task.FromResult(new SplitPreviewBOOutPut() { ReturnMessage = returnMessage, SplitPreviewBO = objSplitPreviewBO });
                }
                else
                {

                    string pLotNo = string.Empty;
                    objSplitPreviewBO.PreviewHeader = new List<SplitPreviewHeaderBO>();
                    foreach (DataRow dr in dt1.Rows)
                    {
                        SplitPreviewHeaderBO objSplitPreviewHeaderBO = new SplitPreviewHeaderBO();
                        objSplitPreviewHeaderBO.PreviewSteps = new List<SplitPreviewStepsBO>();
                        pLotNo = objSplitPreviewHeaderBO.LotNumber = Convert.ToString(dr["ISELotNumber"]) ?? string.Empty;
                        objSplitPreviewHeaderBO.RunningQty = Convert.ToString(dr["RunningCount"]);
                        objSplitPreviewHeaderBO.CurrentQty = Convert.ToString(dr["CurrentCount"]);
                        objSplitPreviewHeaderBO.TotalQty = Convert.ToString(dr["OurCount"]);
                        objSplitPreviewHeaderBO.Customer = Convert.ToString(dr["CustomerName"]);
                        objSplitPreviewHeaderBO.DeviceFamily = Convert.ToString(dr["DeviceFamily"]);
                        objSplitPreviewHeaderBO.Device = Convert.ToString(dr["Device"]);
                        objSplitPreviewHeaderBO.Category = Convert.ToString(dr["Category"]);

                        objSplitPreviewBO.PreviewHeader.Add(objSplitPreviewHeaderBO);

                        foreach (DataRow drow in dt2.Rows)
                        {
                            SplitPreviewStepsBO mfEachStepBO = new SplitPreviewStepsBO();
                            mfEachStepBO.LotNumber = Convert.ToString(drow["ISELotNumber"]);
                            if (pLotNo == mfEachStepBO.LotNumber)
                            {
                                mfEachStepBO.Source = Convert.ToString(drow["Source"]);
                                mfEachStepBO.Sequence = Convert.ToDecimal(drow["Sequence"]);
                                mfEachStepBO.Description = Convert.ToString(drow["Description"]);
                                mfEachStepBO.Status = Convert.ToString(drow["Status"]);
                                mfEachStepBO.QtyIn = Convert.ToString(drow["TotalQty"]);
                                mfEachStepBO.QtyOut = Convert.ToString(Convert.ToString(drow["GoodQty"]) + Convert.ToString(drow["QualifiedRejectQty"]));
                                mfEachStepBO.QualifiedRejectQty = Convert.ToString(drow["QualifiedRejectQty"]);
                                mfEachStepBO.TimeIn = drow["StartTime"].ToString();
                                mfEachStepBO.TimeOut = drow["EndTime"].ToString();
                                mfEachStepBO.RejectQty = Convert.ToString(drow["RejectQty"]);
                                mfEachStepBO.ProcessedQty = Convert.ToString(drow["ProcessedQty"]);
                                mfEachStepBO.Stepname = Convert.ToString(drow["StepName"]);
                                mfEachStepBO.RemainingQty = Convert.ToString(drow["RemainingQty"]);
                                mfEachStepBO.GoodQty = Convert.ToString(drow["GoodQty"]);
                                objSplitPreviewHeaderBO.PreviewSteps.Add(mfEachStepBO);
                            }
                        }
                    }
                    returnMessage = "";
                    return await Task.FromResult(new SplitPreviewBOOutPut() { ReturnMessage = returnMessage, SplitPreviewBO = objSplitPreviewBO }); ;
                }
            }

            catch (Exception ex) { throw ex; }
        }
    }
}
