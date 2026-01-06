namespace ISEMES.Models
{
    public class ShippingDeliveryInfo
    {
        public int DeliveryInfoId { get; set; }
        public int? ShippingMethodId { get; set; }
        public string ShippingMethod { get; set; }
        public string ContactPerson { get; set; }
        public int? CustomerAddressId { get; set; }
        public string Address { get; set; }
        public string CompanyName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Country { get; set; }
        public string PostCode { get; set; }
        public string StateProvince { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
        public string Ext { get; set; }
        public string Email { get; set; }
        public string Sender { get; set; }
        public int? DestinationId { get; set; }
        public string Destination { get; set; }
        public int? CourierId { get; set; }
        public string Courier { get; set; }
        public string TrackingCode { get; set; }
        public DateTime? ExpectedTime { get; set; }
        public string ShippingComments { get; set; }
        public string DropOffCity { get; set; }
        public string SendToCountry { get; set; }
        public string SendToCountryName { get; set; }
        public string ServiceType { get; set; }
        public string AccountNumber { get; set; }
        public string BillTransportationTo { get; set; }
        public string BillTransportationAcct { get; set; }
        public string BillDutyTaxFeesTo { get; set; }
        public string BillDutyTaxFeesAcct { get; set; }
        public string CountryOfOrigin { get; set; }
        public string CommodityDescription { get; set; }
        public string CommodityOrigin { get; set; }
        public string Description { get; set; }
        public string CustomerReference { get; set; }
        public string ECCN { get; set; }
        public string InvoiceNumber { get; set; }
        public string LicenseType { get; set; }
        public string PurchaseNumber { get; set; }
        public string ReferenceNumber1 { get; set; }
        public string ReferenceNumber2 { get; set; }
        public int? OtherAccountNumber { get; set; }
        public long? ScheduleBNumber { get; set; }
        public long? ScheduleBUnits1 { get; set; }
        public DateTime? ShipDate { get; set; }
        public string ShipmentReference { get; set; }
        public int? Qty { get; set; }
        public int? Units { get; set; }
        public decimal? UnitValue { get; set; }
        public decimal? TotalValue { get; set; }
        public decimal? Value { get; set; }
        public string ReceiverName { get; set; }
        public string TaxId { get; set; }
        public bool? Residential { get; set; }
        public decimal? Weight { get; set; }
        public string OtherAccNo { get; set; }
        public string PackageDimentions { get; set; }
        public string? NoOfPackages { get; set; }
        public string PackageType { get; set; }
        public string Attention { get; set; }
        public string IsDomestic { get; set; }
        public string PackingSlipNumber { get; set; }
        public string ShipAlertEmail { get; set; }
        public int? CIFromId { get; set; }
        public string CIFrom { get; set; }
        public string BillToContactPerson { get; set; }
        public string BillToCompanyName { get; set; }
        public string BillToAddress1 { get; set; }
        public string BillToAddress2 { get; set; }
        public string BillToAddress3 { get; set; }
        public string BillToCountry { get; set; }
        public string BillToPostCode { get; set; }
        public string BillToStateProvince { get; set; }
        public string BillToCity { get; set; }
        public string BillToPhone { get; set; }
        public string BillToExt { get; set; }
        public decimal? CIWeight { get; set; }
        public string CIPackageDimentions { get; set; }
        public int? CINoOfPackages { get; set; }
        public int? CustomerOtherAddressId { get; set; }
        public string OtherCountry { get; set; }
        public string OtherCompanyName { get; set; }
        public string OtherAddress1 { get; set; }
        public string OtherAddress2 { get; set; }
        public string OtherAddress3 { get; set; }
        public string OtherPostCode { get; set; }
        public string OtherStateProvince { get; set; }
        public string OtherCity { get; set; }
        public string OtherExt { get; set; }
        public string OtherContactPerson { get; set; }
        public string OtherPhone { get; set; }
        public string SpecialInstructionforShipping { get; set; }
        public string CommentsforPackingSlip { get; set; }
        public string CommentsforCommericalInvoice { get; set; }
        public int? CustomsTermsOfTradeId { get; set; }
        public string CustomsTermsOfTrade { get; set; }
        public bool? IsForwarder { get; set; }
        public DateTime? DropOffExpectedTime { get; set; }
        public bool? BillCheck { get; set; }
        public int? CustomerBillTOAddressId { get; set; }
        public string UltimateConsignee { get; set; }
    }

    public class PackageDimension
    {
        public int MasterListItemId { get; set; }
        public string CIPackageDimentions { get; set; }
    }
}



