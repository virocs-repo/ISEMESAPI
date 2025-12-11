using ISEMES.Models;

namespace ISEMES.Services
{
    public interface ICombinationLotService
    {
        Task<List<CombinationLot>> SearchCombinationLots(string fromDate, string toDate);
        Task<List<CustomerCombLot>> GetCustomerLotsCombine(int customerId, string lotNumber);
        Task<bool> UpsertCombineLot(UpsertCombineLotRequest request);
        Task<List<CombineLotsDto>> GetCustomerCombineLotsAsync(int comboLotId);
    }
}

