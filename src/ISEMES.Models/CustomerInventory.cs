namespace ISEMES.Models
{
    public class CustomerInventory
    {
        public int ReceiptID { get; set; }
        public int InventoryID { get; set; }
        public string? GoodType { get; set; }
        public int? HardwareTypeId { get; set; }
        public string? HardwareType { get; set; }
        public string? ISELotNum { get; set; }
        public string? CustomerLotNum { get; set; }
        public int ExpectedQty { get; set; }
        public bool? Expedite { get; set; }
        public string? PartNum { get; set; }
        public int LabelCount { get; set; }
        public string? COO { get; set; }
        public string? DateCode { get; set; }
        public bool? IsHold { get; set; }
        public string? HoldComments { get; set; }
        public string? AdditionalInfo { get; set; }
        public int? ShippedQty { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public bool Active { get; set; }
    }
}

