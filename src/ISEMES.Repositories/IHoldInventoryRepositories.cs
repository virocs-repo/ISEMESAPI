using ISEMES.Models;

namespace ISEMES.Repositories
{
    public interface IHoldInventoryRepositories
    {
        Task<List<InventoryHolds>> GetAllSearchHoldAsync(DateTime? fromDate, DateTime? toDate);
        Task<IEnumerable<HoldTypeResponse>> GetHoldTypesAsync(int? inventoryId);
        Task<IEnumerable<Hold>> GetHoldCodesAsync(int? inventoryId);
        Task<IEnumerable<InventoryHold>> GetAllHoldsAsync(int? inventoryId);
        Task<int> UpsertHoldAsync(HoldRequest holdRequest);
        Task<IEnumerable<HoldDetails>> GetHoldDetailsAsync(int? inventoryId);
        Task<List<HoldComment>> GetHoldCommentsAsync();
        Task<IEnumerable<HoldCustomerDetails>> GetCustomerDetailsAsync(int? inventoryId);
        Task<IEnumerable<OperaterAttachements>> GetOperaterAttachmentsAsync(int? TFSHoldId);
    }
}



