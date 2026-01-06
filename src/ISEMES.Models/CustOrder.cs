using System.ComponentModel.DataAnnotations;

namespace ISEMES.Models
{
    public class CustOrder
    {
        [Key]
        public int? CustomerOrderID { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public bool OQA { get; set; }
        public bool Bake { get; set; }
        public bool PandL { get; set; }
        public string? CompanyName { get; set; }
        public string? ContactPerson { get; set; }
        public string? ContactPhone { get; set; }
        public string Address { get; set; }
        public string? OrderStatus { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string? CustomerOrderType { get; set; }
        public int? DeliveryInfoId { get; set; }
    }

    public class MasterListItem
    {
        public int MasterListItemId { get; set; }
        public string ItemText { get; set; }
        public int? Parent { get; set; }
    }

    public class ShippingAddress
    {
        public int AddressId { get; set; }
        public string AddressType { get; set; }
        public string ContactPerson { get; set; }
        public string CompanyName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public int CountryId { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string? Extension { get; set; }
        public string? ShipTo { get; set; }
        public string? AEType { get; set; }
        public string Email { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public string PickUpAddress { get; set; }
        public bool? CanSelect { get; set; }
        public string? Alert { get; set; }
        public bool? IsDomestic { get; set; }
    }

    public class ContactPersonDetails
    {
        public int? ShippingContactId { get; set; }
        public string? ShippingContactName { get; set; }
        public int? CustomerId { get; set; }
        public int? ShippingMethodId { get; set; }
        public int? AddressId { get; set; }
        public string? ContactPerson { get; set; }
        public string? CompanyName { get; set; }
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? Address3 { get; set; }
        public string? Country { get; set; }
        public string? Zip { get; set; }
        public string? StateProvince { get; set; }
        public string? City { get; set; }
        public string? Phone { get; set; }
        public string? Extension { get; set; }
        public string? Email { get; set; }
        public int? CourierId { get; set; }
        public string? AWBOrTracking { get; set; }
        public string? ExpectedTime { get; set; }
        public string? Comments { get; set; }
        public string? DropOffCity { get; set; }
        public string? SendToCountry { get; set; }
        public int? DestinationId { get; set; }
        public string? ServiceType { get; set; }
        public string? AccountNumber { get; set; }
        public string? BillTransportationTo { get; set; }
        public string? BillTransportationAcct { get; set; }
        public string? BillDutyTaxFeesTo { get; set; }
        public string? BillDutyTaxFeesAcct { get; set; }
        public string? CommodityDescription { get; set; }
        public string? ShipDate { get; set; }
        public bool? Active { get; set; }
        public string? ShipAlertEmail { get; set; }
        public decimal? UnitValue { get; set; }
        public string? LicenseType { get; set; }
        public int? CustomsTermsOfTradeId { get; set; }
        public bool? IsForwarder { get; set; }
    }
}



