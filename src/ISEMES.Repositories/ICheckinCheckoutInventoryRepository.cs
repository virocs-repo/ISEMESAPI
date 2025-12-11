using ISEMES.Models;

namespace ISEMES.Repositories
{
    public interface ICheckinCheckoutInventoryRepository
    {
        Task<List<InventoryCheckinCheckout>> GetAllInventoryCheckinCheckoutStatusAsync(DateTime? fromDate, DateTime? toDate);
        Task<IEnumerable<InventoryCheckinCheckoutLocation>> GetInventoryCheckinCheckoutLocationAsync();
        Task<IEnumerable<InventoryCheckinCheckoutStatuses>> GetInventoryCheckinCheckoutStatusAsync();
        Task<IEnumerable<InventoryCheckinCheckoutItem>> GetInventoryCheckinCheckoutAsync(string lotNumber);
        Task UpsertInventoryCheckinCheckoutStatusAsync(InventoryCheckinCheckoutRequest request);
        Task<IEnumerable<CheckinCheckoutStatus>> GetCheckinCheckoutAsync(string lotNumber);
    }
}

