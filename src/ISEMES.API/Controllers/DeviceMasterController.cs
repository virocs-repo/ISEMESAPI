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
            try
            {
                // Try to get user ID from token claims (similar to how inventory endpoints work)
                // If not found, use the CreatedBy value from request (frontend sends 0)
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? 
                                 User.FindFirst("EmployeeId")?.Value;
                
                if (!string.IsNullOrEmpty(userIdClaim) && int.TryParse(userIdClaim, out int userId))
                {
                    request.CreatedBy = userId;
                }
                // If user ID not found in token, use CreatedBy from request (defaults to 0)
                // This matches how inventory/receiving endpoints work - they don't extract user ID
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
            try
            {
                // Try to get user ID from token claims (similar to how inventory endpoints work)
                // If not found, use the CreatedBy value from request (frontend sends 0)
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? 
                                 User.FindFirst("EmployeeId")?.Value;
                
                if (!string.IsNullOrEmpty(userIdClaim) && int.TryParse(userIdClaim, out int userId))
                {
                    request.CreatedBy = userId;
                }
                // If user ID not found in token, use CreatedBy from request (defaults to 0)
                // This matches how inventory/receiving endpoints work - they don't extract user ID
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
    }
}
