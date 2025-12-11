using ISEMES.Models;
using ISEMES.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ISEMES.API.Controllers
{
    [Route("api/v1/ise/inventory/splitmerge")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "inventory")]
    public class SplitMergeController : ControllerBase
    {
        private readonly ISplitMergeService _splitMergeService;

        public SplitMergeController(ISplitMergeService splitMergeService)
        {
            _splitMergeService = splitMergeService;
        }

        [HttpGet("getMasterListItems")]
        public async Task<IActionResult> GetTravellerStatus(string listName, int? serviceId)
        {
            var receiptData = await _splitMergeService.GetMasterListItems(listName, serviceId);
            return Ok(receiptData);
        }

        [HttpGet("getLotStatus")]
        public async Task<IActionResult> GetLotStatus()
        {
            var receiptData = await _splitMergeService.GetLotStatus();
            return Ok(receiptData);
        }

        [HttpGet("getTFSCustomers")]
        public async Task<IActionResult> GetTFSCustomers()
        {
            var receiptData = await _splitMergeService.GetTFSCustomers();
            return Ok(receiptData);
        }

        [HttpGet("getDeviceFamilies")]
        public async Task<IActionResult> GetDeviceFamilies(int customerId)
        {
            var receiptData = await _splitMergeService.GetDeviceFamilies(customerId);
            return Ok(receiptData);
        }

        [HttpGet("getDevices")]
        public async Task<IActionResult> GetDevices(int customerId, int? deviceFamiltyId, int? deviceId)
        {
            var receiptData = await _splitMergeService.GetDevices(customerId, deviceFamiltyId, deviceId);
            return Ok(receiptData);
        }

        [HttpGet("getDeviceAlias")]
        public async Task<IActionResult> GetDeviceAlias(int customerId, int deviceFamiltyId, int deviceId, string? source)
        {
            var receiptData = await _splitMergeService.GetDeviceAlias(customerId, deviceFamiltyId, deviceId, source);
            return Ok(receiptData);
        }

        [HttpGet("getLotOwners")]
        public async Task<IActionResult> GetLotOwners()
        {
            var receiptData = await _splitMergeService.GetLotOwners();
            return Ok(receiptData);
        }

        [HttpGet("inventoryLotSearch")]
        public async Task<IActionResult> InventoryLotSearch(string? travellerStatusIds, string? lotStatusIds, DateTime? fromDate, DateTime? toDate)
        {
            var receiptData = await _splitMergeService.InventoryLotSearch(travellerStatusIds, lotStatusIds, fromDate, toDate);
            return Ok(receiptData);
        }

        [HttpGet("getInventory")]
        public async Task<IActionResult> GetInventoryLot([FromQuery] int lotId, [FromQuery] string source)
        {
            var receiptData = await _splitMergeService.GetInventoryLot(lotId, source);
            return Ok(receiptData);
        }

        [HttpGet("getMergeLots")]
        public async Task<IActionResult> GetMergeLots([FromQuery] int lotId)
        {
            var receiptData = await _splitMergeService.GetMergeLots(lotId);
            return Ok(receiptData);
        }

        [HttpGet("icrDashboardSearch")]
        public async Task<IActionResult> IcrDashboardSearch(int? customerId, string? travellerStatusIds, string? lotStatusIds, DateTime? fromDate, DateTime? toDate, string? requestTypeIds)
        {
            var receiptData = await _splitMergeService.IcrDashboardSearch(customerId, travellerStatusIds, lotStatusIds, fromDate, toDate, requestTypeIds);
            return Ok(receiptData);
        }

        [HttpGet("getMatchedLots")]
        public async Task<IActionResult> GetMatchedLots(int trvStepId)
        {
            var receiptData = await _splitMergeService.GetMatchedLots(trvStepId);
            return Ok(receiptData);
        }

        [HttpPost("saveMergeLots")]
        public async Task<IActionResult> SaveMergeLots(LotMerge request)
        {
            var receiptData = await _splitMergeService.SaveMergeLots(request);
            return Ok(receiptData);
        }

        [HttpPost("addOrUpdateMerge")]
        public async Task<IActionResult> AddOrUpdateMerge([FromBody] MergeRequestDto request)
        {
            var data = await _splitMergeService.AddOrUpdateMergeAsync(request.MergeId, request.TrvStepId, request.LotIds, request.UserId);
            return Ok(data);
        }

        [HttpGet("getFutureSplitBins")]
        public async Task<IActionResult> GetFutureSplitBinsAsync(int trvStepId)
        {
            var Data = await _splitMergeService.GetFutureSplitBinsAsync(trvStepId);
            return Ok(Data);
        }

        [HttpGet("getSplitBins")]
        public async Task<IActionResult> GetSplitBinsAsync(int lotId, int trvStepId, bool rejectBins)
        {
            var Data = await _splitMergeService.GetSplitBinsAsync(lotId, trvStepId, rejectBins);
            return Ok(Data);
        }

        [HttpGet("getFutureSplits")]
        public async Task<IActionResult> GetFutureSplitsAsync(int trvStepId)
        {
            var Data = await _splitMergeService.GetFutureSplitsAsync(trvStepId);
            return Ok(Data);
        }

        [HttpGet("getSplits")]
        public async Task<IActionResult> GetSplitsAsync(int trvStepId)
        {
            var Data = await _splitMergeService.GetSplitsAsync(trvStepId);
            return Ok(Data);
        }

        [HttpPost("addOrUpdateSplit")]
        public async Task<IActionResult> AddOrUpdateSplit([FromBody] SplitRequest request)
        {
            var receiptData = await _splitMergeService.AddOrUpdateSplit(request);
            return Ok(receiptData);
        }

        [HttpPost("addOrUpdateFutureSplit")]
        public async Task<IActionResult> AddOrUpdateFutureSplit([FromBody] SplitRequest request)
        {
            var receiptData = await _splitMergeService.AddOrUpdateFutureSplit(request);
            return Ok(receiptData);
        }

        [HttpGet("getSplitPreviewDetails")]
        public async Task<IActionResult> GetPreviewDetailsAsync(int trvStepId, int lotId)
        {
            SplitPreviewBOOutPut splitPreviewBOOutPut = await _splitMergeService.GetPreviewDetails(trvStepId, lotId);
            if (!splitPreviewBOOutPut.ReturnMessage.IsNullOrEmpty())
            {
                return BadRequest(splitPreviewBOOutPut.ReturnMessage);
            }
            return Ok(splitPreviewBOOutPut.SplitPreviewBO);
        }

        [HttpGet("getFSPreviewDetails")]
        public async Task<IActionResult> GetFSPreviewDetailsAsync(int trvStepId)
        {
            SplitPreviewBOOutPut splitPreviewBOOutPut = await _splitMergeService.GetFSPreviewDetails(trvStepId);
            if (!splitPreviewBOOutPut.ReturnMessage.IsNullOrEmpty())
            {
                return BadRequest(splitPreviewBOOutPut.ReturnMessage);
            }
            return Ok(splitPreviewBOOutPut.SplitPreviewBO);
        }

        [HttpGet("generateFutureSplit")]
        public async Task<IActionResult> GenerateFutureSplitAsync(int trvStepId)
        {
            return new ContentResult
            {
                Content = await _splitMergeService.GenerateFutureSplit(trvStepId),
                ContentType = "text/plain",
                StatusCode = 200
            };
        }

        [HttpGet("generateSplit")]
        public async Task<IActionResult> GenerateSplitAsync(int trvStepId, int userId)
        {
            return new ContentResult
            {
                Content = await _splitMergeService.GenerateSplit(trvStepId, userId),
                ContentType = "text/plain",
                StatusCode = 200
            };
        }
    }
}

