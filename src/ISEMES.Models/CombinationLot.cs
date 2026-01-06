namespace ISEMES.Models
{
    public class CombinationLot
    {
        public string ComboLotID { get; set; }
        public string ComboName { get; set; }
        public string Customer { get; set; }
        public string Status { get; set; }
        public string ISELotNumber { get; set; }
        public string CustomerLotNumber { get; set; }
        public string DeviceType { get; set; }
        public bool IsPrimaryAddress { get; set; }
    }

    public class CustomerCombLot
    {
        public int ReceiptID { get; set; }
        public int CustomerVendorID { get; set; }
        public string GoodsType { get; set; }
        public int InventoryID { get; set; }
        public string ISELotNum { get; set; }
        public string CustomerLotNum { get; set; }
        public int ExpectedQty { get; set; }
        public bool Expedite { get; set; }
        public string? PartNum { get; set; }
        public int LabelCount { get; set; }
        public string COO { get; set; }
        public string DateCode { get; set; }
        public bool IsHold { get; set; }
        public bool Active { get; set; }
        public int? AddressId { get; set; }
        public string Address1 { get; set; }
        public int? RunningCount { get; set; }
    }

    public class UpsertCombineLotRequest
    {
        public int? ComboLotID { get; set; }
        public string ComboName { get; set; }
        public string Str_InventoryId { get; set; }
        public int Primary_InventoryId { get; set; }
        public int UserID { get; set; }
        public bool Active { get; set; }
        public string Comments { get; set; }
    }

    public class CombineLotsDto
    {
        public int ReceiptID { get; set; }
        public int CustomerVendorID { get; set; }
        public int BehalfID { get; set; }
        public string GoodsType { get; set; }
        public int InventoryID { get; set; }
        public string ISELotNum { get; set; }
        public string CustomerLotNum { get; set; }
        public int ExpectedQty { get; set; }
        public bool Expedite { get; set; }
        public string? PartNum { get; set; }
        public int? LabelCount { get; set; }
        public string COO { get; set; }
        public string DateCode { get; set; }
        public bool IsHold { get; set; }
        public bool Active { get; set; }
        public int ComboLotID { get; set; }
        public int ViewFlag { get; set; }
        public bool IsPrimaryAddress { get; set; }
        public string ComboName { get; set; }
        public string Comments { get; set; }
        public int? AddressId { get; set; }
        public string Address1 { get; set; }
        public int? RunningCount { get; set; }
    }
}



