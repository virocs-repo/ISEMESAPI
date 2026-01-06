using System.ComponentModel.DataAnnotations;

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

    public class DeviceMasterRequest
    {
        public int DeviceId { get; set; } = -1;
        [Required]
        public string DeviceName { get; set; } = string.Empty;
        [Required]
        public int DeviceFamilyId { get; set; }
        [Required]
        public int CustomerID { get; set; }
        public bool IsActive { get; set; } = true;
        public int CreatedBy { get; set; }
        
        public string? TestDevice { get; set; }
        public string? ReliabilityDevice { get; set; }
        public List<string>? AliasNames { get; set; }
        public string? SKU { get; set; }
        public string? PartType { get; set; }
        [Required]
        public string? LotType { get; set; } = "Standard";
        public bool LabelMapping { get; set; }
        public string? TrayTubeMapping { get; set; } = "Device";
        public int? CountryOfOriginId { get; set; }
        public decimal? UnitCost { get; set; }
        
        public int? MaterialDescriptionId { get; set; }
        public int? USHTSCodeId { get; set; }
        public int? ECCNId { get; set; }
        public int? LicenseExceptionId { get; set; }
        public int? RestrictedCountriesToShipId { get; set; }
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
        
        public int? LockId { get; set; } = -1;
        public string? LastModifiedOn { get; set; }
        
        public string? Labels { get; set; }
        public List<LabelInfo>? lstLabelDetails { get; set; }
    }
    
    public class LabelInfo
    {
        public string LabelName { get; set; } = string.Empty;
        public string LabelDetails { get; set; } = string.Empty;
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
    }

    public class DeviceFamilySearchRequest
    {
        public int? CustomerID { get; set; }
        public string? DeviceFamilyName { get; set; }
    }

    public class DeviceInfoResponse
    {
        public bool CanEdit { get; set; } = true;
        public string? LastModifiedOn { get; set; }
        public bool CanEditlotType { get; set; } = true;
        public bool CanEditLabel1 { get; set; } = true;
        public bool CanEditLabel2 { get; set; } = true;
        public bool CanEditLabel3 { get; set; } = true;
        public bool CanEditLabel4 { get; set; } = true;
        public bool CanEditLabel5 { get; set; } = true;
        public List<Dictionary<string, object>> DQP { get; set; } = new List<Dictionary<string, object>>();
        public List<Dictionary<string, object>> MF { get; set; } = new List<Dictionary<string, object>>();
        public List<Dictionary<string, object>> TRV { get; set; } = new List<Dictionary<string, object>>();
        public List<Dictionary<string, object>> Boards { get; set; } = new List<Dictionary<string, object>>();
        public List<Dictionary<string, object>> DeviceLabelInfo { get; set; } = new List<Dictionary<string, object>>();
    }
}
