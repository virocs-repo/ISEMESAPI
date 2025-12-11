using ISEMES.Models;

namespace ISEMES.Repositories
{
    public interface IReceivingRepository
    {
        Task<List<ReceiptDetail>> GetReceiptDetailsAsync(int? receiptID, DateTime? fromDate, DateTime? toDate, string? receiptStatus, string? facilityIDStr);
        Task<List<DeviceDetails>> GetDeviceDetailsAsync(string? mailRoomNo, string? stagingLocation);
        Task<List<HardwareDetails>> GetHardwareDetailsAsync(int receiptId);
        Task<List<MiscellaneousGoods>> GetMiscellaneousGoodsAsync(int receiptId);
        Task<List<ReceiptEmployee>> GetReceiptEmployeeAsync(int receiptId);
        Task<List<DeviceTypes>> GetDeviceTypeByCustomer(int? customerId);
        Task<string> GetGeneratedLineItemNumber();
        Task UpdateReceiptDetailsAsync(ReceiptRequestDetails receiptRequest);
        Task UpdateDeviceAsync(DeviceDetailsRequest receiptRequest);
        Task UpdateHardwareAsync(HardwareDetailsRequest receiptRequest);
        Task UpdateMiscellaneousGoodsAsync(MiscellaneousGoodsDetailsRequest receiptRequest);
        Task VoidReceipt(int receiptId);
        Task UpsertAttachment(Attachment attachmentrequest);
        Task<List<Attachment>> ListReceiptAttachment(int receiptId);
        Task<List<Attachment>> ListAttachmentById(int Id, string? name = null);
        Task<List<InventoryLosts>> GetInterimLotsAsync();
        Task<List<InterimDevice>> GetInterimDeviceDataAsync(int interimReceiptID);
        Task<List<InterimDevice>> GetdetailsByInventoryIdAsync(int inventoryID);
        Task UpdateInterimDeviceAsync(InterimDeviceDetail interimDevice);
        Task<bool> CanUndoReceiveLotAsync(int inventoryID);
        Task<int> CheckingIsReceiptEditableAsync(int receiptId, int loginId);
        Task<List<ReceiptInternalDetail>> GetReceiverFormInternalAsync(string? status, bool? isExpected, DateTime? fromDate, DateTime? toDate);
        Task<List<CustomerReceiptDetail>> SearchCustomerReceiverForm(int? receivingInfoId, int? customerId, int? deviceFamilyId, int? deviceId, string? customerLotsStr, int? statusId, bool? isExpected, string? isElot, int? serviceCategoryId, int? locationId, string? mail, DateTime? fromDate, DateTime? toDate, string? receiptStatus, string? facilityIdStr);
        Task<List<InventoryReceiptStatus>> GetInventoryReceiptStatusesAsync();
        Task<List<DeviceFamily>> GetDeviceFamiliesAsync(int customerId);
        Task<List<DeviceFamily>> DeviceFamiliesAsync(int customerId);
        Task<List<Device>> GetDevicesAsync(int customerId, int deviceFamilyId);
        Task<List<ServiceCategory>> GetInventoryReceiptServiceCategoryAsync(string ListName);
        Task<List<LotOwners>> GetInventoryReceiptLotOwnersAsync();
        Task<List<TrayVendor>> GetInventoryReceiptTrayVendorAsync(int customerId);
        Task<List<TrayPart>> GetInventoryReceiptTraysByVendorIdAsync(int customerId, int deviceFamilyId);
        Task<List<PurchaseOrder>> GetPurchaseOrdersAsync(int customerId, int? divisionId, bool? isFreezed);
        Task<List<PackageCategory>> GetPackageCategoryAsync(string categoryName);
        Task<List<Quotes>> GetQuotesAsync(int customerId);
        Task<List<ServiceCaetgory>> GetServiceCaetgoryAsync();
        Task<(int ReturnCode, string ReturnMessage)> SaveMailRoomInfoAsync(int? mailId, int loginId, string mailJson, string packageLabelJson, string shipmentPaperJson);
        Task<(int ReturnCode, string ReturnMessage)> ValidateMailRoomInfoAsync(int? mailId, string mailJson);
        Task<(int ReturnCode, string ReturnMessage)> SaveInventoryReceiptAsync(int? customerId, int loginId, string receiptJson, string attachmentsJson);
        Task<List<MailRoomStatusList>> GetMailRoomStatusListAsync(string listName);
        Task<List<MailReceiptSearchResult>> SearchMailReceiptAsync(string? status, DateTime? fromDate, DateTime? toDate);
        Task<MailRoomDetails> GetMailRoomDetailsAsync(int mailId);
        Task<ReceiptFullDetailDto> GetReceiverFormInternalListAsync(int? receiptId, int? mailId);
        Task<List<ReceivingSearchResult>> SearchReceivingAsync(string? receivingTypes, DateTime? fromDate, DateTime? toDate, string? statusIds);
        Task SaveReceivingAsync(string jsonData, int receiptId, int loginId);
        Task<List<CustomersLogin>> GetCustomersLoginIdAsync(int loginId);
        Task<ReceivingDetails?> GetReceivingByIdAsync(int receiptId);
        Task<List<Interim>> GetSearchInterimCustomerIdAsync(int receiptId, int customerId);
    }
}

