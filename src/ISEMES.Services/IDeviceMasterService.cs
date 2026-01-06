using ISEMES.Models;
using System.Data;

namespace ISEMES.Services
{
    public interface IDeviceMasterService
    {
        Task<int> AddUpdateDeviceFamily(DeviceFamilyRequest request);
        Task<int> AddUpdateDevice(DeviceMasterRequest request);
        Task<List<DeviceFamilyResponse>> SearchDeviceFamily(int? customerID, string? deviceFamilyName, bool? active);
        Task<List<DeviceResponse>> SearchDevice(int? customerID, int? deviceFamilyId, string? deviceName, bool? active);
        Task<List<Country>> GetRestrictedCountries();
        Task<List<CustomerLabel>> GetCustomerLabelList(string customerName);
        Task<LabelDetailsResponse> GetLabelDetails(int customerId, int deviceId, string labelName, string lotNum);
        Task<int> AddOrUpdateLabelDetails(int customerId, int deviceId, string labelName, string input);
        Task<DeviceInfoResponse> GetDeviceInfo(int deviceId);
    }
}
