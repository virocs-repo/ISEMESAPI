using ISEMES.Services;
using Microsoft.AspNetCore.Mvc;

namespace ISEMES.API.Controllers
{
    [Route("api/v1/ise/inventory/report")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "inventory")]
    public class InventoryReportController : ControllerBase
    {
        private readonly IInventoryReportService _inventoryReportService;

        public InventoryReportController(IInventoryReportService inventoryReportService)
        {
            _inventoryReportService = inventoryReportService;
        }

        [HttpGet("getallreport")]
        public async Task<IActionResult> GetInventoryReport(
            [FromQuery] int? customerTypeId = null,
            [FromQuery] int? customerVendorId = null,
            [FromQuery] string goodsType = null,
            [FromQuery] string lotNumber = null,
            [FromQuery] int? inventoryStatusId = null,
            [FromQuery] string dateCode = null,
            [FromQuery] DateTime? fromDate = null,
            [FromQuery] DateTime? toDate = null)
        {
            var result = await _inventoryReportService.GetInventoryReportAsync(
                customerTypeId, customerVendorId, goodsType, lotNumber,
                inventoryStatusId, dateCode, fromDate, toDate);
            return Ok(result);
        }
    }
}

