using ISEMES.Models;
using ISEMES.Services;
using Microsoft.AspNetCore.Mvc;

namespace ISEMES.API.Controllers
{
    [Route("api/v1/ise/inventory/inttransfer")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "inventory")]
    public class IntTranferReceivingController : ControllerBase
    {
        private readonly IIntTranferReceivingService _service;

        public IntTranferReceivingController(IIntTranferReceivingService service)
        {
            _service = service;
        }

        [HttpGet("Search")]
        public async Task<IActionResult> Search([FromQuery] DateTime? fromDate = null, [FromQuery] DateTime? toDate = null, [FromQuery] string? statusString = null, [FromQuery] string? facilityId = null)
        {
            var result = await _service.SearchInternalTransferReceivingAsync(fromDate, toDate, statusString, facilityId);
            return Ok(result);
        }

        [HttpGet("GetcustTransferLots")]
        public async Task<IActionResult> GetInternalTransferLot([FromQuery] int customerVendorID, [FromQuery] int customerTypeID)
        {
            var result = await _service.GetInternalTransferLotAsync(customerVendorID, customerTypeID);
            return Ok(result);
        }

        [HttpPost("UpsertIntTransReceipt")]
        public async Task<IActionResult> UpsertInternalTransferReceipt([FromBody] IntTransferReceiptReq request)
        {
            var result = await _service.UpsertInternalTransferReceiptAsync(request);
            return Ok();
        }
    }
}

