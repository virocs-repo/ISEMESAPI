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
        
        // Device Info fields
        public string? TestDevice { get; set; }
        public string? ReliabilityDevice { get; set; }
        public List<string>? AliasNames { get; set; }
        public string? SKU { get; set; }
        public string? PartType { get; set; }
        [Required]
        public string? LotType { get; set; } = "Standard";
        public bool LabelMapping { get; set; }
        public string? TrayTubeMapping { get; set; } = "Device"; // "Lot" or "Device"
        public int? CountryOfOriginId { get; set; }
        public decimal? UnitCost { get; set; }
        
        // EAR Info fields
        public int? MaterialDescriptionId { get; set; }
        public int? USHTSCodeId { get; set; }
        public int? ECCNId { get; set; }
        public int? LicenseExceptionId { get; set; }
        public int? RestrictedCountriesToShipId { get; set; }
        public bool ScheduleB { get; set; }
        
        // Pack&Label Info fields
        public int? MSLId { get; set; }
        public int? PeakPackageBodyTemperatureId { get; set; }
        public int? ShelfLifeMonthId { get; set; }
        public int? FloorLifeId { get; set; }
        public int? PBFreeId { get; set; }
        public int? PBFreeStickerId { get; set; }
        public int? ROHSId { get; set; }
        public int? TrayTubeStrappingId { get; set; }
        public int? TrayStackingId { get; set; }
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
}
