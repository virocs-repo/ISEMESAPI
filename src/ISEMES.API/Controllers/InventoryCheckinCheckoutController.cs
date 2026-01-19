using ISEMES.Models;
using ISEMES.Services;
using Microsoft.AspNetCore.Mvc;

namespace ISEMES.API.Controllers
{
    [Route("api/v1/ise/inventory/inventoryCheckinCheckout")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "inventory")]
    public class InventoryCheckinCheckoutController : ControllerBase
    {
        private readonly ICheckinCheckoutInventoryService _moveInventoryService;

        public InventoryCheckinCheckoutController(ICheckinCheckoutInventoryService moveInventoryService)
        {
            _moveInventoryService = moveInventoryService;
        }

        [HttpGet("getallInventoryCheckinCheckoutStatus")]
        public async Task<IActionResult> GetAllInventoryCheckinCheckoutStatus(DateTime? fromDate, DateTime? toDate)
        {
            var details = await _moveInventoryService.GetAllInventoryCheckinCheckoutStatusAsync(fromDate, toDate);
            return Ok(details);
        }

        [HttpGet("getInventoryCheckinCheckoutLocation")]
        public async Task<IActionResult> GetInventoryCheckinCheckoutLocation()
        {
            var inventoryDetails = await _moveInventoryService.GetInventoryCheckinCheckoutLocationAsync();
            return Ok(inventoryDetails);
        }

        [HttpGet("getInventoryCheckinCheckoutStatuslist")]
        public async Task<IActionResult> GetInventoryCheckinCheckoutStatus()
        {
            var inventoryDetails = await _moveInventoryService.GetInventoryCheckinCheckoutStatusAsync();
            return Ok(inventoryDetails);
        }

        [HttpGet("getInventoryCheckinCheckoutStatus")]
        public async Task<IActionResult> GetInventoryCheckinCheckout([FromQuery] string lotNumber)
        {
            var inventory = await _moveInventoryService.GetInventoryCheckinCheckoutAsync(lotNumber);
            return Ok(inventory);
        }

        [HttpPost("UpsertInventoryCheckinCheckoutStatus")]
        public IActionResult UpsertInventoryCheckinCheckoutStatus([FromBody] InventoryCheckinCheckoutRequest request)
        {
            if (request == null || request.InvMovementDetails == null || request.InvMovementDetails.Count == 0)
            {
                return BadRequest("Invalid input.");
            }

            try
            {
                _moveInventoryService.UpsertInventoryCheckinCheckoutStatusAsync(request);
                return Ok("Inventory move status updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("getCheckinCheckoutStatus")]
        public async Task<IActionResult> GetCheckinCheckout([FromQuery] string lotNumber)
        {
            var inventory = await _moveInventoryService.GetCheckinCheckoutAsync(lotNumber);
            return Ok(inventory);
        }
        [HttpPost("validateBadge")]
        public async Task<EmployeeDetails?> ValidateBadge(string badge)
        {
            return await _moveInventoryService.ValidateBadge(badge);
        }

        [HttpGet("getCheckInCheckOutLotDetails")]
        public async Task<IActionResult> GetCheckInCheckOutLotDetails([FromQuery] string? lotNumber, [FromQuery] int employeeId, [FromQuery] int customerLoginId, [FromQuery] string requestType, [FromQuery] int? count)
        {
            var lotDetails = await _moveInventoryService.GetCheckInCheckOutLotDetailsAsync(lotNumber, employeeId, customerLoginId, requestType, count);
            return Ok(lotDetails);
        }
        [HttpGet("getLastTenCheckOutLotDetails")]
        public async Task<IActionResult> GetLastTenCheckOutLotDetailsAsync()
        {
            var lotDetails = await _moveInventoryService.GetLastTenCheckOutLotDetailsAsync();
            return Ok(lotDetails);
        }
        [HttpPost("saveCheckInCheckOutRequest")]
        public async Task<bool> SaveCheckInRequest([FromBody] CheckInCheckOutRequest request)
        {
            return await _moveInventoryService.SaveCheckInCheckOutRequest(request.InputJson, request.RequestType);
        }
    }
}

