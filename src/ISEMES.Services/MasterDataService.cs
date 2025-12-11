using ISEMES.Models;
using ISEMES.Repositories;
using System.Data;

namespace ISEMES.Services
{
    public class MasterDataService : IMasterDataService
    {
        private readonly IMasterDataRepository _repository;

        public MasterDataService(IMasterDataRepository repository)
        {
            _repository = repository;
        }

        public Task<List<Address>> GetAddressData()
        {
            return _repository.GetAddressData();
        }

        public DataTable GetEntityDetailByType(string type)
        {
            return _repository.GetEntityDetailByType(type);
        }

        public Task<List<HardwareTypeDetails>> GetHardwareTypeData()
        {
            return _repository.GetHardwareTypeData();
        }

        public Task<List<string>> GetMasterData()
        {
            return _repository.GetMasterData();
        }

        public Task<List<EmployeeMaster>> GetInvUserByRoleAsync(string filterKey, int isActive, string condition)
        {
            return _repository.GetInvUserByRoleAsync(filterKey, isActive, condition);
        }
    }
}

