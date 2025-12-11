using Azure.Storage.Blobs;
using ISEMES.Models;
using ISEMES.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ISEMES.API.Controllers
{
    [Route("api/v1/ise/ticketing/ticket")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "ticketing")]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _service;
        private readonly string? _blobConnectionString;
        private readonly string? _blobContainerName;

        public TicketController(ITicketService service, IConfiguration configuration)
        {
            _service = service;
            _blobConnectionString = configuration["BlobStorage:ConnectionString"];
            _blobContainerName = configuration["BlobStorage:ContainerName"];
        }

        [HttpGet("searchTickets")]
        public async Task<IActionResult> SearchTickets([FromQuery] DateTime? fromDate, DateTime? toDate)
        {
            var results = await _service.SearchTickets(fromDate, toDate);
            return Ok(results);
        }

        [HttpGet("getTicketType")]
        public async Task<IActionResult> GetTicketType()
        {
            var results = await _service.GetTicketType();
            return Ok(results);
        }

        [HttpGet("getTicketLots")]
        public async Task<IActionResult> GetTicketLots()
        {
            var results = await _service.GetTicketLots();
            return Ok(results);
        }

        [HttpGet("getTicketLineItemLots")]
        public async Task<IActionResult> GetTicketLineItemLots(string lotNumbers)
        {
            var results = await _service.GetTicketLineItemLots(lotNumbers);
            return Ok(results);
        }

        [HttpPost("upsertTicket")]
        public async Task<IActionResult> UpsertTicket([FromForm] List<IFormFile>? files, [FromForm] string upsertJson, [FromForm] string? ticketAttachments, [FromForm] string? attachmentType, [FromForm] int? tktId)
        {
            string uploadFilename = string.Empty;
            string reviewerAttachments = string.Empty;
            string tAttachments = string.Empty;
            BlobContainerClient containerClient = new BlobContainerClient(_blobConnectionString, _blobContainerName);

            if (ticketAttachments != null && ticketAttachments.Length > 0)
            {
                List<TicketAttachment>? attachments = JsonConvert.DeserializeObject<List<TicketAttachment>>(ticketAttachments);
                if (attachments != null)
                {
                    var deletedAttachments = attachments.Where(a => a.Active == false);
                    foreach (var att in deletedAttachments)
                    {
                        BlobClient blobClient = containerClient.GetBlobClient(att.FileName);
                        blobClient.DeleteIfExists(Azure.Storage.Blobs.Models.DeleteSnapshotsOption.IncludeSnapshots);
                    }
                    tAttachments = string.Join(",", attachments.Where(a => a.Active == true).Select(p => $"{p.FileName}"));
                }
            }

            if (files != null)
            {
                foreach (var file in files)
                {
                    if (file != null && file.Length > 0)
                    {
                        await containerClient.CreateIfNotExistsAsync();
                        string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                        uploadFilename = $"{Path.GetFileNameWithoutExtension(file.FileName)}-{timestamp}{Path.GetExtension(file.FileName)}";

                        BlobClient blobClient = containerClient.GetBlobClient(uploadFilename);
                        using (var stream = file.OpenReadStream())
                        {
                            await blobClient.UploadAsync(stream, true);
                        }
                    }

                    if (attachmentType == "Ticket")
                    {
                        tAttachments = tAttachments.Length > 0 ? tAttachments + "," + uploadFilename : uploadFilename;
                    }
                    else if (attachmentType == "Reviewer")
                    {
                        reviewerAttachments = reviewerAttachments.Length > 0 ? reviewerAttachments + "," + uploadFilename : uploadFilename;
                    }
                }
            }

            var success = await _service.UpsertTicket(upsertJson, tAttachments, reviewerAttachments);
            if (success) return Ok();
            return BadRequest(new { Message = "Upsert operation failed." });
        }

        [HttpGet("getTicketDetail")]
        public async Task<IActionResult> GetTicketDetail(int ticketId)
        {
            var result = await _service.GetTicketDetail(ticketId);
            return Ok(result);
        }

        [HttpGet("voidTicket")]
        public async Task<IActionResult> VoidTicket(int ticketID)
        {
            var success = await _service.VoidTicket(ticketID);
            if (success) return Ok();
            return BadRequest(new { Message = "Void operation failed." });
        }

        [HttpGet("download/{fileName}")]
        public async Task<IActionResult> DownloadFile(string fileName)
        {
            BlobContainerClient containerClient = new BlobContainerClient(_blobConnectionString, _blobContainerName);
            BlobClient blobClient = containerClient.GetBlobClient(fileName);
            if (!await blobClient.ExistsAsync()) return NotFound("File not found.");
            var downloadInfo = await blobClient.DownloadAsync();
            return File(downloadInfo.Value.Content, downloadInfo.Value.ContentType, fileName);
        }
    }
}

