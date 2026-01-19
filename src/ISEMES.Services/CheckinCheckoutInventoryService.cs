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
        public async Task<EmployeeDetails?> ValidateBadge(string badge)
        {
            return await _repository.ValidateBadge(badge);
        }

        public async Task<List<CheckInCheckOutLotDetails>> GetCheckInCheckOutLotDetailsAsync(string lotNumber, int employeeId, int customerLoginId, string requestType, int? count)
        {
            return await _repository.GetCheckInCheckOutLotDetailsAsync(lotNumber, employeeId, customerLoginId, requestType, count);
        }
        public async Task<List<CheckInCheckOutLotDetails>> GetLastTenCheckOutLotDetailsAsync()
        {
            return await _repository.GetLastTenCheckOutLotDetailsAsync();
        }
        public async Task<bool> SaveCheckInCheckOutRequest(string inputData, string requestType)
        {
            return await _repository.SaveCheckInCheckOutRequest(inputData, requestType);
        }
        public async Task<List<CheckInCheckOutLotDetails>> GetLotsPendingApprovalAsync(string requestType, int requestId)
        {
            return await _repository.GetLotsPendingApprovalAsync(requestType, requestId);
        }
        public async Task<List<CheckInCheckOutLocation>> GetCheckInLocationsAsync(string requestType, int requestId)
        {
            return await _repository.GetCheckInLocationsAsync(requestType, requestId);
        }
        public async Task<bool> ApproveCheckInCheckOut(string requestType, string inputData)
        {
            return await _repository.ApproveCheckInCheckOut(requestType, inputData);
        }
    }
}



