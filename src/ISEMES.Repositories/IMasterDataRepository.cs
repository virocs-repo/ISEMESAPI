using ISEMES.Models;
using System.Data;

namespace ISEMES.Repositories
{
    public interface IMasterDataRepository
    {
        Task<List<string>> GetMasterData();
        Task<List<Address>> GetAddressData();        
        DataTable GetEntityDetailByType(string type);
        Task<List<HardwareTypeDetails>> GetHardwareTypeData();
        Task<List<EmployeeMaster>> GetInvUserByRoleAsync(string filterKey, int isActive, string condition);
    }
}

