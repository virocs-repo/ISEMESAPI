using ISEMES.Models;
using ISEMES.Repositories;

namespace ISEMES.Services
{
    public class CheckinCheckoutInventoryService : ICheckinCheckoutInventoryService
    {
        private readonly ICheckinCheckoutInventoryRepository _repository;

        public CheckinCheckoutInventoryService(ICheckinCheckoutInventoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<InventoryCheckinCheckout>> GetAllInventoryCheckinCheckoutStatusAsync(DateTime? fromDate, DateTime? toDate)
        {
            return await _repository.GetAllInventoryCheckinCheckoutStatusAsync(fromDate, toDate);
        }

        public async Task<IEnumerable<InventoryCheckinCheckoutLocation>> GetInventoryCheckinCheckoutLocationAsync()
        {
            return await _repository.GetInventoryCheckinCheckoutLocationAsync();
        }

        public async Task<IEnumerable<InventoryCheckinCheckoutStatuses>> GetInventoryCheckinCheckoutStatusAsync()
        {
            return await _repository.GetInventoryCheckinCheckoutStatusAsync();
        }

        public async Task<IEnumerable<InventoryCheckinCheckoutItem>> GetInventoryCheckinCheckoutAsync(string lotNumber)
        {
            return await _repository.GetInventoryCheckinCheckoutAsync(lotNumber);
        }

        public async Task UpsertInventoryCheckinCheckoutStatusAsync(InventoryCheckinCheckoutRequest request)
        {
            await _repository.UpsertInventoryCheckinCheckoutStatusAsync(request);
        }

        public async Task<IEnumerable<CheckinCheckoutStatus>> GetCheckinCheckoutAsync(string lotNumber)
        {
            return await _repository.GetCheckinCheckoutAsync(lotNumber);
        }
    }
}

