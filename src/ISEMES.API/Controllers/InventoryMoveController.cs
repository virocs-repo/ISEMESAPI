using ISEMES.Models;
using ISEMES.Services;
using Microsoft.AspNetCore.Mvc;

namespace ISEMES.API.Controllers
{
    [Route("api/v1/ise/inventory/move")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "inventory")]
    public class InventoryMoveController : ControllerBase
    {
        private readonly IInventoryDataService _inventoryService;

        public InventoryMoveController(IInventoryDataService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        [HttpGet("search")]
        public async Task<IActionResult> GetInventoryMoveData([FromQuery] DateTime? fromDate = null, [FromQuery] DateTime? toDate = null)
        {
            var details = await _inventoryService.GetInventoryMoveDataAsync(fromDate, toDate);
            return Ok(details);
        }

        [HttpGet("lotinfo")]
        public async Task<IActionResult> GetInventoryMoveLotInfoById([FromQuery] string lotNumber)
        {
            var details = await _inventoryService.GetInventoryMoveLotInfoById(lotNumber);
            return Ok(details);
        }

        [HttpGet("facility")]
        public async Task<IActionResult> GetInventoryMoveAreaByFacility([FromQuery] int facilityId)
        {
            var details = await _inventoryService.GetInventoryMoveAreaByFacility(facilityId);
            return Ok(details);
        }

        [HttpPost]
        public async Task<IActionResult> UpsertInventoryMove([FromQuery] int inventoryId, [FromQuery] int? areaFacilityId, [FromQuery] int? facilityId)
        {
            if (inventoryId <= 0) return BadRequest("Invalid InventoryId.");

            var request = new InventoryMoveRequest
            {
                InventoryId = inventoryId,
                AreaFacilityId = areaFacilityId,
                FacilityId = facilityId
            };

            var result = await _inventoryService.UpsertInventoryMove(request);
            if (result) return Ok();
            return StatusCode(500, "Failed to upsert inventory move.");
        }
    }
}

