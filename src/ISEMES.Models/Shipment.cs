using System.ComponentModel.DataAnnotations;

namespace ISEMES.Models
{
    public class Shipment
    {
        [Key]
        public int ShipmentId { get; set; }
        public int CustomerID { get; set; }
        public string? Customer { get; set; }
        public string ShipmentNum { get; set; }        
        public int CurrentLocationID { get; set; }
        public int ShipmentCategoryID { get; set; }
        public string? ShipmentCategory { get; set; }
        public string ShipmentLocation { get; set; }
        public string? CurrentLocation { get; set; }
        public string SenderInfo { get; set; }
        public string CustomerInfo { get; set; }
        public string? ShippmentInfo { get; set; }
        public string? Trackingurl { get; set; }
        public string? Trackingnum { get; set; }
        public Boolean IsShipped { get; set; }        
        public DateTime ModifiedOn { get; set; }
        public string? DeliveryMethod { get; set; }
        public string? ContactPerson { get; set; }
        public int? DeliveryInfoId { get; set; }
    }

    public class ShipmentRequest
    {
        public int? ShipmentID { get; set; }
        public int CustomerID { get; set; }
        public string ShipmentNum { get; set; }
        public int ShipmentCategoryID { get; set; }
        public string ShipmentLocation { get; set; }
        public int CurrentLocationID { get; set; }
        public string SenderInfo { get; set; }
        public string CustomerInfo { get; set; }
        public string ShippmentInfo { get; set; }        
        public string RecordStatus { get; set; }
        public bool Active { get; set; }
        public bool IsShipped { get; set; }        
        public int LoginId { get; set; }
        public List<ShipmentDetail> ShipmentDetails { get; set; }
    }

    public class ShipmentDetail
    {
        public int? ShipmentLineItemID { get; set; }
        public int InventoryID { get; set; }
    }

    public class CreateShipmentLabelRequest
    {
        public string? ToName { get; set; }
        public string? ToStreet1 { get; set; }
        public string? ToCity { get; set; }
        public string? ToState { get; set; }
        public string? ToZip { get; set; }
        public string? ToCountry { get; set; }
        public string? ClientAccountNumber { get; set; }
        public int ShipmentID { get; set; }
        public int UserID { get; set; }
        public List<ParcelRequest>? Parcels { get; set; }
    }

    public class ParcelRequest
    {
        public decimal Length { get; set; }
        public decimal Width { get; set; }
        public decimal Height { get; set; }
        public decimal Weight { get; set; }
        public int Pno { get; set; }
    }
}
