using ISEMES.Models;
using ISEMES.Services;
using ISEMES.API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ISEMES.API.Controllers
{
    [Route("api/v1/ise/devicemaster/")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "devicemaster")]
    public class DeviceMasterController : ControllerBase
    {
        private readonly TokenProvider _tokenProvider;
        private readonly IDeviceMasterService _deviceMasterService;

        public DeviceMasterController(TokenProvider tokenProvider, IDeviceMasterService deviceMasterService)
        {
            _tokenProvider = tokenProvider;
            _deviceMasterService = deviceMasterService;
        }

        [HttpPost("devicefamily")]
        [ProducesResponseType(typeof(int), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 401)]
        [ProducesResponseType(typeof(string), 403)]
        public async Task<IActionResult> AddUpdateDeviceFamily([FromBody] DeviceFamilyRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? 
                                 User.FindFirst("EmployeeId")?.Value;
                
                if (!string.IsNullOrEmpty(userIdClaim) && int.TryParse(userIdClaim, out int userId))
                {
                    request.CreatedBy = userId;
                }
                
                var result = await _deviceMasterService.AddUpdateDeviceFamily(request);
                
                if (result < 0)
                {
                    return BadRequest("Device family already exists or operation failed");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("device")]
        [ProducesResponseType(typeof(int), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 401)]
        [ProducesResponseType(typeof(string), 403)]
        public async Task<IActionResult> AddUpdateDevice([FromBody] DeviceMasterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? 
                                 User.FindFirst("EmployeeId")?.Value;
                
                if (!string.IsNullOrEmpty(userIdClaim) && int.TryParse(userIdClaim, out int userId))
                {
                    request.CreatedBy = userId;
                }
                
                var result = await _deviceMasterService.AddUpdateDevice(request);
                
                if (result < 0)
                {
                    return BadRequest("Device already exists or operation failed");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("devicefamily/search")]
        [ProducesResponseType(typeof(List<DeviceFamilyResponse>), 200)]
        [ProducesResponseType(typeof(string), 401)]
        [ProducesResponseType(typeof(string), 403)]
        public async Task<IActionResult> SearchDeviceFamily([FromQuery] int? customerID, [FromQuery] string? deviceFamilyName, [FromQuery] bool? active)
        {
            try
            {
                var result = await _deviceMasterService.SearchDeviceFamily(customerID, deviceFamilyName, active);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("device/search")]
        [ProducesResponseType(typeof(List<DeviceResponse>), 200)]
        [ProducesResponseType(typeof(string), 401)]
        [ProducesResponseType(typeof(string), 403)]
        public async Task<IActionResult> SearchDevice([FromQuery] int? customerID, [FromQuery] int? deviceFamilyId, [FromQuery] string? deviceName, [FromQuery] bool? active)
        {
            try
            {
                var result = await _deviceMasterService.SearchDevice(customerID, deviceFamilyId, deviceName, active);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("restrictedcountries")]
        [ProducesResponseType(typeof(List<Country>), 200)]
        [ProducesResponseType(typeof(string), 401)]
        [ProducesResponseType(typeof(string), 403)]
        public async Task<IActionResult> GetRestrictedCountries()
        {
            try
            {
                var countries = await _deviceMasterService.GetRestrictedCountries();
                return Ok(countries);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("customerlabels")]
        [ProducesResponseType(typeof(List<CustomerLabel>), 200)]
        [ProducesResponseType(typeof(string), 401)]
        [ProducesResponseType(typeof(string), 403)]
        public async Task<IActionResult> GetCustomerLabelList([FromQuery] string? customerName)
        {
            try
            {
                var result = await _deviceMasterService.GetCustomerLabelList(customerName ?? string.Empty);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("labeldetails")]
        [ProducesResponseType(typeof(LabelDetailsResponse), 200)]
        [ProducesResponseType(typeof(string), 401)]
        [ProducesResponseType(typeof(string), 403)]
        public async Task<IActionResult> GetLabelDetails([FromQuery] int? customerId, [FromQuery] int? deviceId, [FromQuery] string? labelName, [FromQuery] string? lotNum)
        {
            try
            {
                var result = await _deviceMasterService.GetLabelDetails(
                    customerId ?? 0, 
                    deviceId ?? 0, 
                    labelName ?? string.Empty, 
                    lotNum ?? string.Empty);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("labeldetails")]
        [ProducesResponseType(typeof(int), 200)]
        [ProducesResponseType(typeof(string), 401)]
        [ProducesResponseType(typeof(string), 403)]
        public async Task<IActionResult> AddOrUpdateLabelDetails([FromBody] LabelDetailsRequest request)
        {
            try
            {
                var result = await _deviceMasterService.AddOrUpdateLabelDetails(request.CustomerId, request.DeviceId, request.LabelName, request.Input);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("device/getDeviceInfo")]
        [ProducesResponseType(typeof(DeviceInfoResponse), 200)]
        [ProducesResponseType(typeof(string), 401)]
        [ProducesResponseType(typeof(string), 403)]
        public async Task<IActionResult> GetDeviceInfo([FromQuery] int deviceId)
        {
            if (deviceId <= 0)
            {
                return BadRequest("Device ID must be greater than 0");
            }

            try
            {
                var result = await _deviceMasterService.GetDeviceInfo(deviceId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }

    public class LabelDetailsRequest
    {
        public int CustomerId { get; set; }
        public int DeviceId { get; set; }
        public string LabelName { get; set; } = string.Empty;
        public string Input { get; set; } = string.Empty;
    }
}
