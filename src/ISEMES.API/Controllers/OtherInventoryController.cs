using ISEMES.Models;
using ISEMES.Services;
using Microsoft.AspNetCore.Mvc;

namespace ISEMES.API.Controllers
{
    [Route("api/v1/ise/otherinventory/")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "inventory")]
    public class OtherInventoryController : ControllerBase
    {
        private readonly IOtherInventoryService _otherInventoryService;

        public OtherInventoryController(IOtherInventoryService otherInventoryService)
        {
            _otherInventoryService = otherInventoryService;
        }

        [HttpGet("getOtherInventoryStatus")]
        public async Task<IActionResult> GetOtherInventoryStatusAsync()
        {
            var receiptData = await _otherInventoryService.GetOtherInventoryStatusAsync();
            return Ok(receiptData);
        }

        [HttpGet("getOtherInventoryShipments")]
        public async Task<IActionResult> GetOtherInventoryShipmentsAsync(int? customerId, int? employeeId, int? statusId, DateTime? fromDate, DateTime? toDate)
        {
            var receiptData = await _otherInventoryService.GetOtherInventoryShipmentsAsync(customerId, employeeId, statusId, fromDate, toDate);
            return Ok(receiptData);
        }

        [HttpGet("getOtherInventoryShipment")]
        public async Task<IActionResult> GetOtherInventoryShipmentAsync(int anotherShippingId)
        {
            var receiptData = await _otherInventoryService.GetOtherInventoryShipmentAsync(anotherShippingId);
            return Ok(receiptData);
        }

        [HttpPost("upsertAntherInventoryShipment")]
        public async Task<IActionResult> UpsertAntherInventoryShipmentAsync([FromBody] OrderRequestWrapper request)
        {
            var receiptData = await _otherInventoryService.UpsertAntherInventoryShipmentAsync(request.InputJSON);
            return Ok(receiptData);
        }

        [HttpGet("getServiceTypes")]
        public async Task<IActionResult> GetServiceTypesAsync()
        {
            var receiptData = await _otherInventoryService.GetServiceTypesAsync();
            return Ok(receiptData);
        }

        [HttpGet("voidAnotherShipping")]
        public async Task<IActionResult> VoidAnotherShippingAsync(int anotherShippingID)
        {
            var receiptData = await _otherInventoryService.VoidAnotherShippingAsync(anotherShippingID);
            return Ok(receiptData);
        }

        [HttpGet("getAnotherInventoryLots")]
        public async Task<IActionResult> GetAnotherInventoryLots(int customerTypeId, int customerVendorId)
        {
            var lots = await _otherInventoryService.GetAnotherInventoryLots(customerTypeId, customerVendorId);
            return Ok(lots);
        }
    }
}

