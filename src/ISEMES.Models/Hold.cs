using System.ComponentModel.DataAnnotations;

namespace ISEMES.Models
{
    public class InventoryHolds
    {
        [Key]
        public int InventoryID { get; set; }
        public string IseLotNumber { get; set; }
        public string CustomerName { get; set; }
        public string Device {  get; set; }
        public string Status { get; set; }
        public DateTime? CommitSOD {  get; set; }
        public DateTime? HoldDate { get; set;}
        public string? ElapsedTimeOfHold{get; set; }
    }

    public class Hold
    {
        public int HoldCodeId { get; set; }
        public string HoldCode { get; set; }
        public string Source { get; set; }
        public string HoldReason { get; set; }
        public string Description { get; set; }
        public int HoldTypeId { get; set; }
        public string GroupName { get; set; }
        public bool IsReceiving { get; set; }
        public bool IsService { get; set; }
        public int? ServiceId { get; set; }
        public bool IsShipping { get; set; }
        public int? ProcessTypeId { get; set; }
    }

    public class HoldTypeResponse
    {
        public int HoldTypeId { get; set; }
        public string HoldType { get; set; }
    }

    public class HoldGroup
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public List<HoldGroupDetail> HoldGroupDetails { get; set; }
    }

    public class HoldGroupDetail
    {
        public int HoldCodeId { get; set; }
        public string HoldCode { get; set; }
    }

    public class InventoryHold
    {
        public int InventoryXHoldId { get; set; }
        public int InventoryID { get; set; }
        public string HoldType { get; set; }
        public string HoldCode { get; set; }
        public string HoldComments { get; set; }
        public DateTime? HoldDate { get; set; }
        public string OffHoldComments { get; set; }
        public DateTime? OffHoldDate { get; set; }
        public string HoldBy { get; set; }
        public string OffHoldBy { get; set; }
        public string Reason { get; set; }
        public int TFSHold {  get; set; }
        public string Source { get; set; }
        public string OnHold { get; set; }
    }

    public class HoldRequest
    {
        public int? InventoryXHoldId { get; set; }
        public int InventoryID { get; set; }
        public string Reason { get; set; }
        public string HoldComments { get; set; }
        public string HoldType { get; set; }
        public string GroupName { get; set; }
        public int HoldCodeId { get; set; }
        public string? OffHoldComments { get; set; }
        public int UserId { get; set; }
        public string CategoryName { get; set; }
        public int? ConfirmedQty { get; set; }
    }

    public class HoldDetails
    {
        public int InventoryXHoldId { get; set; }
        public int InventoryID { get; set; }
        public string Reason { get; set; }
        public string HoldComments { get; set; }
        public string HoldType { get; set; }
        public string GroupName { get; set; }
        public int HoldCodeId { get; set; }
        public string OffHoldComments { get; set; }
        public DateTime OffHoldDate { get; set; }
        public string OffHoldBy { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool Active { get; set; }
        public int TFSHold { get; set; }
        public string HoldCode { get; set; }
        public int ExpectedQty { get; set; }
        public int AvailableQty { get; set; }
        public int ConfirmedQty { get; set; }
    }

    public class HoldComment
    {
        public int HoldCommentId { get; set; }
        public string HoldComments { get; set; }
    }

    public class HoldCustomerDetails
    {
        public int? InventoryId { get; set; }
        public string CustomerName { get; set; }
        public string Device { get; set; }
        public string HoldTime { get; set; }
    }

    public class OperaterAttachements
    {
        public int? AttachmentId { get; set; }
        public int TRVStepId { get; set; }
        public int TransactionId { get; set; }
        public string AttachedFile { get; set; }
        public int AttachmentTypeId { get; set; }
        public string AttachmentType { get; set; }
        public int AttachedById { get; set; }
        public string AttachedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime AttachedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool Active { get; set; }
    }
}

