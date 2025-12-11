using ISEMES.Models;
using ISEMES.Services;
using Microsoft.AspNetCore.Mvc;

namespace ISEMES.API.Controllers
{
    [Route("api/v1/ise/inventory/combinedlot")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "inventory")]
    public class CombinedLotController : ControllerBase
    {
        private readonly ICombinationLotService _service;

        public CombinedLotController(ICombinationLotService service)
        {
            _service = service;
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchCombinationLots([FromQuery] string fromDate = "", [FromQuery] string toDate = "")
        {
            var results = await _service.SearchCombinationLots(fromDate, toDate);
            return Ok(results);
        }

        [HttpGet("customer")]
        public async Task<IActionResult> GetCustomerLotsCombine([FromQuery] int customerId, [FromQuery] string lotNumber = "")
        {
            var results = await _service.GetCustomerLotsCombine(customerId, lotNumber);
            return Ok(results);
        }

        [HttpPost("upinsertcombolot")]
        public async Task<IActionResult> UpsertCombineLot([FromBody] UpsertCombineLotRequest request)
        {
            var success = await _service.UpsertCombineLot(request);
            if (success) return Ok();
            return BadRequest(new { Message = "Upsert operation failed." });
        }

        [HttpGet("vieweditcombolots")]
        public async Task<IActionResult> GetCustomerCombineLotsAsync([FromQuery] int comboLotId)
        {
            var data = await _service.GetCustomerCombineLotsAsync(comboLotId);
            return Ok(data);
        }
    }
}

