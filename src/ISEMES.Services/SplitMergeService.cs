using ISEMES.Models;
using ISEMES.Repositories;

namespace ISEMES.Services
{
    public class SplitMergeService : ISplitMergeService
    {
        private readonly ISplitMergeRepository _splitMergeRepository;

        public SplitMergeService(ISplitMergeRepository splitMergeRepository)
        {
            _splitMergeRepository = splitMergeRepository;
        }

        public async Task<List<MasterList>> GetMasterListItems(string listName, int? serviceId)
        {
            return await _splitMergeRepository.GetMasterListItems(listName, serviceId);
        }

        public async Task<List<MasterList>> GetLotStatus()
        {
            return await _splitMergeRepository.GetLotStatus();
        }

        public async Task<List<TFSCustomer>> GetTFSCustomers()
        {
            return await _splitMergeRepository.GetTFSCustomers();
        }

        public async Task<List<TFSDeviceFamily>> GetDeviceFamilies(int customerId)
        {
            return await _splitMergeRepository.GetDeviceFamilies(customerId);
        }

        public async Task<List<TFSDevice>> GetDevices(int customerId, int? deviceFamiltyId, int? deviceId)
        {
            return await _splitMergeRepository.GetDevices(customerId, deviceFamiltyId, deviceId);
        }

        public async Task<List<TFSDeviceAlias>> GetDeviceAlias(int customerId, int deviceFamiltyId, int deviceId, string? source)
        {
            return await _splitMergeRepository.GetDeviceAlias(customerId, deviceFamiltyId, deviceId, source);
        }

        public async Task<List<LotOwner>> GetLotOwners()
        {
            return await _splitMergeRepository.GetLotOwners();
        }

        public async Task<List<LotSearch>> InventoryLotSearch(string? travellerStatusIds, string? lotStatusIds, DateTime? fromDate, DateTime? toDate)
        {
            return await _splitMergeRepository.InventoryLotSearch(travellerStatusIds, lotStatusIds, fromDate, toDate);
        }

        public async Task<LotDetail> GetInventoryLot(int lotId, string source)
        {
            return await _splitMergeRepository.GetInventoryLot(lotId, source);
        }

        public async Task<List<MergeLot>> GetMergeLots(int lotId)
        {
            return await _splitMergeRepository.GetMergeLots(lotId);
        }

        public async Task<List<IcrSearch>> IcrDashboardSearch(int? customerId, string? travellerStatusIds, string? lotStatusIds, DateTime? fromDate, DateTime? toDate, string? requestTypeIds)
        {
            return await _splitMergeRepository.IcrDashboardSearch(customerId, travellerStatusIds, lotStatusIds, fromDate, toDate, requestTypeIds);
        }

        public async Task<List<MergedLots>> GetMatchedLots(int trvStepId)
        {
            return await _splitMergeRepository.GetMatchedLots(trvStepId);
        }

        public async Task<bool> SaveMergeLots(LotMerge request)
        {
            return await _splitMergeRepository.SaveMergeLots(request);
        }

        public async Task<int> AddOrUpdateMergeAsync(int? mergeId, int trvStepId, string? lotIds, int userId)
        {
            return await _splitMergeRepository.AddOrUpdateMergeAsync(mergeId, trvStepId, lotIds, userId);
        }

        public async Task<List<FutureSplitBin>> GetFutureSplitBinsAsync(int trvStepId)
        {
            return await _splitMergeRepository.GetFutureSplitBinsAsync(trvStepId);
        }

        public async Task<List<SplitBin>> GetSplitBinsAsync(int lotId, int trvStepId, bool rejectBins)
        {
            return await _splitMergeRepository.GetSplitBinsAsync(lotId, trvStepId, rejectBins);
        }

        public async Task<List<FutureSplit>> GetFutureSplitsAsync(int trvStepId)
        {
            return await _splitMergeRepository.GetFutureSplitsAsync(trvStepId);
        }

        public async Task<List<TotalSplitDto>> GetSplitsAsync(int trvStepId)
        {
            return await _splitMergeRepository.GetSplitsAsync(trvStepId);
        }

        public async Task<bool> AddOrUpdateSplit(SplitRequest splitRequest)
        {
            return await _splitMergeRepository.AddOrUpdateSplit(splitRequest);
        }

        public async Task<bool> AddOrUpdateFutureSplit(SplitRequest splitRequest)
        {
            return await _splitMergeRepository.AddOrUpdateFutureSplit(splitRequest);
        }

        public async Task<SplitPreviewBOOutPut> GetPreviewDetails(int trvStepId, int lotId)
        {
            return await _splitMergeRepository.GetPreviewDetails(trvStepId, lotId);
        }

        public async Task<SplitPreviewBOOutPut> GetFSPreviewDetails(int trvStepId)
        {
            return await _splitMergeRepository.GetFSPreviewDetails(trvStepId);
        }

        public async Task<string> GenerateFutureSplit(int trvStepId)
        {
            return await _splitMergeRepository.GenerateFutureSplit(trvStepId);
        }

        public async Task<string> GenerateSplit(int trvStepId, int userId)
        {
            return await _splitMergeRepository.GenerateSplit(trvStepId, userId);
        }
    }
}

