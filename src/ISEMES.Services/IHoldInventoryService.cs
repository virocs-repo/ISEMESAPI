using ISEMES.Models;

namespace ISEMES.Services
{
    public interface IHoldInventoryService
    {
        Task<List<InventoryHolds>> GetAllSearchHoldAsync(DateTime? fromDate, DateTime? toDate);
        Task<IEnumerable<HoldTypeResponse>> GetHoldTypesAsync(int? inventoryId);
        Task<IEnumerable<HoldGroup>> GetHoldCodesAsync(int? inventoryId, int? holdTypeId);
        Task<IEnumerable<InventoryHold>> GetAllHoldsAsync(int? inventoryId);
        Task<int> UpsertHoldAsync(HoldRequest holdRequest);
        Task<IEnumerable<HoldDetails>> GetHoldDetailsAsync(int? inventoryId);
        Task<List<HoldComment>> GetHoldCommentsAsync();
        Task<IEnumerable<HoldCustomerDetails>> GetCustomerDetailsAsync(int? inventoryId);
        Task<IEnumerable<OperaterAttachements>> GetOperaterAttachmentsAsync(int? TFSHoldId);
    }
}

