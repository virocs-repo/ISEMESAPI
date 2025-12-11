using System.ComponentModel.DataAnnotations;

namespace ISEMES.Models
{
    public class AnotherShipment
    {
        [Key]
        public int AnotherShipmentID {  get; set; }
        public string Requestor { get; set; }
        public string? DeliveryMethod { get; set; }
        public string CustomerType { get; set; }
        public string CustomerVendor { get; set; }
        public string ShipTo { get; set; }
        public string RecordStatus { get; set; }
        public string Status { get; set; }
        public int? DeliveryInfoId { get; set; }
    }

    public class AnotherShipmentDetail
    {
        [Key]
        public int AnotherShipmentID { get; set; }
        public int RequestorID { get; set; }
        public string Email { get; set; }
        public string RecipientName { get; set; }
        public string PhoneNo { get; set; }
        public int CustomerTypeID { get; set; }
        public int CustomerVendorID { get; set; }
        public int BehalfID { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
        public string Instructions { get; set; }
        public string Status { get; set; }
        public string RecordStatus { get; set; }
        public int UserId { get; set; }
        public int? ApproverID { get; set; }
        public int? ApprovedBy { get; set; }
        public DateTime ApprovedON { get; set; }
        public int ServiceTypeID { get; set; }
        public string AccountNo { get; set; }
        public List<AnotherShipmentLineItem> AnotherShipLineItems { get; set; }
    }

    public class AnotherShipmentLineItem
    {
        [Key]
        public int LineItemID { get; set; }
        public int InventoryID { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public int Value { get; set; }
        public string LotNumber { get; set; }
        public int Status { get; set; }
    }
}

