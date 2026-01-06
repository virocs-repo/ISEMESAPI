namespace ISEMES.Models
{
    public class InventoryDetail
    {
        public string? GoodType { get; set; }
        public int InventoryID { get; set; }
        public int? ReceiptID { get; set; }
        public string? ISELotNumber { get; set; }
        public int Qty { get; set; }
        public int Expedite { get; set; }
        public string? PartNum { get; set; }
        public string? FGTPartNum { get; set; }
        public int Unprocessed { get; set; }
        public int Good { get; set; }
        public int Reject { get; set; }
        public string? COO { get; set; }
        public string? DateCode { get; set; }
        public string? Status { get; set; }
        public string? Hold { get; set; }
    }

    public class ShipmentAddInventory
    {
        public int InventoryID { get; set; }
        public string CustomerLotNum { get; set; } = string.Empty;
        public string ISELotNum { get; set; } = string.Empty;
        public string GoodsType { get; set; } = string.Empty;
        public string? PartNum { get; set; }
        public int CurrentQty { get; set; }
        public int ShipmentQty { get; set; }
        public int ShipmentTypeID { get; set; }
        public string ShipmentType { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public int? LocationID { get; set; }
        public int ReceivedFromID { get; set; }
        public int CustomerID { get; set; }
        public string CustomerName { get; set; } = string.Empty;
    }

    public class CreateAddShipRequest
    {
        public int CustomerID { get; set; }
        public int? CurrentLocationID { get; set; }
        public string InventoryIds { get; set; }
        public int? ShipmentCategoryID { get; set; }
        public int UserID { get; set; }
    }

    public class InventoryLosts
    {
        public int InventoryID { get; set; }
        public string ISELotNumber { get; set; }
    }
}



