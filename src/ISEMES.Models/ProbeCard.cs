using System.ComponentModel.DataAnnotations;

namespace ISEMES.Models
{
    public class ProbeCardSearchRequest
    {
        public int? CustomerId { get; set; } = -1;
        public string? ISEId { get; set; } = string.Empty;
        public string? CustomerHWId { get; set; } = string.Empty;
        public int? PlatformId { get; set; } = -1;
        public int? ProbeCardTypeId { get; set; } = -1;
        public int? EquipmentId { get; set; } = -1;
        public int HardwareTypeId { get; set; } = 4; // Default to ProbeCard hardware type
        public int? IsActive { get; set; } = -1; // -1 for all, 0 for inactive, 1 for active
        public int? BoardTypeId { get; set; } = -1;
    }

    public class ProbeCardResponse
    {
        public int MasterId { get; set; }
        public int ProbeCardId { get; set; }
        public string? ISEId { get; set; }
        public string? CustomerName { get; set; }
        public int? CustomerId { get; set; }
        public string? CustomerHWId { get; set; }
        public string? HardwareType { get; set; }
        public string? Location { get; set; }
        public string? Status { get; set; }
        public bool? IsActive { get; set; }
        public string? PlatformName { get; set; }
        public string? ProbeCardType { get; set; }
        public string? EquipmentName { get; set; }
        public string? BoardType { get; set; }
        public string? CreatedOn { get; set; }
        public string? ModifiedOn { get; set; }
        public string? SecondaryCustomerName { get; set; }
        public string? DeviceFamily { get; set; }
        public string? Device { get; set; }
        public string? DeviceAlias { get; set; }
    }

    public class ProbeCardDetailsResponse
    {
        public int MasterId { get; set; }
        public int ProbeCardId { get; set; }
        public string? ISEId { get; set; }
        public string? GenISEID { get; set; }
        public bool IsITAR { get; set; }
        public int ProbeCardTypeId { get; set; }
        public string? ProbeCardType_Others { get; set; }
        public int? CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerHardwareId { get; set; }
        public int HardwareLocation { get; set; }
        public string? ProbeCardDimension { get; set; }
        public int? PogoTowerId { get; set; }
        public bool IsPowerCard { get; set; }
        public string? WaferDimension { get; set; }
        public int? InterfaceBoardId { get; set; }
        public int? VendorId { get; set; }
        public string? DTSId { get; set; }
        public bool IsActive { get; set; }
        public string? Comments { get; set; }
        public string? PogoTowerISEId { get; set; }
        public string? InterfaceBoardISEId { get; set; }
        public bool PogoTowerIsIse { get; set; }
        public bool InterfaceBoardIsIse { get; set; }
        public int? SubSlotId { get; set; }
        public int? ShelfId { get; set; }
        public string? Shelf { get; set; }
        public string? SubSlot { get; set; }
        public string? Status { get; set; }
        public int? IsTempShip { get; set; }
        public int? LocationStatusId { get; set; }
        public int? BoardTypeId { get; set; }
        public string? SecondaryCustomerName { get; set; }
        public List<int>? PlatformIds { get; set; }
        public string? PlatformIdStr { get; set; }
        public List<int>? EquipmentIds { get; set; }
        public string? EquipmentIdStr { get; set; }
        public List<ProbeCardAttachmentResponse>? Attachments { get; set; }
        public List<ProbeCardExternalEquipmentResponse>? ExternalEquipment { get; set; }
        public List<ProbeCardCorrelationResponse>? Correlations { get; set; }
    }

    public class ProbeCardAttachmentResponse
    {
        public int AttachmentId { get; set; }
        public string? FilePath { get; set; }
        public int? AttachedById { get; set; }
        public string? AttachedBy { get; set; }
        public bool Void { get; set; }
    }

    public class ProbeCardExternalEquipmentResponse
    {
        public int ExEquipmentId { get; set; }
        public string? ExISEId { get; set; }
        public string? IsExEqip { get; set; }
    }

    public class ProbeCardCorrelationResponse
    {
        public int ExEquipmentId { get; set; }
        public string? ExISEId { get; set; }
        public string? IsExEqip { get; set; }
    }

    public class MasterDataItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class SlotItem
    {
        public int SlotId { get; set; }
        public string SlotName { get; set; } = string.Empty;
        public bool IsMultipleHWAllowed { get; set; }
        public int HWLimit { get; set; }
    }

    public class SubSlotItem
    {
        public int SubSlotId { get; set; }
        public string SubSlotName { get; set; } = string.Empty;
        public int MinSlot { get; set; }
        public int MaxSlot { get; set; }
    }

    public class LocationInfoItem
    {
        public int Location { get; set; }
        public int LocationStatusId { get; set; }
        public string? ISEID { get; set; }
        public int SubSlotId { get; set; }
    }

    /// <summary>Request for Correlation (WSCorrelation) search used in probe card picker.</summary>
    public class CorrelationSearchRequest
    {
        public int? CustomerId { get; set; } = -1;
        public string? ISEId { get; set; } = string.Empty;
        public string? CustomerHWId { get; set; } = string.Empty;
        /// <summary>Hardware type for Correlation (e.g. 9 = WSCorrelation).</summary>
        public int HardwareTypeId { get; set; } = 9;
        /// <summary>1 = external/customer, 0 = ISE Labs.</summary>
        public int IsISE { get; set; } = 1;
        public int IsActive { get; set; } = 1;
        public bool IsPicker { get; set; } = true;
    }

    /// <summary>Correlation item returned from search for multi-select picker.</summary>
    public class CorrelationSearchResponse
    {
        public int MasterId { get; set; }
        public int ExEquipmentId { get; set; }
        public string? ISEId { get; set; }
        public string? CustomerName { get; set; }
        public string? HardwareType { get; set; }
        public string? Location { get; set; }
    }
}
