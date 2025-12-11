using System.ComponentModel.DataAnnotations;

namespace ISEMES.Models
{
    public class DeviceDetails
    {
        [Key]
        public int DeviceID { get; set; }
        public int InventoryID { get; set; }
        public int ReceiptID { get; set; }
        public string? ISELotNumber { get; set; }
        public string? CustomerLotNumber { get; set; }
        public int CustomerCount { get; set; }
        public bool Expedite { get; set; }
        public bool IQA { get; set; }
        public string? LotIdentifier { get; set; }
        public int LotOwnerID { get; set; }
        public string? LotOwner { get; set; }
        public int? LabelCount { get; set; }
        public int? DeviceTypeID { get; set; }
        public string? DeviceType { get; set; }
        public int LotCategoryID { get; set; }
        public string? COO { get; set; }
        public string? DateCode { get; set; }
        public bool IsHold { get; set; }
        public string? HoldComments { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public bool Active { get; set; }
        public bool IsReceived { get; set; }
        public bool CanEdit { get; set; }
        public int LotId { get; set; }
    }

    public class DeviceRequest
    {        
        public int? DeviceID { get; set; }        
        public int ReceiptID { get; set; }
        public string? CustomerLotNumber { get; set; }
        public int CustomerCount { get; set; }
        public bool Expedite { get; set; }
        public bool IQA { get; set; }
        public string? LotIdentifier { get; set; }
        public int LotOwnerID { get; set; }
        public int? LabelCount { get; set; }
        public int? DeviceTypeID { get; set; }
        public int LotCategoryID { get; set; }
        public string? DateCode { get; set; }
        public int? COO { get; set; }
        public bool IsHold { get; set; }
        public string? HoldComments { get; set; }
        public string RecordStatus { get; set; }
        public bool Active { get; set; }
        public int LoginId { get; set; }
        public string? IseLotNumber { get; set; }
        public Boolean IsReceived { get; set; }
    }

    public class DeviceDetailsRequest
    {
        public List<DeviceRequest> DeviceDetails { get; set; }
    }  

    public class InterimDevice : DeviceDetails
    {
        public int? InterimReceiptID { get; set; }
        public int? ReceivedQTY { get; set; }
        public int? GoodQty { get; set; }
        public int? RejectedQty { get; set; }
        public DateTime InterimReceivedOn { get; set; }
    }

    public class InterimDeviceDetail
    {
        public int InventoryID { get; set; }
        public int InterimReceiptID { get; set; }
        public int? ReceivedQTY { get; set; }
        public int? GoodQty { get; set; }
        public int? RejectedQty { get; set; }
        public string RecordStatus { get; set; }
        public int InterimStatusID { get; set; }
        public int UserID { get; set; }
        public bool IsHold { get; set; }
        public bool IsReceived { get; set; }
        public bool Active { get; set; }
    }
}

