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
    }
}
