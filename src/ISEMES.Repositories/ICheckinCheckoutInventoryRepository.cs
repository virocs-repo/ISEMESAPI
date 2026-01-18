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
        Task<EmployeeDetails?> ValidateBadge(string badge);
        Task<List<CheckInCheckOutLotDetails>> GetCheckInCheckOutLotDetailsAsync(string? lotNumber, int employeeId, int customerLoginId, string requestType, int? count);
        Task<List<CheckInCheckOutLotDetails>> GetLastTenCheckOutLotDetailsAsync();
        Task<bool> SaveCheckInCheckOutRequest(string inputData, string requestType);
        Task<List<CheckInCheckOutLotDetails>> GetLotsPendingApprovalAsync(string requestType, int requestId);
        Task<List<CheckInCheckOutLocation>> GetCheckInLocationsAsync(string requestType, int requestId);
        Task<bool> ApproveCheckInCheckOut(string requestType, string inputData);
    }
}



