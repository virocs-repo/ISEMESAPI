using ISEMES.Models;
using ISEMES.Services;
using Microsoft.AspNetCore.Mvc;

namespace ISEMES.API.Controllers
{
    [Route("api/v1/ise/inventory/inventorydata")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "inventory")]
    public class InventoryDataController : ControllerBase
    {
        private readonly IInventoryDataService _inventoryService;

        public InventoryDataController(IInventoryDataService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        [HttpGet("getdetails")]
        public async Task<IActionResult> GetInventoryDetails(int? customerVendorID = null, [FromQuery] DateTime? fromDate = null, [FromQuery] DateTime? toDate = null)
        {
            var details = await _inventoryService.GetInventoryDetailsAsync(customerVendorID, fromDate, toDate);
            return Ok(details);
        }

        [HttpGet("GetShipmentInventory")]
        public async Task<IActionResult> GetShipmentInventory(int? customerId = null, int? locationId = null, int? receivedFromId = null, int? deviceId = null, int? shipmentCategoryID = null, string lotNumber = null)
        {
            var inventoryData = await _inventoryService.GetShipmentInventoryAsync(customerId, locationId, receivedFromId, deviceId, shipmentCategoryID, lotNumber);
            return Ok(inventoryData);
        }

        [HttpPost("add-ship-record")]
        public async Task<IActionResult> CreateShipmentRecord([FromBody] CreateAddShipRequest request)
        {
            var isSuccess = await _inventoryService.UpsertCreateShipmentRecordAsync(request);
            if (isSuccess)
            {
                return Ok();
            }
            return StatusCode(500, new { message = "Failed to create shipment record." });
        }

        [HttpGet("GetShipdeliveryInfo")]
        public async Task<IActionResult> GetShipmentdeliveryInfo(int? deliveryInfoId = null)
        {
            var shipdeliveryData = await _inventoryService.GetShipmentdeliveryInfo(deliveryInfoId);
            return Ok(shipdeliveryData);
        }

        [HttpGet("GetPackageDimension")]
        public async Task<IActionResult> GetPackageDimensions()
        {
            var dimensionData = await _inventoryService.GetPackageDimensionsAsync();
            return Ok(dimensionData);
        }

        [HttpPost("UpsertShipPackDim")]
        public async Task<IActionResult> UpsertShipPackageDimensions([FromBody] UpsertShipPackageDimensionReq request)
        {
            if (request == null) return BadRequest("Invalid request payload.");
            bool result = await _inventoryService.UpsertPackageDimensionsAsync(request);
            if (result) return Ok();
            return StatusCode(StatusCodes.Status500InternalServerError, "Upsert operation failed.");
        }

        [HttpGet("PackageShipmentdataById")]
        public async Task<ActionResult<List<ShipmentPackage>>> GetPackagesByShipmentId(int shipmentId)
        {
            var packages = await _inventoryService.GetPackagesByShipmentIdAsync(shipmentId);
            if (packages == null || packages.Count == 0)
            {
                return NotFound($"No packages found for ShipmentId {shipmentId}.");
            }
            return Ok(packages);
        }
    }
}

