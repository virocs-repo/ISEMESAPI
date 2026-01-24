using ISEMES.Models;

using ISEMES.Models;

namespace ISEMES.Services
{
    public interface IProbeCardService
    {
        Task<List<ProbeCardResponse>> SearchProbeCard(ProbeCardSearchRequest request);
        Task<ProbeCardDetailsResponse> GetProbeCardDetails(int masterId);
        Task<List<MasterDataItem>> GetPlatforms();
        Task<List<MasterDataItem>> GetProbeCardTypes();
        Task<List<MasterDataItem>> GetProbers();
        Task<List<MasterDataItem>> GetBoardTypes();
        Task<List<SlotItem>> GetSlots(int? hardwareTypeId, string? platformId);
        Task<List<SubSlotItem>> GetSubSlots(int slotId);
        Task<List<LocationInfoItem>> GetLocationsInfo(int subSlotId);
    }
}
