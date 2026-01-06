namespace ISEMES.Models
{
    public class CustomerOrder
    {
        public int? CustomerOrderID { get; set; }
        public int CustomerId { get; set; }
        public bool OQA { get; set; }
        public bool Bake { get; set; }
        public bool PandL { get; set; }
        public string CompanyName { get; set; }
        public string ContactPerson { get; set; }
        public string ContactPhone { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
        public string? OrderStatus { get; set; }
        public string RecordStatus { get; set; }
        public bool Active { get; set; }
        public List<CustomerOrderDetails> CustomerOrderDetails { get; set; } = new();
    }

    public class OrderRequest
    {
        public List<CustomerOrder> CustomerOrder { get; set; } = new();
    }

    public class CustomerOrderDetails
    {
        public int? CustomerOrderDetailID { get; set; }
        public int InventoryID { get; set; }
        public int ShippedQty { get; set; }
        public string RecordStatus { get; set; }
    }

    public class CustomerEditDetail
    {
        public int? CustomerOrderID { get; set; }
        public int? CustomerOrderDetailID { get; set; }
        public string GoodsType { get; set; }
        public int InventoryID { get; set; }
        public string ISELotNum { get; set; }
        public string CustomerLotNum { get; set; }
        public int ExpectedQty { get; set; }
        public bool? Expedite { get; set; }
        public string PartNum { get; set; }
        public int Unprocessed { get; set; }
        public int Good { get; set; }
        public int Reject { get; set; }
        public string COO { get; set; }
        public string DateCode { get; set; }
        public int? ShippedQty { get; set; }
    }
}



