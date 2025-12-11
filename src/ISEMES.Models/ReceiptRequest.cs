using System.Text.Json.Serialization;

namespace ISEMES.Models
{
    public class ReceiptRequest
    {
        public int? ReceiptID { get; set; }
        public int? VendorID { get; set; }
        public string? VendorName { get; set; }
        public int? CustomerTypeID { get; set; }
        public int? CustomerVendorID { get; set; }
        public int? BehalfID { get; set; }
        public int? ReceivingFacilityID { get; set; }
        public int? DeliveryModeID { get; set; }
        public int? CourierDetailID { get; set; }
        public int? CountryFromID { get; set; }
        public string? ContactPerson { get; set; }
        public string? ContactPhone { get; set; }
        public string? Email { get; set; }
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime ExpectedDateTime { get; set; }
        public string? TrackingNumber { get; set; }        
        public int? AddressID { get; set; }
        public string? MailComments { get; set; }
        public string? PMComments { get; set; }
        public int? NoOfCartons { get; set; }
        public bool IsHold { get; set; }
        public string? HoldComments { get; set; }
        public bool IsExpected { get; set; }
        public bool IsInterim { get; set; }
        public bool IsFTZ { get; set; }
        public string? MailStatus { get; set; }
        public string? ReceivingStatus { get; set; }
        public string? SignaturePersonType { get; set; }
        public int? SignaturePersonID { get; set; }
        public string? Signaturebase64Data { get; set; }
        public string? Signature { get; set; }
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime SignatureDate { get; set; }
        public string? RecordStatus { get; set; }
        public bool Active { get; set; }
        public int LoginId { get; set; }
        public string? SignaturePerson { get; set; }
        public List<Employee> EmployeeDetail { get; set; }
    }

    public class ReceiptRequestDetails
    {
        public List<ReceiptRequest> ReceiptDetails { get; set; }
    }
}

