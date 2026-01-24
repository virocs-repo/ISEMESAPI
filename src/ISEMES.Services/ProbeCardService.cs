using ISEMES.Models;
using ISEMES.Repositories;
using Microsoft.Extensions.Logging;
using System.Data;

namespace ISEMES.Services
{
    public class ProbeCardService : IProbeCardService
    {
        private readonly IProbeCardRepository _repository;
        private readonly ILogger<ProbeCardService> _logger;

        public ProbeCardService(IProbeCardRepository repository, ILogger<ProbeCardService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<List<ProbeCardResponse>> SearchProbeCard(ProbeCardSearchRequest request)
        {
            var dataTable = await _repository.SearchProbeCard(request);
            var result = new List<ProbeCardResponse>();

            if (dataTable != null && dataTable.Rows != null)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    result.Add(new ProbeCardResponse
                    {
                        MasterId = SafeGetInt32(row, "MasterId"),
                        ProbeCardId = SafeGetInt32(row, "ProbeCardId") != 0 ? SafeGetInt32(row, "ProbeCardId") : SafeGetInt32(row, "MasterId"),
                        ISEId = SafeGetString(row, "ISEID") ?? SafeGetString(row, "ISEId"),
                        CustomerName = SafeGetString(row, "CustomerName"),
                        CustomerId = SafeGetNullableInt(row, "CustomerId"),
                        CustomerHWId = SafeGetString(row, "Customer_NO") ?? SafeGetString(row, "CustomerHWId"),
                        HardwareType = SafeGetString(row, "HardwareType"),
                        Location = SafeGetString(row, "Location"),
                        Status = SafeGetString(row, "ProbeCardStatus") ?? SafeGetString(row, "Status"),
                        IsActive = SafeGetNullableBoolean(row, "Active"),
                        PlatformName = SafeGetString(row, "PlatformName"),
                        ProbeCardType = SafeGetString(row, "ProbeCardType") ?? SafeGetString(row, "ProbType"),
                        EquipmentName = SafeGetString(row, "EquipmentName"),
                        BoardType = SafeGetString(row, "BoardType"),
                        CreatedOn = SafeGetString(row, "CreatedOn") ?? SafeGetString(row, "CreatedDate"),
                        ModifiedOn = SafeGetString(row, "ModifiedOn"),
                        SecondaryCustomerName = SafeGetString(row, "SecondaryCustomer") ?? SafeGetString(row, "SecondaryCustomerName") ?? SafeGetString(row, "SecondaryCustomerId"),
                        DeviceFamily = SafeGetString(row, "DeviceFamily") ?? SafeGetString(row, "DeviceFamilyName"),
                        Device = SafeGetString(row, "Device") ?? SafeGetString(row, "DeviceName"),
                        DeviceAlias = SafeGetString(row, "DeviceAlias") ?? SafeGetString(row, "DeviceAliasName")
                    });
                }
            }

            return result;
        }

        public async Task<ProbeCardDetailsResponse> GetProbeCardDetails(int masterId)
        {
            var dataSet = await _repository.GetProbeCardDetails(masterId);
            var response = new ProbeCardDetailsResponse();

            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                // Table 0: Main ProbeCard details
                if (dataSet.Tables[0].Rows.Count > 0)
                {
                    var row = dataSet.Tables[0].Rows[0];
                    response.MasterId = SafeGetInt32(row, "MasterId");
                    response.ProbeCardId = SafeGetInt32(row, "ProbeCardId");
                    response.ISEId = SafeGetString(row, "PCISEId") ?? SafeGetString(row, "ISEId");
                    response.GenISEID = SafeGetString(row, "GenISEID");
                    response.IsITAR = SafeGetBoolean(row, "IsITAR");
                    response.ProbeCardTypeId = SafeGetInt32(row, "ProbTypeId");
                    response.ProbeCardType_Others = SafeGetString(row, "Others");
                    response.CustomerId = SafeGetNullableInt(row, "CustomerId");
                    response.CustomerHardwareId = SafeGetString(row, "Customer_NO");
                    response.HardwareLocation = SafeGetInt32(row, "LocationId");
                    response.ProbeCardDimension = SafeGetString(row, "ProbDimension");
                    response.PogoTowerId = SafeGetNullableInt(row, "PogoTowerId");
                    response.IsPowerCard = SafeGetBoolean(row, "IsPowerCard");
                    response.WaferDimension = SafeGetString(row, "WaferDimension");
                    response.InterfaceBoardId = SafeGetNullableInt(row, "InterfaceBoardId");
                    response.VendorId = SafeGetNullableInt(row, "VendorId");
                    response.DTSId = SafeGetString(row, "OldDTSId");
                    response.IsActive = SafeGetBoolean(row, "Active");
                    response.Comments = SafeGetString(row, "Comments");
                    response.PogoTowerISEId = SafeGetString(row, "PTISEID");
                    response.InterfaceBoardISEId = SafeGetString(row, "IBISEID");
                    response.PogoTowerIsIse = SafeGetBoolean(row, "IsIsePT");
                    response.InterfaceBoardIsIse = SafeGetBoolean(row, "IsIseIB");
                    response.SubSlotId = SafeGetNullableInt(row, "subSlotId");
                    response.ShelfId = SafeGetNullableInt(row, "SlotId");
                    response.Shelf = SafeGetString(row, "slotName");
                    response.SubSlot = SafeGetString(row, "subslotName");
                    response.Status = SafeGetString(row, "Status");
                    response.IsTempShip = SafeGetNullableInt(row, "IsTempShipOut");
                    response.LocationStatusId = SafeGetNullableInt(row, "LocationStatusId");
                    response.BoardTypeId = SafeGetNullableInt(row, "BoardTypeId");
                    response.SecondaryCustomerName = SafeGetString(row, "SecondaryCustomerId");
                }

                // Table 1: Equipment IDs
                if (dataSet.Tables.Count > 1 && dataSet.Tables[1].Rows.Count > 0)
                {
                    response.EquipmentIds = new List<int>();
                    response.EquipmentIdStr = string.Empty;
                    foreach (DataRow row in dataSet.Tables[1].Rows)
                    {
                        var equipmentId = SafeGetInt32(row, "EquipmentId");
                        if (equipmentId > 0)
                        {
                            response.EquipmentIds.Add(equipmentId);
                            response.EquipmentIdStr += equipmentId + ",";
                        }
                    }
                    if (!string.IsNullOrEmpty(response.EquipmentIdStr))
                    {
                        response.EquipmentIdStr = response.EquipmentIdStr.TrimEnd(',');
                    }
                }

                // Table 2: Platform IDs
                if (dataSet.Tables.Count > 2 && dataSet.Tables[2].Rows.Count > 0)
                {
                    response.PlatformIds = new List<int>();
                    response.PlatformIdStr = string.Empty;
                    foreach (DataRow row in dataSet.Tables[2].Rows)
                    {
                        var platformId = SafeGetInt32(row, "PlatformID");
                        if (platformId > 0)
                        {
                            response.PlatformIds.Add(platformId);
                            response.PlatformIdStr += platformId + ",";
                        }
                    }
                    if (!string.IsNullOrEmpty(response.PlatformIdStr))
                    {
                        response.PlatformIdStr = response.PlatformIdStr.TrimEnd(',');
                    }
                }

                // Table 3: Attachments
                if (dataSet.Tables.Count > 3 && dataSet.Tables[3].Rows.Count > 0)
                {
                    response.Attachments = new List<ProbeCardAttachmentResponse>();
                    foreach (DataRow row in dataSet.Tables[3].Rows)
                    {
                        response.Attachments.Add(new ProbeCardAttachmentResponse
                        {
                            AttachmentId = SafeGetInt32(row, "AttachmentId"),
                            FilePath = SafeGetString(row, "FilePath"),
                            AttachedById = SafeGetNullableInt(row, "AttachedById"),
                            AttachedBy = SafeGetString(row, "AttachedBy"),
                            Void = SafeGetBoolean(row, "Active") == false // Active = false means void
                        });
                    }
                }

                // Table 4: External Equipment and Correlations
                if (dataSet.Tables.Count > 4 && dataSet.Tables[4].Rows.Count > 0)
                {
                    response.ExternalEquipment = new List<ProbeCardExternalEquipmentResponse>();
                    response.Correlations = new List<ProbeCardCorrelationResponse>();
                    
                    foreach (DataRow row in dataSet.Tables[4].Rows)
                    {
                        var hardwareType = SafeGetInt32(row, "HardWareType");
                        // HardWareType = (Int32)UtilityClass.HardwareType.ExternalUnit (from TFS code)
                        if (hardwareType == 8) // ExternalUnit
                        {
                            response.ExternalEquipment.Add(new ProbeCardExternalEquipmentResponse
                            {
                                ExEquipmentId = SafeGetInt32(row, "ExEquipmentId"),
                                ExISEId = SafeGetString(row, "ExISEId"),
                                IsExEqip = SafeGetString(row, "IsExEqip")
                            });
                        }
                        // HardWareType = (Int32)UtilityClass.HardwareType.WSCorrelation
                        else if (hardwareType == 9) // WSCorrelation
                        {
                            response.Correlations.Add(new ProbeCardCorrelationResponse
                            {
                                ExEquipmentId = SafeGetInt32(row, "ExEquipmentId"),
                                ExISEId = SafeGetString(row, "ExISEId"),
                                IsExEqip = SafeGetString(row, "IsExEqip")
                            });
                        }
                    }
                }
            }

            return response;
        }

        private int SafeGetInt32(DataRow row, string columnName)
        {
            if (row.Table.Columns.Contains(columnName) && row[columnName] != DBNull.Value)
            {
                if (int.TryParse(row[columnName].ToString(), out int result))
                    return result;
            }
            return 0;
        }

        private int? SafeGetNullableInt(DataRow row, string columnName)
        {
            if (row.Table.Columns.Contains(columnName) && row[columnName] != DBNull.Value)
            {
                if (int.TryParse(row[columnName].ToString(), out int result))
                    return result;
            }
            return null;
        }

        private bool SafeGetBoolean(DataRow row, string columnName)
        {
            if (row.Table.Columns.Contains(columnName) && row[columnName] != DBNull.Value)
            {
                if (bool.TryParse(row[columnName].ToString(), out bool result))
                    return result;
                if (int.TryParse(row[columnName].ToString(), out int intResult))
                    return intResult != 0;
            }
            return false;
        }

        private bool? SafeGetNullableBoolean(DataRow row, string columnName)
        {
            if (row.Table.Columns.Contains(columnName) && row[columnName] != DBNull.Value)
            {
                if (bool.TryParse(row[columnName].ToString(), out bool result))
                    return result;
                if (int.TryParse(row[columnName].ToString(), out int intResult))
                    return intResult != 0;
            }
            return null;
        }

        private string? SafeGetString(DataRow row, string columnName)
        {
            if (row.Table.Columns.Contains(columnName) && row[columnName] != DBNull.Value)
            {
                return row[columnName].ToString();
            }
            return null;
        }

        public async Task<List<MasterDataItem>> GetPlatforms()
        {
            var dataTable = await _repository.GetPlatforms();
            var result = new List<MasterDataItem>();
            
            // Add default "--Select--" option
            result.Add(new MasterDataItem { Id = -1, Name = "--Select--" });

            if (dataTable != null && dataTable.Rows != null)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    result.Add(new MasterDataItem
                    {
                        Id = SafeGetInt32(row, "PlatformId"),
                        Name = SafeGetString(row, "PlatformName") ?? string.Empty
                    });
                }
            }

            return result;
        }

        public async Task<List<MasterDataItem>> GetProbeCardTypes()
        {
            var dataTable = await _repository.GetProbeCardTypes();
            var result = new List<MasterDataItem>();
            
            // Add default "--Select--" option
            result.Add(new MasterDataItem { Id = -1, Name = "--Select--" });

            if (dataTable != null && dataTable.Rows != null)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    result.Add(new MasterDataItem
                    {
                        Id = SafeGetInt32(row, "ProbTypeId"),
                        Name = SafeGetString(row, "ProbType") ?? string.Empty
                    });
                }
            }

            return result;
        }

        public async Task<List<MasterDataItem>> GetProbers()
        {
            var dataTable = await _repository.GetProbers();
            var result = new List<MasterDataItem>();
            
            // Add default "--Select--" option
            result.Add(new MasterDataItem { Id = -1, Name = "--Select--" });

            if (dataTable != null && dataTable.Rows != null)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    result.Add(new MasterDataItem
                    {
                        Id = SafeGetInt32(row, "EquipmentId"),
                        Name = SafeGetString(row, "EquipmentName") ?? string.Empty
                    });
                }
            }

            return result;
        }

        public async Task<List<MasterDataItem>> GetBoardTypes()
        {
            var dataTable = await _repository.GetBoardTypes();
            var result = new List<MasterDataItem>();
            
            // Add default "--Select--" option
            result.Add(new MasterDataItem { Id = -1, Name = "--Select--" });

            if (dataTable != null && dataTable.Rows != null)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    result.Add(new MasterDataItem
                    {
                        Id = SafeGetInt32(row, "BoardTypeId"),
                        Name = SafeGetString(row, "BoardType") ?? string.Empty
                    });
                }
            }

            return result;
        }

        public async Task<List<SlotItem>> GetSlots(int? hardwareTypeId, string? platformId)
        {
            var dataTable = await _repository.GetSlots(hardwareTypeId, platformId);
            var result = new List<SlotItem>();

            if (dataTable != null && dataTable.Rows != null)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    result.Add(new SlotItem
                    {
                        SlotId = SafeGetInt32(row, "SlotId"),
                        SlotName = SafeGetString(row, "SlotName") ?? SafeGetString(row, "slotName") ?? string.Empty,
                        IsMultipleHWAllowed = SafeGetBoolean(row, "IsMultipleHWAllowed") || SafeGetBoolean(row, "isMultipleHWAllowed"),
                        HWLimit = SafeGetInt32(row, "HWLimit") != 0 ? SafeGetInt32(row, "HWLimit") : SafeGetInt32(row, "hwLimit")
                    });
                }
            }

            return result;
        }

        public async Task<List<SubSlotItem>> GetSubSlots(int slotId)
        {
            var dataTable = await _repository.GetSubSlots(slotId);
            var result = new List<SubSlotItem>();

            if (dataTable != null && dataTable.Rows != null)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    result.Add(new SubSlotItem
                    {
                        SubSlotId = SafeGetInt32(row, "SubSlotId") != 0 ? SafeGetInt32(row, "SubSlotId") : SafeGetInt32(row, "subSlotId"),
                        SubSlotName = SafeGetString(row, "SubSlotName") ?? SafeGetString(row, "subSlotName") ?? string.Empty,
                        MinSlot = SafeGetInt32(row, "MinSlot") != 0 ? SafeGetInt32(row, "MinSlot") : SafeGetInt32(row, "minSlot"),
                        MaxSlot = SafeGetInt32(row, "MaxSlot") != 0 ? SafeGetInt32(row, "MaxSlot") : SafeGetInt32(row, "maxSlot")
                    });
                }
            }

            return result;
        }

        public async Task<List<LocationInfoItem>> GetLocationsInfo(int subSlotId)
        {
            var dataTable = await _repository.GetLocationsInfo(subSlotId);
            var result = new List<LocationInfoItem>();

            if (dataTable != null && dataTable.Rows != null)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    result.Add(new LocationInfoItem
                    {
                        Location = SafeGetInt32(row, "Location") != 0 ? SafeGetInt32(row, "Location") : SafeGetInt32(row, "location"),
                        LocationStatusId = SafeGetInt32(row, "LocationStatusId") != 0 ? SafeGetInt32(row, "LocationStatusId") : SafeGetInt32(row, "locationStatusId"),
                        ISEID = SafeGetString(row, "ISEID") ?? SafeGetString(row, "iseID") ?? SafeGetString(row, "IseId"),
                        SubSlotId = SafeGetInt32(row, "SubSlotId") != 0 ? SafeGetInt32(row, "SubSlotId") : SafeGetInt32(row, "subSlotId")
                    });
                }
            }

            return result;
        }
    }
}
