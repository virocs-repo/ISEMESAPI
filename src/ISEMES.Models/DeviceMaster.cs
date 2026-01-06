using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace ISEMES.Models
{
    public class DeviceFamilyRequest
    {
        public int DeviceFamilyId { get; set; } = -1;
        [Required]
        public string DeviceFamilyName { get; set; } = string.Empty;
        [Required]
        public int CustomerID { get; set; }
        public bool IsActive { get; set; } = true;
        public int CreatedBy { get; set; }
    }

    public class DeviceFamilyResponse
    {
        public int DeviceFamilyId { get; set; }
        public string DeviceFamilyName { get; set; } = string.Empty;
        public string? CustomerDeviceFamily { get; set; } = string.Empty;
        public int CustomerID { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public bool Active { get; set; }
        public string CreatedOn { get; set; } = string.Empty;
        public string ModifiedOn { get; set; } = string.Empty;
    }

    [XmlRoot("PRD_DeviceFamilyBO")]
    public class DeviceMasterRequest
    {
        [XmlElement("DeviceId")]
        public int DeviceId { get; set; } = -1;
        [Required]
        [XmlElement("DeviceName")]
        public string DeviceName { get; set; } = string.Empty;
        [Required]
        [XmlElement("DeviceFamilyID")]
        public int DeviceFamilyId { get; set; }
        [Required]
        [XmlElement("CustomerID")]
        public int CustomerID { get; set; }
        [XmlElement("isActive")]
        public bool IsActive { get; set; } = true;
        [XmlElement("CreatedBy")]
        public int CreatedBy { get; set; }
        
        [XmlElement("TestDevice")]
        public string? TestDevice { get; set; }
        [XmlElement("ReliabilityDevice")]
        public string? ReliabilityDevice { get; set; }
        [XmlArray("lstDeviceAliasNames")]
        [XmlArrayItem("PRD_DeviceAliasNames")]
        public List<DeviceAliasName>? AliasNames { get; set; }
        [XmlElement("SKU")]
        public string? SKU { get; set; }
        [XmlElement("PartTypeId")]
        public int? PartTypeId { get; set; }
        [Required]
        [XmlElement("DeviceTypeId")]
        public int? DeviceTypeId { get; set; }
        [XmlElement("IsLabelMapped")]
        public bool LabelMapping { get; set; }
        [XmlElement("IsDeviceBasedTray")]
        public bool IsDeviceBasedTray { get; set; }
        [XmlElement("COOId")]
        public int? CountryOfOriginId { get; set; }
        [XmlElement("UnitCost")]
        public decimal? UnitCost { get; set; }
        
        [XmlElement("MaterialDescriptionId")]
        public int? MaterialDescriptionId { get; set; }
        [XmlElement("USHTSCodeId")]
        public int? USHTSCodeId { get; set; }
        [XmlElement("ECCNId")]
        public int? ECCNId { get; set; }
        [XmlElement("LicenseExceptionId")]
        public int? LicenseExceptionId { get; set; }
        [XmlElement("RestrictedCountriesIds")]
        public string? RestrictedCountriesToShipId { get; set; }
        [XmlElement("ScheduleB")]
        public bool ScheduleB { get; set; }
        
        [XmlElement("MSL")]
        public int? MSLId { get; set; }
        [XmlElement("PeakPacckageBody")]
        public int? PeakPackageBodyTemperatureId { get; set; }
        [XmlElement("ShelfLife")]
        public int? ShelfLifeMonthId { get; set; }
        [XmlElement("FloorLife")]
        public int? FloorLifeId { get; set; }
        [XmlElement("PBFree")]
        public int? PBFreeId { get; set; }
        [XmlElement("PBFreeSticker")]
        public int? PBFreeStickerId { get; set; }
        [XmlElement("ROHS")]
        public int? ROHSId { get; set; }
        [XmlElement("TrayStrapping")]
        public int? TrayTubeStrappingId { get; set; }
        [XmlElement("TrayStacking")]
        public int? TrayStackingId { get; set; }
        
        [XmlElement("LockId")]
        public int? LockId { get; set; } = -1;
        [XmlElement("LastModifiedOn")]
        public string? LastModifiedOn { get; set; }
        
        [XmlElement("Labels")]
        public string? Labels { get; set; }
        [XmlArray("lstLabelDetails")]
        [XmlArrayItem("LabelInfo")]
        public List<LabelInfo>? lstLabelDetails { get; set; }
        
        // Conditional serialization methods to exclude null values from XML (prevents DBNull casting errors in stored procedure)
        // Only serialize nullable int fields if they have a valid value (not null and > 0 or != -1 for LockId)
        public bool ShouldSerializeCountryOfOriginId() => CountryOfOriginId.HasValue && CountryOfOriginId.Value > 0;
        public bool ShouldSerializeUnitCost() => UnitCost.HasValue;
        public bool ShouldSerializeMaterialDescriptionId() => MaterialDescriptionId.HasValue && MaterialDescriptionId.Value > 0;
        public bool ShouldSerializeUSHTSCodeId() => USHTSCodeId.HasValue && USHTSCodeId.Value > 0;
        public bool ShouldSerializeECCNId() => ECCNId.HasValue && ECCNId.Value > 0;
        public bool ShouldSerializeLicenseExceptionId() => LicenseExceptionId.HasValue && LicenseExceptionId.Value > 0;
        public bool ShouldSerializeRestrictedCountriesToShipId() => !string.IsNullOrEmpty(RestrictedCountriesToShipId) && RestrictedCountriesToShipId != "-1" && RestrictedCountriesToShipId != "0";
        public bool ShouldSerializeMSLId() => MSLId.HasValue && MSLId.Value > 0;
        public bool ShouldSerializePeakPackageBodyTemperatureId() => PeakPackageBodyTemperatureId.HasValue && PeakPackageBodyTemperatureId.Value > 0;
        public bool ShouldSerializeShelfLifeMonthId() => ShelfLifeMonthId.HasValue && ShelfLifeMonthId.Value > 0;
        public bool ShouldSerializeFloorLifeId() => FloorLifeId.HasValue && FloorLifeId.Value > 0;
        public bool ShouldSerializePBFreeId() => PBFreeId.HasValue && PBFreeId.Value > 0;
        public bool ShouldSerializePBFreeStickerId() => PBFreeStickerId.HasValue && PBFreeStickerId.Value > 0;
        public bool ShouldSerializeROHSId() => ROHSId.HasValue && ROHSId.Value > 0;
        public bool ShouldSerializeTrayTubeStrappingId() => TrayTubeStrappingId.HasValue && TrayTubeStrappingId.Value > 0;
        public bool ShouldSerializeTrayStackingId() => TrayStackingId.HasValue && TrayStackingId.Value > 0;
        // LockId: only serialize if it's not null and not -1 (valid lock ID)
        public bool ShouldSerializeLockId() => LockId.HasValue && LockId.Value != -1;
        public bool ShouldSerializeLastModifiedOn() => !string.IsNullOrEmpty(LastModifiedOn);
        public bool ShouldSerializeLabels() => !string.IsNullOrEmpty(Labels);
        public bool ShouldSerializelstLabelDetails() => lstLabelDetails != null && lstLabelDetails.Count > 0;
        public bool ShouldSerializeTestDevice() => !string.IsNullOrEmpty(TestDevice);
        public bool ShouldSerializeReliabilityDevice() => !string.IsNullOrEmpty(ReliabilityDevice);
        public bool ShouldSerializeAliasNames() => AliasNames != null && AliasNames.Count > 0;
        public bool ShouldSerializeSKU() => !string.IsNullOrEmpty(SKU);
        public bool ShouldSerializePartTypeId() => PartTypeId.HasValue && PartTypeId.Value > 0;
    }
    
    public class LabelInfo
    {
        [XmlElement("LabelName")]
        public string LabelName { get; set; } = string.Empty;
        [XmlElement("LabelDetails")]
        public string LabelDetails { get; set; } = string.Empty;
    }

    [XmlType("PRD_DeviceAliasNames")]
    public class DeviceAliasName
    {
        [XmlElement("DeviceId")]
        public int DeviceId { get; set; }
        [XmlElement("AliasId")]
        public int AliasId { get; set; }
        [XmlElement("AliasName")]
        public string AliasName { get; set; } = string.Empty;
        [XmlElement("DeviceFamilyId")]
        public int DeviceFamilyId { get; set; }
    }

    public class DeviceResponse
    {
        public int DeviceId { get; set; }
        public string Device { get; set; } = string.Empty;
        public int? DeviceFamilyId { get; set; }
        public string? DeviceFamily { get; set; } = string.Empty;
        public string? CustomerDevice { get; set; } = string.Empty;
        public int? CustomerId { get; set; }
        public string? CustomerName { get; set; } = string.Empty;
        public bool Active { get; set; }
        public string CreatedOn { get; set; } = string.Empty;
        public string ModifiedOn { get; set; } = string.Empty;
        public string? TestDevice { get; set; }
        public string? ReliabilityDevice { get; set; }
        public string? DeviceAlias { get; set; }
        public decimal? UnitCost { get; set; }
        public int? DeviceTypeId { get; set; } // Lot Type ID
        public int? COOId { get; set; } // Country of Origin ID
        public int? PartTypeId { get; set; } // Part Type ID
        public int? MaterialDescriptionId { get; set; }
        public int? USHTSCodeId { get; set; }
        public int? ECCNId { get; set; }
        public int? LicenseExceptionId { get; set; }
        public string? RestrictedCountriesIds { get; set; }
        public bool ScheduleB { get; set; }
        public int? MSLId { get; set; }
        public int? PeakPackageBodyTemperatureId { get; set; }
        public int? ShelfLifeMonthId { get; set; }
        public int? FloorLifeId { get; set; }
        public int? PBFreeId { get; set; }
        public int? PBFreeStickerId { get; set; }
        public int? ROHSId { get; set; }
        public int? TrayTubeStrappingId { get; set; }
        public int? TrayStackingId { get; set; }
        public bool IsLabelMapped { get; set; }
        public bool IsDeviceBasedTray { get; set; }
        public string? SKU { get; set; }
    }

    public class DeviceFamilySearchRequest
    {
        public int? CustomerID { get; set; }
        public string? DeviceFamilyName { get; set; }
    }

    public class DeviceInfoResponse
    {
        // CanEdit flags
        public bool CanEdit { get; set; } = true;
        public string? LastModifiedOn { get; set; }
        public bool CanEditlotType { get; set; } = true;
        public bool CanEditLabel1 { get; set; } = true;
        public bool CanEditLabel2 { get; set; } = true;
        public bool CanEditLabel3 { get; set; } = true;
        public bool CanEditLabel4 { get; set; } = true;
        public bool CanEditLabel5 { get; set; } = true;
        
        // Device fields from Table[0] - #GetDevices
        public int? DeviceId { get; set; }
        public int? DeviceFamilyID { get; set; }
        public string? DeviceFamily { get; set; }
        public string? Device { get; set; }
        public string? TestDevice { get; set; }
        public string? ReliabilityDevice { get; set; }
        public int? PartTypeId { get; set; }
        public int? MaterialDescriptionId { get; set; }
        public int? USHTSCodeId { get; set; }
        public bool? ScheduleB { get; set; }
        public int? ECCNId { get; set; }
        public string? ECCN { get; set; }
        public int? LicenseExceptionId { get; set; }
        public string? RestrictedCountriesIds { get; set; }
        public bool? Active { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? CreatedBy { get; set; }
        public string? LastModifiedBy { get; set; }
        public decimal? UnitCost { get; set; }
        public bool? IsLabelMapped { get; set; }
        public string? SKU { get; set; }
        public bool? IsDeviceBasedTray { get; set; }
        public int? MSL { get; set; }
        public int? PeakPacckageBody { get; set; }
        public int? ShelfLife { get; set; }
        public int? FloorLife { get; set; }
        public int? PBFree { get; set; }
        public int? PBFreeSticker { get; set; }
        public int? ROHS { get; set; }
        public int? TrayStrapping { get; set; }
        public int? TrayStacking { get; set; }
        public int? DeviceTypeId { get; set; }
        public int? COOId { get; set; }
        
        // Label details from Table[5] - @LabelDetails
        public string? LabelCustomer { get; set; }
        public string? LabelDevice { get; set; }
        public string? Label1 { get; set; }
        public string? Label2 { get; set; }
        public string? Label3 { get; set; }
        public string? Label4 { get; set; }
        public string? Label5 { get; set; }
        
        // Usage data tables
        public List<Dictionary<string, object>> DQP { get; set; } = new List<Dictionary<string, object>>();
        public List<Dictionary<string, object>> MF { get; set; } = new List<Dictionary<string, object>>();
        public List<Dictionary<string, object>> TRV { get; set; } = new List<Dictionary<string, object>>();
        public List<Dictionary<string, object>> Boards { get; set; } = new List<Dictionary<string, object>>();
        public List<Dictionary<string, object>> DeviceLabelInfo { get; set; } = new List<Dictionary<string, object>>();
    }
}
