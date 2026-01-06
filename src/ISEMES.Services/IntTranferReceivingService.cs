using ISEMES.Models;
using ISEMES.Repositories;

namespace ISEMES.Services
{
    public class IntTranferReceivingService : IIntTranferReceivingService
    {
        private readonly IIntTranferReceivingRepository _inventoryRepository;

        public IntTranferReceivingService(IIntTranferReceivingRepository inventoryReportRepository)
        {
            _inventoryRepository = inventoryReportRepository;
        }

        public async Task<IEnumerable<IntTranferReceiving>> SearchInternalTransferReceivingAsync(DateTime? fromDate, DateTime? toDate, string? statusString, string? facilityId)
        {
            return await _inventoryRepository.SearchInternalTransferReceivingAsync(fromDate, toDate, statusString, facilityId);
        }

        public async Task<IEnumerable<InternalTransferLot>> GetInternalTransferLotAsync(int customerVendorID, int customerTypeID)
        {
            return await _inventoryRepository.GetInternalTransferLotAsync(customerVendorID, customerTypeID);
        }

        public async Task<int> UpsertInternalTransferReceiptAsync(IntTransferReceiptReq request)
        {
            return await _inventoryRepository.UpsertInternalTransferReceiptAsync(request);
        }
    }
}



