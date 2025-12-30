using ISEMES.Models;
using ISEMES.Repositories;

namespace ISEMES.Services
{
    public class ReceivingService : IReceivingService
    {
        private readonly IReceivingRepository _repository;

        public ReceivingService(IReceivingRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<ReceiptDetail>> GetReceiptDetailsAsync(int? receiptID, DateTime? fromDate, DateTime? toDate, string? receiptStatus, string? facilityIDStr)
        {
            return await _repository.GetReceiptDetailsAsync(receiptID, fromDate, toDate, receiptStatus, facilityIDStr);
        }

        public async Task<List<DeviceDetails>> GetDeviceDetailsAsync(string? mailRoomNo, string? stagingLocation)
        {
            return await _repository.GetDeviceDetailsAsync(mailRoomNo, stagingLocation);
        }

        public async Task<List<HardwareDetails>> GetHardwareDetailsAsync(int receiptId)
        {
            return await _repository.GetHardwareDetailsAsync(receiptId);
        }

        public async Task UpdateReceiptDetailsAsync(ReceiptRequestDetails request)
        {
            await _repository.UpdateReceiptDetailsAsync(request);
        }

        public async Task UpdateDeviceAsync(DeviceDetailsRequest request)
        {
            await _repository.UpdateDeviceAsync(request);
        }

        public async Task UpdateHardwareAsync(HardwareDetailsRequest request)
        {
            await _repository.UpdateHardwareAsync(request);
        }

        public async Task<List<MiscellaneousGoods>> GetMiscellaneousGoodsAsync(int receiptId)
        {
            return await _repository.GetMiscellaneousGoodsAsync(receiptId);
        }

        public async Task UpdateMiscellaneousGoodsAsync(MiscellaneousGoodsDetailsRequest request)
        {
            await _repository.UpdateMiscellaneousGoodsAsync(request);
        }

        public async Task<List<ReceiptEmployee>> GetReceiptEmployeeAsync(int receiptId)
        {
            return await _repository.GetReceiptEmployeeAsync(receiptId);
        }

        public async Task VoidReceipt(int receiptId)
        {
            await _repository.VoidReceipt(receiptId);
        }

        public async Task<List<DeviceTypes>> GetDeviceTypeByCustomer(int? customerId)
        {
            return await _repository.GetDeviceTypeByCustomer(customerId);
        }

        public async Task<string> GetGeneratedLineItemnumber()
        {
            return await _repository.GetGeneratedLineItemNumber();
        }

        public async Task UpsertAttachment(Attachment attachmentrequest)
        {
            await _repository.UpsertAttachment(attachmentrequest);
        }

        public async Task<List<Attachment>> ListReceiptAttachment(int receiptId)
        {
            return await _repository.ListReceiptAttachment(receiptId);
        }

        public async Task<List<Attachment>> ListAttachmentById(int Id, string? name = null)
        {
            return await _repository.ListAttachmentById(Id, name);
        }

        public async Task<List<InventoryLosts>> GetInterimLotsAsync()
        {
            return await _repository.GetInterimLotsAsync();
        }

        public async Task<List<InterimDevice>> GetInterimDeviceDataAsync(int interimReceiptID)
        {
            return await _repository.GetInterimDeviceDataAsync(interimReceiptID);
        }

        public async Task<List<InterimDevice>> GetdetailsByInventoryIdAsync(int inventoryID)
        {
            return await _repository.GetdetailsByInventoryIdAsync(inventoryID);
        }

        public async Task UpdateInterimDeviceAsync(InterimDeviceDetail interimDevice)
        {
            await _repository.UpdateInterimDeviceAsync(interimDevice);
        }

        public async Task<bool> CanUndoReceiveLotAsync(int inventoryID)
        {
            return await _repository.CanUndoReceiveLotAsync(inventoryID);
        }

        public async Task<int> CheckingIsReceiptEditableAsync(int receiptId, int logId)
        {
            return await _repository.CheckingIsReceiptEditableAsync(receiptId, logId);
        }

        public async Task<List<ReceiptInternalDetail>> GetReceiverFormInternalAsync(string? status, bool? isExpected, DateTime? fromDate, DateTime? toDate)
        {
            return await _repository.GetReceiverFormInternalAsync(status, isExpected, fromDate, toDate);
        }

        public async Task<List<CustomerReceiptDetail>> SearchCustomerReceiverForm(int? receivingInfoId, int? customerId, int? deviceFamilyId, int? deviceId, string customerLotsStr, int? statusId, bool? isExpected, string isElot, int? serviceCategoryId, int? locationId, string mail, DateTime? fromDate, DateTime? toDate, string receiptStatus, string facilityIdStr)
        {
            return await _repository.SearchCustomerReceiverForm(receivingInfoId, customerId, deviceFamilyId, deviceId, customerLotsStr, statusId, isExpected, isElot, serviceCategoryId, locationId, mail, fromDate, toDate, receiptStatus, facilityIdStr);
        }

        public async Task<List<InventoryReceiptStatus>> GetInventoryReceiptStatusesAsync()
        {
            return await _repository.GetInventoryReceiptStatusesAsync();
        }

        public async Task<List<DeviceFamily>> GetDeviceFamiliesAsync(int customerId)
        {
            return await _repository.GetDeviceFamiliesAsync(customerId);
        }

        public async Task<List<DeviceFamily>> DeviceFamiliesAsync(int customerId)
        {
            return await _repository.DeviceFamiliesAsync(customerId);
        }

        public async Task<List<Device>> GetDevicesAsync(int customerId, int deviceFamilyId)
        {
            return await _repository.GetDevicesAsync(customerId, deviceFamilyId);
        }

        public async Task<List<ServiceCategory>> GetInventoryReceiptServiceCategoryAsync(string ListName)
        {
            return await _repository.GetInventoryReceiptServiceCategoryAsync(ListName);
        }

        public async Task<List<LotOwners>> GetInventoryReceiptLotOwnersAsync()
        {
            return await _repository.GetInventoryReceiptLotOwnersAsync();
        }

        public async Task<List<TrayVendor>> GetInventoryReceiptTrayVendorAsync(int customerId)
        {
            return await _repository.GetInventoryReceiptTrayVendorAsync(customerId);
        }

        public async Task<List<TrayPart>> GetInventoryReceiptTraysByVendorIdAsync(int customerId, int vendorId)
        {
            return await _repository.GetInventoryReceiptTraysByVendorIdAsync(customerId, vendorId);
        }

        public async Task<List<PurchaseOrder>> GetPurchaseOrdersAsync(int customerId, int? divisionId, bool? isFreezed)
        {
            return await _repository.GetPurchaseOrdersAsync(customerId, divisionId, isFreezed);
        }

        public async Task<List<PackageCategory>> GetPackageCategoryAsync(string categoryName)
        {
            return await _repository.GetPackageCategoryAsync(categoryName);
        }

        public async Task<List<Quotes>> GetQuotesAsync(int customerId)
        {
            return await _repository.GetQuotesAsync(customerId);
        }

        public async Task<List<ServiceCaetgory>> GetServiceCaetgoryAsync()
        {
            return await _repository.GetServiceCaetgoryAsync();
        }

        public async Task<(int ReturnCode, string ReturnMessage)> SaveMailRoomInfoAsync(int? mailId, int loginId, string mailJson, string packageLabelJson, string shipmentPaperJson)
        {
            return await _repository.SaveMailRoomInfoAsync(mailId, loginId, mailJson, packageLabelJson, shipmentPaperJson);
        }

        public async Task<(int ReturnCode, string ReturnMessage)> ValidateMailRoomInfoAsync(int? mailId, string mailJson)
        {
            return await _repository.ValidateMailRoomInfoAsync(mailId, mailJson);
        }

        public async Task<(int ReturnCode, string ReturnMessage)> SaveInventoryReceiptAsync(int? receiptId, int loginId, string receiptJson, string attachmentsJson)
        {
            return await _repository.SaveInventoryReceiptAsync(receiptId, loginId, receiptJson, attachmentsJson);
        }

        public async Task<List<MailRoomStatusList>> GetMailRoomStatusListAsync(string listName)
        {
            return await _repository.GetMailRoomStatusListAsync(listName);
        }

        public async Task<List<MailReceiptSearchResult>> SearchMailReceiptAsync(string? status, DateTime? fromDate, DateTime? toDate)
        {
            return await _repository.SearchMailReceiptAsync(status, fromDate, toDate);
        }

        public async Task<MailRoomDetails> GetMailRoomDetailsAsync(int mailId)
        {
            return await _repository.GetMailRoomDetailsAsync(mailId);
        }

        public async Task<ReceiptFullDetailDto> GetReceiverFormInternalListAsync(int? receiptId, int? mailId)
        {
            return await _repository.GetReceiverFormInternalListAsync(receiptId, mailId);
        }

        public async Task<List<ReceivingSearchResult>> SearchReceivingAsync(string? receivingTypes, DateTime? fromDate, DateTime? toDate, string? statusIds)
        {
            return await _repository.SearchReceivingAsync(receivingTypes, fromDate, toDate, statusIds);
        }

        public async Task SaveReceivingAsync(string jsonData, int receiptId, int loginId)
        {
            await _repository.SaveReceivingAsync(jsonData, receiptId, loginId);
        }

        public async Task<List<CustomersLogin>> GetCustomersLoginIdAsync(int loginId)
        {
            return await _repository.GetCustomersLoginIdAsync(loginId);
        }

        public async Task<ReceivingDetails?> GetReceivingByIdAsync(int receiptId)
        {
            return await _repository.GetReceivingByIdAsync(receiptId);
        }

        public async Task<List<Interim>> GetSearchInterimCustomerIdAsync(int receiptId, int customerId)
        {
            return await _repository.GetSearchInterimCustomerIdAsync(receiptId, customerId);
        }
    }
}

