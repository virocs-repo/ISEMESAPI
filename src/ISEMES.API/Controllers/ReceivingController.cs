using Azure.Storage.Blobs;
using ISEMES.Models;
using ISEMES.Services;
using ISEMES.API.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;

namespace ISEMES.API.Controllers
{
    [Route("api/v1/ise/inventory/")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "inventory")]
    public class ReceivingController : ControllerBase
    {
        private readonly TokenProvider _tokenProvider;
        private readonly IReceivingService _receivingService;
        private readonly IConfiguration _configuration;
        private readonly string _blobConnectionString;
        private readonly string _blobContainerName;

        public ReceivingController(TokenProvider tokenProvider, IReceivingService receivingService, IConfiguration configuration)
        {
            _tokenProvider = tokenProvider;
            _receivingService = receivingService;
            _configuration = configuration;
            _blobConnectionString = configuration["BlobStorage:ConnectionString"];
            _blobContainerName = configuration["BlobStorage:ContainerName"];
        }

        [HttpGet("receiptdata")]
        public async Task<IActionResult> GetReceiptdata(int? receiptID, DateTime? fromDate, DateTime? toDate, string? receiptStatus, string? facilityIDStr)
        {
            var receiptData = await _receivingService.GetReceiptDetailsAsync(receiptID, fromDate, toDate, receiptStatus, facilityIDStr);
            return Ok(receiptData);
        }

        [HttpGet("devicedata")]
        public async Task<IActionResult> GetDeviceData(string? mailRoomNo, string? stagingLocation)
        {
            var receiptData = await _receivingService.GetDeviceDetailsAsync(mailRoomNo, stagingLocation);
            return Ok(receiptData);
        }

        [HttpGet("hardwaredata")]
        public async Task<IActionResult> GetHardwareData(int receiptId)
        {
            var receiptData = await _receivingService.GetHardwareDetailsAsync(receiptId);
            return Ok(receiptData);
        }

        [HttpGet("receiptEmployee")]
        public async Task<IActionResult> GetReceiptEmployee(int receiptId)
        {
            var receiptData = await _receivingService.GetReceiptEmployeeAsync(receiptId);
            return Ok(receiptData);
        }

        [HttpPost("processReceipt")]
        public async Task<IActionResult> ProcessReceiptData([FromBody] ReceiptRequestDetails request)
        {
            if (request == null) return BadRequest("Mandatory field(s) not supplied.");
            await _receivingService.UpdateReceiptDetailsAsync(request);
            return Ok();
        }

        [HttpPost("processDevice")]
        public async Task<IActionResult> ProcessDevice([FromBody] DeviceDetailsRequest request)
        {
            if (request == null) return BadRequest("Mandatory field(s) not supplied.");
            await _receivingService.UpdateDeviceAsync(request);
            return Ok();
        }

        [HttpPost("processHardware")]
        public async Task<IActionResult> ProcessHardware([FromBody] HardwareDetailsRequest request)
        {
            if (request == null) return BadRequest("Mandatory field(s) not supplied.");
            await _receivingService.UpdateHardwareAsync(request);
            return Ok();
        }

        [HttpPost("processMiscellaneousGoods")]
        public async Task<IActionResult> ProcessMiscellaneousGoods([FromBody] MiscellaneousGoodsDetailsRequest request)
        {
            if (request == null) return BadRequest("Mandatory field(s) not supplied.");
            await _receivingService.UpdateMiscellaneousGoodsAsync(request);
            return Ok();
        }

        [HttpGet("miscellaneousGoods")]
        public async Task<IActionResult> GetMiscellaneousGoods(int receiptId)
        {
            var receiptData = await _receivingService.GetMiscellaneousGoodsAsync(receiptId);
            return Ok(receiptData);
        }

        [HttpPost("voidReceipt")]
        public async Task<IActionResult> voidReceipt(int receiptID)
        {
            await _receivingService.VoidReceipt(receiptID);
            return Ok();
        }

        [HttpGet("deviceTypeByCustomer")]
        public async Task<IActionResult> GetDeviceTypeByCustomer(int? customerId = null)
        {
            var deviceTypes = await _receivingService.GetDeviceTypeByCustomer(customerId);
            return Ok(deviceTypes);
        }

        [HttpGet("lineItem")]
        public async Task<IActionResult> GetGeneratedLineItemnumber()
        {
            var result = await _receivingService.GetGeneratedLineItemnumber();
            return Ok(result);
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file, int receiptId, int loginId)
        {
            if (file == null || file.Length == 0) return BadRequest("File not provided.");

            BlobContainerClient containerClient = new BlobContainerClient(_blobConnectionString, _blobContainerName);
            await containerClient.CreateIfNotExistsAsync();
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            string uploadFilename = $"{receiptId}-{Path.GetFileNameWithoutExtension(file.FileName)}-{timestamp}{Path.GetExtension(file.FileName)}";
            BlobClient blobClient = containerClient.GetBlobClient(uploadFilename);

            using (var stream = file.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, true);
            }

            var attachment = new Attachment
            {
                AttachmentName = "Receipt",
                ObjectID = receiptId,
                Path = uploadFilename,
                Active = true,
                LoginId = loginId
            };
            await _receivingService.UpsertAttachment(attachment);
            return Ok();
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

        [HttpDelete("delete-attachment")]
        public async Task<IActionResult> DeleteFile(Attachment attachment)
        {
            await _receivingService.UpsertAttachment(attachment);
            return Ok();
        }

        [HttpGet("receipt-attachments")]
        public async Task<IActionResult> ListFiles(int receiptId)
        {
            var receiptAttachment = await _receivingService.ListReceiptAttachment(receiptId);
            return Ok(receiptAttachment);
        }

        [HttpGet("get-attachmentslist")]
        public async Task<IActionResult> ListAttchFiles(int id, string? categoryName = null)
        {
            var receiptAttachment = await _receivingService.ListAttachmentById(id, categoryName);
            return Ok(receiptAttachment);
        }

        [HttpGet("getInterimLots")]
        public async Task<IActionResult> GetInterimLotsAsync()
        {
            var interimLots = await _receivingService.GetInterimLotsAsync();
            return Ok(interimLots);
        }

        [HttpGet("getInterimDeviceData")]
        public async Task<IActionResult> GetInterimDeviceDataAsync(int interimReceiptID)
        {
            var interimDeviceData = await _receivingService.GetInterimDeviceDataAsync(interimReceiptID);
            return Ok(interimDeviceData);
        }

        [HttpGet("getdetailsByInventoryID")]
        public async Task<IActionResult> GetdetailsByInventoryIdAsync(int inventoryID)
        {
            var result = await _receivingService.GetdetailsByInventoryIdAsync(inventoryID);
            return Ok(result);
        }

        [HttpPost("processInterimDevice")]
        public async Task<IActionResult> ProcessInterimDevice([FromBody] InterimDeviceDetail interimDevice)
        {
            if (interimDevice == null) return BadRequest("Mandatory field(s) not supplied.");
            await _receivingService.UpdateInterimDeviceAsync(interimDevice);
            return Ok();
        }

        [HttpGet("canUndoReceiveLot")]
        public async Task<IActionResult> CanUndoReceiveLotAsync(int inventoryId)
        {
            var undoCheck = await _receivingService.CanUndoReceiveLotAsync(inventoryId);
            return Ok(undoCheck);
        }

        [HttpGet("isReceiptEditable")]
        public async Task<IActionResult> CheckingIsReceiptEditable(int receiptId, int loginId)
        {
            var isAllowed = await _receivingService.CheckingIsReceiptEditableAsync(receiptId, loginId);
            return Ok(isAllowed);
        }

        [HttpGet("GetReceiverFormInternal")]
        public async Task<IActionResult> GetReceiverFormInternal(string? status, bool? isExpected, DateTime? fromDate, DateTime? toDate)
        {
            var details = await _receivingService.GetReceiverFormInternalAsync(status, isExpected, fromDate, toDate);
            return Ok(details);
        }

        [HttpGet("searchCustomerReceiverForm")]
        public async Task<IActionResult> SearchCustomerReceiverForm(int? receivingInfoId, int? customerId, int? deviceFamilyId, int? deviceId, string? customerLotsStr, int? statusId, bool? isExpected, string? isElot, int? serviceCategoryId, int? locationId, string? mail, DateTime? fromDate, DateTime? toDate, string? receiptStatus, string? facilityIdStr)
        {
            var details = await _receivingService.SearchCustomerReceiverForm(receivingInfoId, customerId, deviceFamilyId, deviceId, customerLotsStr, statusId, isExpected, isElot, serviceCategoryId, locationId, mail, fromDate, toDate, receiptStatus, facilityIdStr);
            return Ok(details);
        }

        [HttpGet("getInventoryReceiptStatuses")]
        public async Task<IActionResult> GetInventoryReceiptStatusesAsync()
        {
            var statuseslist = await _receivingService.GetInventoryReceiptStatusesAsync();
            return Ok(statuseslist);
        }

        [HttpGet("getDeviceFamilies")]
        public async Task<IActionResult> GetDeviceFamilies([FromQuery] int customerId)
        {
            var deviceFamilies = await _receivingService.GetDeviceFamiliesAsync(customerId);
            return Ok(deviceFamilies);
        }

        [HttpGet("DeviceFamilies")]
        public async Task<IActionResult> DeviceFamilies([FromQuery] int customerId)
        {
            var deviceFamilies = await _receivingService.DeviceFamiliesAsync(customerId);
            return Ok(deviceFamilies);
        }

        [HttpGet("getDevices")]
        public async Task<IActionResult> GetDevices([FromQuery] int customerId, [FromQuery] int deviceFamilyId)
        {
            var devices = await _receivingService.GetDevicesAsync(customerId, deviceFamilyId);
            return Ok(devices);
        }

        [HttpGet("getInventoryReceiptServiceCategory")]
        public async Task<IActionResult> GetInventoryReceiptServiceCategory([FromQuery] string ListName)
        {
            var serviceCategory = await _receivingService.GetInventoryReceiptServiceCategoryAsync(ListName);
            return Ok(serviceCategory);
        }

        [HttpGet("getInventoryReceiptLotOwners")]
        public async Task<IActionResult> GetInventoryReceiptLotOwners()
        {
            var lotOwners = await _receivingService.GetInventoryReceiptLotOwnersAsync();
            return Ok(lotOwners);
        }

        [HttpGet("getInventoryReceiptTrayVendor")]
        public async Task<IActionResult> GetInventoryReceiptTrayVendor([FromQuery] int customerId)
        {
            try
            {
                var trayVendor = await _receivingService.GetInventoryReceiptTrayVendorAsync(customerId);
                return Ok(trayVendor);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving tray vendors.", Error = ex.Message, StackTrace = ex.StackTrace });
            }
        }

        [HttpGet("getInventoryReceiptTraysByVendorId")]
        public async Task<IActionResult> GetInventoryReceiptTraysByVendorId([FromQuery] int customerId, [FromQuery] int VendorId)
        {
            try
            {
                var trayPart = await _receivingService.GetInventoryReceiptTraysByVendorIdAsync(customerId, VendorId);
                return Ok(trayPart);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving tray parts.", Error = ex.Message, StackTrace = ex.StackTrace });
            }
        }

        [HttpGet("GetPurchaseOrders")]
        public async Task<IActionResult> GetPurchaseOrdersAsync([FromQuery] int customerId, int? divisionId, bool? isFreezed)
        {
            var purchaseOrders = await _receivingService.GetPurchaseOrdersAsync(customerId, divisionId, isFreezed);
            return Ok(purchaseOrders);
        }

        [HttpGet("GetPackageCategoryList")]
        public async Task<IActionResult> GetPackageCategoryAsync([FromQuery] string categoryName)
        {
            var list = await _receivingService.GetPackageCategoryAsync(categoryName);
            return Ok(list);
        }

        [HttpGet("getQuotes")]
        public async Task<IActionResult> GetQuotes([FromQuery] int customerId)
        {
            try
            {
                var quotes = await _receivingService.GetQuotesAsync(customerId);
                return Ok(quotes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving quotes.", Error = ex.Message, StackTrace = ex.StackTrace });
            }
        }

        [HttpGet("getServiceCaetgory")]
        public async Task<IActionResult> GetServiceCaetgory()
        {
            var details = await _receivingService.GetServiceCaetgoryAsync();
            return Ok(details);
        }

        [HttpGet("GetMailRoomStatusList")]
        public async Task<IActionResult> GetMailRoomStatusListAsync([FromQuery] string listName)
        {
            var list = await _receivingService.GetMailRoomStatusListAsync(listName);
            return Ok(list);
        }

        [HttpGet("GetMailRoomSearchData")]
        public async Task<IActionResult> SearchMailReceiptAsync([FromQuery] string? status, DateTime? fromDate, DateTime? toDate)
        {
            var list = await _receivingService.SearchMailReceiptAsync(status, fromDate, toDate);
            return Ok(list);
        }

        [HttpGet("getMailRoomDetails")]
        public async Task<IActionResult> GetMailRoomDetailsAsync([FromQuery] int mailId)
        {
            var list = await _receivingService.GetMailRoomDetailsAsync(mailId);
            return Ok(list);
        }

        [HttpGet("getReceiverFormInternalList")]
        public async Task<IActionResult> GetReceiverFormInternalList([FromQuery] int? receiptId, [FromQuery] int? mailId)
        {
            var receiptlist = await _receivingService.GetReceiverFormInternalListAsync(receiptId, mailId);
            return Ok(receiptlist);
        }

        [HttpGet("GetReceivingSearchData")]
        public async Task<IActionResult> SearchReceivingAsync(string? receivingTypes, DateTime? fromDate, DateTime? toDate, string? statusIds)
        {
            var list = await _receivingService.SearchReceivingAsync(receivingTypes, fromDate, toDate, statusIds);
            return Ok(list);
        }

        [HttpPost("processReceivingData")]
        public async Task<IActionResult> SaveReceivingAsync([FromBody] ReceivingRequest requestWrapper)
        {
            await _receivingService.SaveReceivingAsync(requestWrapper.Jsondata, requestWrapper.ReceiptId, requestWrapper.LoginId);
            return Ok();
        }

        [HttpGet("getCustomersLoginIdAsync")]
        public async Task<IActionResult> GetCustomersLoginId([FromQuery] int loginId)
        {
            var customerLogins = await _receivingService.GetCustomersLoginIdAsync(loginId);
            return Ok(customerLogins);
        }

        [HttpGet("GetReceivingById")]
        public async Task<IActionResult> GetReceivingById(int receiptId)
        {
            if (receiptId <= 0) return BadRequest("Invalid Receipt ID");
            var result = await _receivingService.GetReceivingByIdAsync(receiptId);
            if (result == null) return NotFound($"No Receiving details found for Receipt ID {receiptId}");
            return Ok(result);
        }

        [HttpGet("getSearchInterimCustomerIdAsync")]
        public async Task<IActionResult> GetSearchInterimCustomerId([FromQuery] int receiptId, int customerId)
        {
            var list = await _receivingService.GetSearchInterimCustomerIdAsync(receiptId, customerId);
            return Ok(list);
        }

        [HttpPost("save-inventory-receipt")]
        public async Task<IActionResult> SaveInventoryReceipt([FromForm] List<IFormFile>? receiptFiles, [FromForm] int receiptId, [FromForm] int loginId, [FromForm] string receiptJson, [FromForm] string? deletedAttachmentsJson)
        {
            string uploadedFiles = string.Empty;
            BlobContainerClient containerClient = new BlobContainerClient(_blobConnectionString, _blobContainerName);

            if (receiptFiles != null && receiptFiles.Count > 0)
            {
                await containerClient.CreateIfNotExistsAsync();
                var fileNames = new List<string>();

                foreach (var file in receiptFiles)
                {
                    if (file != null && file.Length > 0)
                    {
                        string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                        string uploadFilename = $"{receiptId}-{Path.GetFileNameWithoutExtension(file.FileName)}-{timestamp}{Path.GetExtension(file.FileName)}";
                        BlobClient blobClient = containerClient.GetBlobClient(uploadFilename);

                        using (var stream = file.OpenReadStream())
                        {
                            await blobClient.UploadAsync(stream, true);
                        }

                        fileNames.Add(uploadFilename);
                    }
                }

                uploadedFiles = string.Join(",", fileNames);
            }

            var (returnCode, returnMessage) = await _receivingService.SaveInventoryReceiptAsync(receiptId, loginId, receiptJson, deletedAttachmentsJson ?? string.Empty);
            
            if (returnCode == 0)
            {
                return Ok(new { Message = returnMessage, Files = uploadedFiles });
            }
            return BadRequest(new { Message = returnMessage });
        }

        [HttpPost("savemailroominfo")]
        public async Task<IActionResult> SaveMailRoomInfo([FromForm] List<IFormFile>? packageLabelFiles, [FromForm] List<IFormFile>? shipmentPaperFiles, [FromForm] int mailId, [FromForm] int loginId, [FromForm] string mailJson, [FromForm] string? deletedAttachmentsJson)
        {
            string packageLabelFilesJson = string.Empty;
            string shipmentPaperFilesJson = string.Empty;
            BlobContainerClient containerClient = new BlobContainerClient(_blobConnectionString, _blobContainerName);

            if (packageLabelFiles != null && packageLabelFiles.Count > 0)
            {
                await containerClient.CreateIfNotExistsAsync();
                var fileNames = new List<string>();

                foreach (var file in packageLabelFiles)
                {
                    if (file != null && file.Length > 0)
                    {
                        string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                        string uploadFilename = $"{mailId}-PackageLabel-{Path.GetFileNameWithoutExtension(file.FileName)}-{timestamp}{Path.GetExtension(file.FileName)}";
                        BlobClient blobClient = containerClient.GetBlobClient(uploadFilename);

                        using (var stream = file.OpenReadStream())
                        {
                            await blobClient.UploadAsync(stream, true);
                        }

                        fileNames.Add(uploadFilename);
                    }
                }

                packageLabelFilesJson = string.Join(",", fileNames);
            }

            if (shipmentPaperFiles != null && shipmentPaperFiles.Count > 0)
            {
                await containerClient.CreateIfNotExistsAsync();
                var fileNames = new List<string>();

                foreach (var file in shipmentPaperFiles)
                {
                    if (file != null && file.Length > 0)
                    {
                        string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                        string uploadFilename = $"{mailId}-ShipmentPaper-{Path.GetFileNameWithoutExtension(file.FileName)}-{timestamp}{Path.GetExtension(file.FileName)}";
                        BlobClient blobClient = containerClient.GetBlobClient(uploadFilename);

                        using (var stream = file.OpenReadStream())
                        {
                            await blobClient.UploadAsync(stream, true);
                        }

                        fileNames.Add(uploadFilename);
                    }
                }

                shipmentPaperFilesJson = string.Join(",", fileNames);
            }

            var (returnCode, returnMessage) = await _receivingService.SaveMailRoomInfoAsync(mailId, loginId, mailJson, packageLabelFilesJson, shipmentPaperFilesJson);
            
            if (returnCode == 0)
            {
                return Ok(new { Message = returnMessage });
            }
            return BadRequest(new { Message = returnMessage });
        }

        [HttpPost("validatemailroom")]
        public async Task<IActionResult> ValidateMailRoom([FromBody] MailInfoRequestWrapper request)
        {
            if (request == null || string.IsNullOrEmpty(request.MailJson))
            {
                return BadRequest(new { Message = "MailJson is required." });
            }

            var (returnCode, returnMessage) = await _receivingService.ValidateMailRoomInfoAsync(request.MailRoomId, request.MailJson);
            
            if (returnCode == 0)
            {
                return Ok(new { Message = returnMessage });
            }
            return BadRequest(new { Message = returnMessage });
        }
    }
}

