using ISEMES.Models;

namespace ISEMES.Repositories
{
    public interface ISplitMergeRepository
    {
        Task<List<MasterList>> GetMasterListItems(string listName, int? serviceId);
        Task<List<MasterList>> GetLotStatus();
        Task<List<TFSCustomer>> GetTFSCustomers();
        Task<List<TFSDeviceFamily>> GetDeviceFamilies(int customerId);
        Task<List<TFSDevice>> GetDevices(int customerId, int? deviceFamiltyId, int? deviceId);
        Task<List<TFSDeviceAlias>> GetDeviceAlias(int customerId, int deviceFamilyId, int deviceId, string? source);
        Task<List<LotOwner>> GetLotOwners();
        Task<List<LotSearch>> InventoryLotSearch(string? travellerStatusIds, string? lotStatusIds, DateTime? fromDate, DateTime? toDate);
        Task<LotDetail> GetInventoryLot(int lotId, string source);
        Task<List<MergeLot>> GetMergeLots(int lotId);
        Task<List<IcrSearch>> IcrDashboardSearch(int? customerId, string? travellerStatusIds, string? lotStatusIds, DateTime? fromDate, DateTime? toDate, string? requestTypeIds);
        Task<List<MergedLots>> GetMatchedLots(int trvStepId);
        Task<bool> SaveMergeLots(LotMerge request);
        Task<int> AddOrUpdateMergeAsync(int? mergeId, int trvStepId, string? lotIds, int userId);
        Task<List<FutureSplitBin>> GetFutureSplitBinsAsync(int trvStepId);
        Task<List<SplitBin>> GetSplitBinsAsync(int lotId, int trvStepId, bool rejectBins);
        Task<List<FutureSplit>> GetFutureSplitsAsync(int trvStepId);
        Task<List<TotalSplitDto>> GetSplitsAsync(int trvStepId);
        Task<bool> AddOrUpdateSplit(SplitRequest splitRequest);
        Task<bool> AddOrUpdateFutureSplit(SplitRequest splitRequest);
        Task<SplitPreviewBOOutPut> GetPreviewDetails(int trvStepId, int lotId);
        Task<SplitPreviewBOOutPut> GetFSPreviewDetails(int trvStepId);
        Task<string> GenerateFutureSplit(int trvStepId);
        Task<string> GenerateSplit(int trvStepId, int userId);
    }
}

