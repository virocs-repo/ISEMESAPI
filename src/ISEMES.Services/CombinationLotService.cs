using ISEMES.Models;
using ISEMES.Repositories;

namespace ISEMES.Services
{
    public class CombinationLotService : ICombinationLotService
    {
        private readonly ICombinationLotRepository _repository;

        public CombinationLotService(ICombinationLotRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<CombinationLot>> SearchCombinationLots(string fromDate, string toDate)
        {
            return await _repository.SearchCombinationLots(fromDate, toDate);
        }

        public async Task<List<CustomerCombLot>> GetCustomerLotsCombine(int customerId, string lotNumber)
        {
            return await _repository.GetCustomerLotsCombine(customerId, lotNumber);
        }

        public async Task<bool> UpsertCombineLot(UpsertCombineLotRequest request)
        {
            return await _repository.UpsertCombineLot(request);
        }

        public async Task<List<CombineLotsDto>> GetCustomerCombineLotsAsync(int comboLotId)
        {
            return await _repository.GetCustomerCombineLotsAsync(comboLotId);
        }
    }
}



