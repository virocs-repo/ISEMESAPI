using ISEMES.Models;
using System.Data;

namespace ISEMES.Repositories
{
    public interface IProbeCardRepository
    {
        Task<DataTable> SearchProbeCard(ProbeCardSearchRequest request);
        Task<DataSet> GetProbeCardDetails(int masterId);
        Task<DataTable> GetPlatforms();
        Task<DataTable> GetProbeCardTypes();
        Task<DataTable> GetProbers();
        Task<DataTable> GetBoardTypes();
        Task<DataTable> GetSlots(int? hardwareTypeId, string? platformId);
        Task<DataTable> GetSubSlots(int slotId);
        Task<DataTable> GetLocationsInfo(int subSlotId);
    }
}
