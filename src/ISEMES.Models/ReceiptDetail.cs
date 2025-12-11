using System.ComponentModel.DataAnnotations;

namespace ISEMES.Models
{
    public class ReceiptDetail
    {
        [Key]
        public int ReceiptID { get; set; }
        public int? CustomerTypeID { get; set; }
        public string? CustomerType { get; set; }
        public string? TrackingNumber { get; set; }
        public int? CustomerVendorID { get; set; }
        public string? CustomerVendor { get; set; }
        public int? BehalfID { get; set; }
        public string? ReceivedOnBehalf { get; set; }
        public int? ReceivingFacilityID { get; set; }
        public string? ReceivingFacility { get; set; }
        public int? DeliveryModeID { get; set; }
        public string? DeliveryMode { get; set; }
        public int? CourierDetailID { get; set; }
        public string? CourierName { get; set; }
        public int? CountryFromID { get; set; }
        public string? CountryFrom { get; set; }
        public string? ContactPerson { get; set; }
        public string? ContactPhone { get; set; }
        public string? Email { get; set; }
        public int? AddressID { get; set; }
        public DateTime? ExpectedDateTime { get; set; }
        public string? Address { get; set; }
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
        public string? ReceiptType { get; set; }
        public string? SignaturePersonType { get; set; }
        public int? SignaturePersonID { get; set; }
        public string? SignaturePerson { get; set; }
        public string? Signature { get; set; }
        public string? Signaturebase64Data { get; set; }
        public int? PMReceiverID { get; set; }
        public DateTime? SignatureDate { get; set; }
        public bool Active { get; set; }
        public string? IseLots { get; set; }
        public string? CustomerLots { get; set; }
        public string Modifiedby { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }

    public class FilePath
    {
        public string Filepath { get; set; }
    }

    public class ReceiptInternalDetail
    {
        [Key]
        public int? ReceiptID { get; set; }
        public string? ReceivingNo { get; set; }
        public string? MailNo { get; set; }
        public string? CustomerName { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? Recipient { get; set; }
        public string? DeliveryMethod { get; set; }
        public DateTime? ExpectedDate { get; set; }
        public int? NoofPackages { get; set; }
        public string? ReceivingStatus { get; set; }
        public string? MailStatus { get; set; }
        public string SenderName { get; set; }
        public string AWB { get; set; }
        public string Location { get; set; }
        public string CreatedBy { get; set; }
        public string Device { get; set; }
        public string ISELotNumbers { get; set; }
        public string CustomerLotNumbers { get; set; }
        public string ExpectedUnexpected { get; set; }
    }

    public class CustomerReceiptDetail
    {
        [Key]
        public int ReceiptID { get; set; }
        public int? CustomerTypeID { get; set; }
        public string? CustomerType { get; set; }
        public int? CustomerVendorID { get; set; }
        public string? CustomerVendor { get; set; }
        public int? BehalfID { get; set; }
        public string? ReceivedOnBehalf { get; set; }
        public int? ReceivingFacilityID { get; set; }
        public string? ReceivingFacility { get; set; }
        public int? DeliveryModeID { get; set; }
        public string? DeliveryMode { get; set; }
        public int? CourierDetailID { get; set; }
        public string? CourierName { get; set; }
        public int? CountryFromID { get; set; }
        public string? TrackingNumber { get; set; }
        public string? CountryFrom { get; set; }
        public string? ContactPerson { get; set; }
        public string? ContactPhone { get; set; }
        public string? Email { get; set; }
        public int? AddressID { get; set; }
        public DateTime? ExpectedDateTime { get; set; }
        public string? Address { get; set; }
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
        public string? SignaturePerson { get; set; }
        public string? Signature { get; set; }
        public string? Signaturebase64Data { get; set; }
        public DateTime? SignatureDate { get; set; }
        public bool Active { get; set; }
        public int ReceiptStatusID { get; set; }
        public string ReceiptStatus { get; set; }
        public string? ReceiptType { get; set; }
        public int PMReceiverID { get; set; }
        public string IseLots { get; set; }
        public string CustomerLots { get; set; }
        public string Modifiedby { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string? ReceivingInfo { get; set; }
        public string? Mail { get; set; }
        public string Sender { get; set; }
        public string SenderFrom { get; set; }
        public string SubCategory { get; set; }
        public string TravelerStatus { get; set; }
        public string? Recipient { get; set; }
        public string? Location { get; set; }
    }

    public class MailReceiptSearchResult
    {
        public int? MailId { get; set; }
        public string? Customer { get; set; }
        public string? MailNo { get; set; }
        public string? ReceivingNo { get; set; }
        public DateTime? CreateDateTime { get; set; }
        public string? Recipient { get; set; }
        public string? DeliveryMethod { get; set; }
        public DateTime? ExpectedDate { get; set; }
        public int? NoOfPackages { get; set; }
        public string? Status { get; set; }
        public string? Received { get; set; }
        public string? Damage { get; set; }
        public string? PartialDelivery { get; set; }
        public string? Sendor { get; set; }
        public string? AWB { get; set; }
        public string? TrackingNumber { get; set; }
        public string? Location { get; set; }
        public string? CreatedBy { get; set; }
        public string? Lot { get; set; }
        public string? CustomerLot { get; set; }
        public bool? CanEdit { get; set; }
    }

    public class InventoryReceiptStatus
    {
        public int MasterListItemId { get; set; }
        public int MasterListId { get; set; }
        public string ItemText { get; set; }
        public string ItemValue { get; set; }
    }

    public class DeviceFamily
    {
        public int DeviceFamilyId { get; set; }
        public string DeviceFamilyName { get; set; }
    }

    public class Device
    {
        public int DeviceId { get; set; }
        public string DeviceName { get; set; }
    }

    public class ServiceCategory
    {
        public int ServiceCategoryId { get; set; }
        public string ServiceCategoryName { get; set; }
    }

    public class LotOwners
    {
        public int employeeID { get; set; }
        public string employeeName { get; set; }
    }

    public class TrayVendor
    {
        public int TrayVendorId { get; set; }
        public string VendorName { get; set; }
    }

    public class TrayPart
    {
        public int TrayPartId { get; set; }
        public string TrayNumber { get; set; }
    }

    public class PurchaseOrder
    {
        public int PurchaseOrderId { get; set; }
        public string CustomerPoNumber { get; set; }
    }

    public class PackageCategory
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
    }

    public class MailRoomStatusList
    {
        public int MasterListItemId { get; set; }
        public string ItemText { get; set; }
    }

    public class Quotes
    {
        public string QuoteId { get; set; }
        public string Quote { get; set; }
    }

    public class ServiceCaetgory
    {
        public int ServiceCategoryId { get; set; }
        public string ServiceCategoryName { get; set; }
    }

    public class InventoryReceiptRequest
    {
        public string ReceiptJson { get; set; }
    }

    public class MailRoomDetails
    {
        public int MailId { get; set; }
        public int? CustomerTypeId { get; set; }
        public int? CustomerVendorId { get; set; }
        public int? BehalfId { get; set; }
        public string? AWBMailCode { get; set; }
        public string? ScanLocation { get; set; }
        public int? LocationId { get; set; }
        public int? RecipientId { get; set; }
        public bool? Received { get; set; }
        public int? SendorId { get; set; }
        public bool? PartialDelivery { get; set; }
        public bool? IsDamage { get; set; }
        public bool? IsHold { get; set; }
        public int? DeliveryMethodId { get; set; }
        public string? ContactPerson { get; set; }
        public string? Email { get; set; }
        public int? CourierId { get; set; }
        public int? SendFromCountryId { get; set; }
        public string? TrackingNumber { get; set; }
        public DateTime? ExpectedDateTime { get; set; }
        public int? AddressId { get; set; }
        public string? MailComments { get; set; }
        public int? NoofPackages { get; set; }
        public string? PackageCategory { get; set; }
        public int? POId { get; set; }
        public string? Signiture { get; set; }
        public string? Phone { get; set; }
        public int? StatusId { get; set; }
        public bool? Active { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public string? HoldComments { get; set; }
        public List<MailRoomOtherDetails> Others { get; set; } = new();
        public List<Attachment> MailAttachments { get; set; } = new();
    }

    public class MailRoomOtherDetails
    {
        public int OtherId { get; set; }
        public int MailId { get; set; }
        public int? TypeId { get; set; }
        public string Details { get; set; }
        public int? Qty { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }
    }

    public class InternalReceiptDetail
    {
        public int ReceiptId { get; set; }
        public bool? IsInterim { get; set; }
        public int? CustomerTypeID { get; set; }
        public int? CustomerVendorID { get; set; }
        public int? BehalfID { get; set; }
        public string? Recipient { get; set; }
        public string? Sendor { get; set; }
        public int? ReceivingFacilityID { get; set; }
        public int? DeliveryMethodID { get; set; }
        public string? ContactPerson { get; set; }
        public string? Email { get; set; }
        public int? CourierID { get; set; }
        public int? CountryFromID { get; set; }
        public string? TrackingNumber { get; set; }
        public DateTime? ExpectedDateTime { get; set; }
        public int? AddressID { get; set; }
        public string? ContactPhone { get; set; }
        public string? ReceivingInstructions { get; set; }
        public string? Notes { get; set; }
        public int? NoofPackages { get; set; }
        public string? PackageCategory { get; set; }
        public string? Quotes { get; set; }
        public int? POId { get; set; }
        public int? LotCategoryId { get; set; }
        public bool? Active { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool? CanEditHeader { get; set; }
        public string? InterimShippingIds { get; set; }
    }

    public class InternalDevice
    {
        public int? DeviceId { get; set; }
        public int? ReceiptId { get; set; }
        public string? ISELotNumber { get; set; }
        public string? CustomerLotNumber { get; set; }
        public int? CustomerCount { get; set; }
        public int? DeviceTypeID { get; set; }
        public string? DateCode { get; set; }
        public string? COO { get; set; }
        public bool? Expedite { get; set; }
        public string? IQA { get; set; }
        public int? LotOwnerID { get; set; }
        public int? LotCategoryId { get; set; }
        public bool? Active { get; set; }
        public bool? IsHold { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool? CanEdit { get; set; }
    }

    public class InternalTray
    {
        public int? Id { get; set; }
        public int? ReceiptId { get; set; }
        public int? TrayVendorId { get; set; }
        public int? TrayPartId { get; set; }
        public int? Qty { get; set; }
        public bool? Active { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }

    public class InternalHardware
    {
        public int? Id { get; set; }
        public int? ReceiptId { get; set; }
        public int? HardwareTypeId { get; set; }
        public string? DeviceName { get; set; }
        public string? HardwareId { get; set; }
        public int? Qty { get; set; }
        public bool? Active { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }

    public class InternalOther
    {
        public int? OtherId { get; set; }
        public int? ReceiptId { get; set; }
        public int? TypeId { get; set; }
        public string? Details { get; set; }
        public int? Qty { get; set; }
        public bool? Active { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }

    public class ReceiptFullDetailDto
    {
        public InternalReceiptDetail Receipt { get; set; }
        public List<InternalDevice> Devices { get; set; }
        public List<InternalTray> Trays { get; set; }
        public List<InternalHardware> Hardware { get; set; }
        public List<InternalOther> Others { get; set; }
        public List<Attachment> ReceiptAttachments { get; set; }
    }

    public class ReceivingSearchResult
    {
        public int? ReceiptId { get; set; }
        public string? ReceivingNo { get; set; }
        public string? MailNo { get; set; }
        public string? CustomerName { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? Recipient { get; set; }
        public string? DeliveryMethod { get; set; }
        public DateTime? ExpectedDate { get; set; }
        public int? NoofPackages { get; set; }
        public string? ReceivingStatus { get; set; }
        public string? MailStatus { get; set; }
        public string? SenderName { get; set; }
        public string? AWB { get; set; }
        public string? Location { get; set; }
        public string? CreatedBy { get; set; }
        public string? Device { get; set; }
        public string? ISELotNumbers { get; set; }
        public string? CustomerLotNumbers { get; set; }
        public string? ExpectedUnexpected { get; set; }
        public int? StatusId { get; set; }
    }

    public class ReceivingRequest
    {
        public string Jsondata { get; set; }
        public int ReceiptId { get; set; }
        public int LoginId { get; set; }
    }

    public class CustomersLogin
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
    }

    public class ReceivingDetails
    {
        public string? MailRoomNo { get; set; }
        public string? StagingLocation { get; set; }
        public List<DeviceItem> Devices { get; set; } = new();
    }

    public class DeviceItem
    {
        public int DeviceID { get; set; }
        public int InventoryID { get; set; }
        public int ReceiptID { get; set; }
        public string? ISELotNumber { get; set; }
        public string? LotId { get; set; }
        public string? CustomerLotNumber { get; set; }
        public int? CustomerCount { get; set; }
        public bool? Expedite { get; set; }
        public string? IQA { get; set; }
        public int? LotOwnerID { get; set; }
        public string? LotOwner { get; set; }
        public int? LabelCount { get; set; }
        public int? COOID { get; set; }
        public int? DeviceTypeID { get; set; }
        public string? DeviceType { get; set; }
        public int? LotCategoryID { get; set; }
        public string? LotCategory { get; set; }
        public string? COO { get; set; }
        public string? DateCode { get; set; }
        public bool? IsHold { get; set; }
        public string? HoldComments { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool? Active { get; set; }
        public string? LotIdentifier { get; set; }
        public bool? IsReceived { get; set; }
        public bool? CanEdit { get; set; }
    }

    public class Interim
    {
        public int? LotId { get; set; }
        public int? InterimShippingId { get; set; }
        public string? LotNumber { get; set; }
        public string? CustomerLotNumber { get; set; }
        public int? CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
}

