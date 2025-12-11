using ISEMES.Models;
using ISEMES.Services;
using ISEMES.API.Services;
using Microsoft.AspNetCore.Mvc;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using Shippo;
using Shippo.Models.Components;
using Shippo.Models.Requests;
using System.Net;

namespace ISEMES.API.Controllers
{
    [Route("api/v1/ise/shipment/")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "inventory")]
    public class ShipmentController : ControllerBase
    {
        private readonly TokenProvider _tokenProvider;
        private readonly IShipmentService _receivingService;
        private readonly IConfiguration _configuration;
        private readonly ShippoSDK _shippoSdk;

        public ShipmentController(TokenProvider tokenProvider, IShipmentService receivingService, IConfiguration configuration)
        {
            _tokenProvider = tokenProvider;
            _receivingService = receivingService;
            _configuration = configuration;
            _shippoSdk = new ShippoSDK(apiKeyHeader: _configuration["ShippoSDK:apiKeyHeader"]);
        }

        [HttpGet("shipmentdata")]
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(typeof(string), 401)]
        [ProducesResponseType(typeof(string), 403)]
        [ProducesResponseType(typeof(void), 404)]
        public async Task<IActionResult> GetShipmentdata(DateTime? fromDate, DateTime? toDate)
        {
            try
            {
                var receiptData = await _receivingService.ListShipmentAsync(fromDate, toDate);
                return Ok(receiptData);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        [HttpGet("shipmentcategory")]
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(typeof(string), 401)]
        [ProducesResponseType(typeof(string), 403)]
        [ProducesResponseType(typeof(void), 404)]
        public async Task<IActionResult> GetShipmentCategory()
        {
            try
            {
                var receiptData = await _receivingService.ListShipmentCategoryAsync();
                return Ok(receiptData);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        [HttpGet("shipmenttypes")]
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(typeof(string), 401)]
        [ProducesResponseType(typeof(string), 403)]
        [ProducesResponseType(typeof(void), 404)]
        public async Task<IActionResult> GetShipmenttypes()
        {
            try
            {
                var receiptData = await _receivingService.ListShipmentType();
                return Ok(receiptData);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        [HttpGet("shipmentLineItem")]
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(typeof(string), 401)]
        [ProducesResponseType(typeof(string), 403)]
        [ProducesResponseType(typeof(void), 404)]
        public async Task<IActionResult> GetShipmentLineItem(int shipmentID)
        {
            try
            {
                var receiptData = await _receivingService.GetShipmentLineItem(shipmentID);
                return Ok(receiptData);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        [HttpGet("shipmentInventory")]
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(typeof(string), 401)]
        [ProducesResponseType(typeof(string), 403)]
        [ProducesResponseType(typeof(void), 404)]
        public async Task<IActionResult> GetShipmentInventory(int customerID)
        {
            try
            {
                var receiptData = await _receivingService.GetShipmentInventory(customerID);
                return Ok(receiptData);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        [HttpPost("processShipment")]
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(typeof(string), 401)]
        [ProducesResponseType(typeof(string), 403)]
        [ProducesResponseType(typeof(void), 404)]
        public async Task<IActionResult> ProcessShipment([FromBody] ShipmentRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest("Mandatory field(s) not supplied.");
                }
                await _receivingService.UpdateShipment(request);
                return Ok();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        [HttpPost("create-shipment")]
        public async Task<IActionResult> CreateShipment([FromBody] CreateShipmentLabelRequest request)
        {
            try
            {
                AddressFrom addressFrom = AddressFrom.CreateAddressCreateRequest(new AddressCreateRequest
                {
                    Name = _configuration["ShippoSDK:FromName"],
                    Street1 = _configuration["ShippoSDK:FromStreet1"],
                    City = _configuration["ShippoSDK:FromCity"],
                    State = _configuration["ShippoSDK:FromState"],
                    Zip = _configuration["ShippoSDK:FromZip"],
                    Country = _configuration["ShippoSDK:FromCountry"],
                    Email = _configuration["ShippoSDK:FromEmail"],
                    Phone = _configuration["ShippoSDK:FromPhone"]
                });

                AddressTo addressTo = AddressTo.CreateAddressCreateRequest(new AddressCreateRequest
                {
                    Name = request.ToName,
                    Street1 = request.ToStreet1,
                    City = request.ToCity,
                    State = request.ToState,
                    Zip = request.ToZip,
                    Country = request.ToCountry
                });

                List<string> labelUrls = new List<string>();
                List<string> trackingNumbers = new List<string>();
                ShippmentLabel shippmentLabel = new ShippmentLabel();

                foreach (var parcelRequest in request.Parcels)
                {
                    Shippo.Models.Components.Parcels parcel = Shippo.Models.Components.Parcels.CreateParcelCreateRequest(new ParcelCreateRequest
                    {
                        Length = Convert.ToString(parcelRequest.Length),
                        Width = Convert.ToString(parcelRequest.Width),
                        Height = Convert.ToString(parcelRequest.Height),
                        DistanceUnit = DistanceUnitEnum.In,
                        Weight = Convert.ToString(parcelRequest.Weight),
                        MassUnit = WeightUnitEnum.Lb
                    });

                    Shippo.Models.Components.Shipment shipment = await _shippoSdk.Shipments.CreateAsync(new ShipmentCreateRequest
                    {
                        AddressFrom = addressFrom,
                        AddressTo = addressTo,
                        Parcels = new List<Shippo.Models.Components.Parcels> { parcel },
                        Async = false,
                        CarrierAccounts = !string.IsNullOrEmpty(request.ClientAccountNumber) ? new List<string> { request.ClientAccountNumber } : null
                    });

                    if (shipment.Rates.Count == 0)
                    {
                        return BadRequest("No rates available for this shipment.");
                    }

                    Rate selectedRate = shipment.Rates[0];

                    Transaction transaction = await _shippoSdk.Transactions.CreateAsync(CreateTransactionRequestBody.CreateTransactionCreateRequest(new TransactionCreateRequest
                    {
                        Rate = selectedRate.ObjectId,
                        LabelFileType = LabelFileTypeEnum.Pdf,
                        Async = false,
                        Metadata = $"Package {parcelRequest.Pno}",
                    }));

                    if (transaction.Status == TransactionStatusEnum.Success)
                    {
                        labelUrls.Add(transaction.LabelUrl);
                        trackingNumbers.Add(transaction.TrackingNumber);
                    }
                }

                if (labelUrls.Count > 0)
                {
                    shippmentLabel.Trackingurl = string.Join(";;", labelUrls);
                    shippmentLabel.Trackingnum = string.Join(";;", trackingNumbers);
                    shippmentLabel.ShipmentID = request.ShipmentID;
                    shippmentLabel.UserID = request.UserID;
                }

                await _receivingService.UpdateShipmentLabel(shippmentLabel);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        private async Task<string> MergeLabelsIntoSinglePDF(List<string> labelUrls)
        {
            string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "shipping_labels");

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            string mergedPdfPath = Path.Combine(directoryPath, $"Merged_Labels_{Guid.NewGuid()}.pdf");

            using (PdfDocument outputDocument = new PdfDocument())
            {
                foreach (var labelUrl in labelUrls)
                {
                    using (WebClient client = new WebClient())
                    {
                        byte[] pdfBytes = await client.DownloadDataTaskAsync(labelUrl);
                        using (MemoryStream stream = new MemoryStream(pdfBytes))
                        {
                            PdfDocument inputDocument = PdfReader.Open(stream, PdfDocumentOpenMode.Import);
                            foreach (PdfPage page in inputDocument.Pages)
                            {
                                outputDocument.AddPage(page);
                            }
                        }
                    }
                }
                outputDocument.Save(mergedPdfPath);
            }

            return mergedPdfPath;
        }
    }
}
