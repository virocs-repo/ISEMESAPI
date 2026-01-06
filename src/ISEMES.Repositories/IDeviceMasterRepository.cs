using ISEMES.Models;
using System.Data;

namespace ISEMES.Repositories
{
    public interface IDeviceMasterRepository
    {
        Task<int> AddUpdateDeviceFamily(DeviceFamilyRequest request);
        Task<int> AddUpdateDevice(DeviceMasterRequest request);
        Task<DataTable> SearchDeviceFamily(int? customerID, string? deviceFamilyName, bool? active);
        Task<DataTable> SearchDevice(int? customerID, int? deviceFamilyId, string? deviceName, bool? active);
        Task<List<Country>> GetRestrictedCountries();
        Task<List<CustomerLabel>> GetCustomerLabelList(string customerName);
        Task<LabelDetailsResponse> GetLabelDetails(int customerId, int deviceId, string labelName, string lotNum);
        Task<int> AddOrUpdateLabelDetails(int customerId, int deviceId, string labelName, string input);
        Task<DataSet> GetDeviceInfo(int deviceId);
    }
}
