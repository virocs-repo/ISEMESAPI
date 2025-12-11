using Azure.Storage.Blobs;
using ISEMES.Models;
using ISEMES.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ISEMES.API.Controllers
{
    [Route("api/v1/ise/inventory/inventoryHold")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "inventory")]
    public class InventoryHoldController : ControllerBase
    {
        private readonly IHoldInventoryService _holdInventoryService;
        private readonly IReceivingService _receivingService;
        private readonly string _blobConnectionString;
        private readonly string _blobContainerName;

        public InventoryHoldController(IHoldInventoryService holdInventoryService, IReceivingService receivingService, IConfiguration configuration)
        {
            _holdInventoryService = holdInventoryService;
            _receivingService = receivingService;
            _blobConnectionString = configuration["BlobStorage:ConnectionString"];
            _blobContainerName = configuration["BlobStorage:ContainerName"];
        }

        [HttpGet("getAllSearchHold")]
        public async Task<IActionResult> GetAllSearchHold(DateTime? fromDate, DateTime? toDate)
        {
            var details = await _holdInventoryService.GetAllSearchHoldAsync(fromDate, toDate);
            return Ok(details);
        }

        [HttpGet("getHoldType")]
        public async Task<IActionResult> GetHoldTypes([FromQuery] int? inventoryId)
        {
            if (inventoryId == null) return BadRequest("LotId is required.");
            var details = await _holdInventoryService.GetHoldTypesAsync(inventoryId);
            return Ok(details);
        }

        [HttpGet("getHold")]
        public async Task<IActionResult> GetHoldCodes([FromQuery] int? inventoryId, int? holdTypeId)
        {
            if (inventoryId == null) return BadRequest("LotId is required.");
            var details = await _holdInventoryService.GetHoldCodesAsync(inventoryId, holdTypeId);
            return Ok(details);
        }

        [HttpGet("getAllHolds")]
        public async Task<IActionResult> GetAllHolds([FromQuery] int inventoryID)
        {
            var holds = await _holdInventoryService.GetAllHoldsAsync(inventoryID);
            if (holds == null) return NotFound($"No holds found for Inventory ID: {inventoryID}");
            return Ok(holds);
        }

        [HttpPost("UpsertHold")]
        public async Task<IActionResult> UpsertHold([FromForm] IFormFile[]? files, [FromForm] string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return BadRequest("Invalid input.");

            var request = JsonConvert.DeserializeObject<HoldRequest>(input);
            int insertedHoldId = await _holdInventoryService.UpsertHoldAsync(request);

            if (files != null && files.Length > 0)
            {
                BlobContainerClient containerClient = new BlobContainerClient(_blobConnectionString, _blobContainerName);
                await containerClient.CreateIfNotExistsAsync();
                string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");

                foreach (var file in files)
                {
                    string uploadFilename = $"{insertedHoldId}-{Path.GetFileNameWithoutExtension(file.FileName)}-{timestamp}{Path.GetExtension(file.FileName)}";
                    BlobClient blobClient = containerClient.GetBlobClient(uploadFilename);
                    using (var stream = file.OpenReadStream())
                    {
                        await blobClient.UploadAsync(stream, true);
                    }

                    var attachment = new Attachment
                    {
                        AttachmentName = string.IsNullOrEmpty(request.CategoryName) ? "ShipAlert" : request.CategoryName,
                        ObjectID = insertedHoldId,
                        Path = uploadFilename,
                        Active = true,
                        LoginId = request.UserId
                    };
                    await _receivingService.UpsertAttachment(attachment);
                }
            }

            return Ok(new { InsertedHoldID = insertedHoldId, message = "Inventory hold status updated successfully." });
        }

        [HttpGet("getHoldDetails")]
        public async Task<IActionResult> GetHoldDetails([FromQuery] int inventoryXHoldId)
        {
            var result = await _holdInventoryService.GetHoldDetailsAsync(inventoryXHoldId);
            return Ok(result);
        }

        [HttpGet("getHoldComments")]
        public async Task<IActionResult> GetHoldComments()
        {
            var details = await _holdInventoryService.GetHoldCommentsAsync();
            return Ok(details);
        }

        [HttpGet("getCustomerDetails")]
        public async Task<IActionResult> GetCustomerDetails([FromQuery] int inventoryID)
        {
            var holds = await _holdInventoryService.GetCustomerDetailsAsync(inventoryID);
            if (holds == null) return NotFound($"No holds found for Inventory ID: {inventoryID}");
            return Ok(holds);
        }

        [HttpGet("getOperaterAttachments")]
        public async Task<IActionResult> GetOperaterAttachments([FromQuery] int TFSHoldId)
        {
            var attachements = await _holdInventoryService.GetOperaterAttachmentsAsync(TFSHoldId);
            if (attachements == null) return NotFound($"No holds found for Inventory ID: {TFSHoldId}");
            return Ok(attachements);
        }
    }
}

