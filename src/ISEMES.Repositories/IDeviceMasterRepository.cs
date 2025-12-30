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
    }
}
