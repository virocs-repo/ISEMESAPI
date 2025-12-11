using ISEMES.Models;

namespace ISEMES.Services
{
    public interface IIntTranferReceivingService
    {
        Task<IEnumerable<IntTranferReceiving>> SearchInternalTransferReceivingAsync(DateTime? fromDate, DateTime? toDate, string? statusString, string? facilityId);
        Task<IEnumerable<InternalTransferLot>> GetInternalTransferLotAsync(int customerVendorID, int customerTypeID);
        Task<int> UpsertInternalTransferReceiptAsync(IntTransferReceiptReq request);
    }
}

