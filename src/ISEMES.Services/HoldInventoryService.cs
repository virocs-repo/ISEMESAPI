using ISEMES.Models;
using ISEMES.Repositories;

namespace ISEMES.Services
{
    public class HoldInventoryService : IHoldInventoryService
    {
        private readonly IHoldInventoryRepositories _repository;

        public HoldInventoryService(IHoldInventoryRepositories repository)
        {
            _repository = repository;
        }

        public async Task<List<InventoryHolds>> GetAllSearchHoldAsync(DateTime? fromDate, DateTime? toDate)
        {
            return await _repository.GetAllSearchHoldAsync(fromDate, toDate);
        }

        public async Task<IEnumerable<HoldTypeResponse>> GetHoldTypesAsync(int? inventoryId)
        {
            return await _repository.GetHoldTypesAsync(inventoryId);
        }

        public async Task<IEnumerable<HoldGroup>> GetHoldCodesAsync(int? inventoryId, int? holdTypeId)
        {
            List<HoldGroup> holdGroups = new List<HoldGroup>();
            var filterResult = await _repository.GetHoldCodesAsync(inventoryId);
            var result = holdTypeId.HasValue 
                ? filterResult.Where(e => e.ServiceId == holdTypeId).ToList() 
                : filterResult.ToList();

            int grpId = 0;
            var groups = result.GroupBy(x => x.GroupName).Select(x => new HoldGroup { GroupId = ++grpId, GroupName = x.Key });

            foreach (var grp in groups)
            {
                var holdGrp = new HoldGroup { GroupId = grp.GroupId, GroupName = grp.GroupName };
                var holdGrpDetail = result.Where(x => x.GroupName == grp.GroupName).Select(x => new HoldGroupDetail { HoldCodeId = x.HoldCodeId, HoldCode = x.HoldCode });
                holdGrp.HoldGroupDetails = new List<HoldGroupDetail>();
                holdGrp.HoldGroupDetails.AddRange(holdGrpDetail);
                holdGroups.Add(holdGrp);
            }
            return holdGroups.OrderBy(x => x.GroupId);
        }

        public async Task<IEnumerable<InventoryHold>> GetAllHoldsAsync(int? inventoryId)
        {
            return await _repository.GetAllHoldsAsync(inventoryId);
        }

        public async Task<int> UpsertHoldAsync(HoldRequest holdRequest)
        {
            return await _repository.UpsertHoldAsync(holdRequest);
        }

        public async Task<IEnumerable<HoldDetails>> GetHoldDetailsAsync(int? inventoryXHoldId)
        {
            return await _repository.GetHoldDetailsAsync(inventoryXHoldId);
        }

        public async Task<List<HoldComment>> GetHoldCommentsAsync()
        {
            return await _repository.GetHoldCommentsAsync();
        }

        public async Task<IEnumerable<HoldCustomerDetails>> GetCustomerDetailsAsync(int? inventoryId)
        {
            return await _repository.GetCustomerDetailsAsync(inventoryId);
        }

        public async Task<IEnumerable<OperaterAttachements>> GetOperaterAttachmentsAsync(int? TFSHoldId)
        {
            return await _repository.GetOperaterAttachmentsAsync(TFSHoldId);
        }
    }
}



