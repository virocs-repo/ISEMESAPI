namespace ISEMES.Models
{
    public class IntTranferReceiving
    {
        public int InternalTransferId { get; set; }
        public string LotNum { get; set; }
        public int LotQty { get; set; }
        public string Device { get; set; }
        public string? CustomerName { get; set; }
        public string DeviceFamily { get; set; }
        public int? CustomerID { get; set; }
        public int ReceivedQty { get; set; }
        public string CustomerLotNumber { get; set; }
        public string Status { get; set; }
    }

    public class InternalTransferLot
    {
        public string ISELotNum { get; set; }
        public int InventoryID { get; set; }
    }

    public class IntTransferReceiptReq
    {
        public string? InternalTransferID { get; set; }
        public int ReceivedQty { get; set; }
        public int UserId { get; set; }
        public bool Active { get; set; }
        public int InventoryID { get; set; }
    }
}

