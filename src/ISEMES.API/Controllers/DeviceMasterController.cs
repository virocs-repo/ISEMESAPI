using ISEMES.Models;
using ISEMES.Services;
using ISEMES.API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json.Serialization;

namespace ISEMES.API.Controllers
{
    [Route("api/v1/ise/devicemaster")]
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
        public async Task<IActionResult> AddUpdateDevice([FromBody] DeviceMasterRequestDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Handle PartTypeId conversion (frontend may send partType as string or partTypeId as int)
                int? partTypeId = dto.PartTypeId;
                if (!partTypeId.HasValue && dto.PartType != null)
                {
                    if (dto.PartType is int intValue)
                        partTypeId = intValue;
                    else if (dto.PartType is string strValue && !string.IsNullOrWhiteSpace(strValue) && int.TryParse(strValue, out int parsed))
                        partTypeId = parsed;
                    // If partType is a string that's not a number, leave it as null
                }

                // Handle DeviceTypeId conversion (frontend sends lotType as string, need to convert to DeviceTypeId int)
                // Note: This may need to lookup the ID from the lotType string value
                // For now, if DeviceTypeId is provided, use it; otherwise try to parse from LotType
                int? deviceTypeId = dto.DeviceTypeId;
                if (!deviceTypeId.HasValue && dto.LotType != null)
                {
                    if (dto.LotType is int intValue)
                        deviceTypeId = intValue;
                    else if (dto.LotType is string strValue && !string.IsNullOrWhiteSpace(strValue) && int.TryParse(strValue, out int parsed))
                        deviceTypeId = parsed;
                    // If lotType is a string like "Standard", we'd need to lookup the ID
                    // For now, set to null if it's not a number
                }

                // Convert TrayTubeMapping string to IsDeviceBasedTray bool
                bool isDeviceBasedTray = dto.IsDeviceBasedTray;
                if (!string.IsNullOrEmpty(dto.TrayTubeMapping))
                {
                    isDeviceBasedTray = dto.TrayTubeMapping.Equals("Device", StringComparison.OrdinalIgnoreCase);
                }

                // Convert DTO to model and handle alias names conversion
                var request = new DeviceMasterRequest
                {
                    DeviceId = dto.DeviceId,
                    DeviceName = dto.DeviceName ?? string.Empty,
                    DeviceFamilyId = dto.DeviceFamilyId,
                    CustomerID = dto.CustomerID,
                    IsActive = dto.IsActive,
                    TestDevice = dto.TestDevice,
                    ReliabilityDevice = dto.ReliabilityDevice,
                    SKU = dto.SKU,
                    PartTypeId = partTypeId,
                    DeviceTypeId = deviceTypeId,
                    LabelMapping = dto.LabelMapping,
                    IsDeviceBasedTray = isDeviceBasedTray,
                    CountryOfOriginId = dto.CountryOfOriginId,
                    UnitCost = dto.UnitCost,
                    MaterialDescriptionId = dto.MaterialDescriptionId,
                    USHTSCodeId = dto.USHTSCodeId,
                    ECCNId = dto.ECCNId,
                    LicenseExceptionId = dto.LicenseExceptionId,
                    RestrictedCountriesToShipId = dto.RestrictedCountriesToShipId,
                    ScheduleB = dto.ScheduleB,
                    MSLId = dto.MSLId,
                    PeakPackageBodyTemperatureId = dto.PeakPackageBodyTemperatureId,
                    ShelfLifeMonthId = dto.ShelfLifeMonthId,
                    FloorLifeId = dto.FloorLifeId,
                    PBFreeId = dto.PBFreeId,
                    PBFreeStickerId = dto.PBFreeStickerId,
                    ROHSId = dto.ROHSId,
                    TrayTubeStrappingId = dto.TrayTubeStrappingId,
                    TrayStackingId = dto.TrayStackingId,
                    LockId = dto.LockId ?? -1,
                    LastModifiedOn = dto.LastModifiedOn,
                    Labels = dto.Labels,
                    lstLabelDetails = dto.LstLabelDetails
                };

                // Convert alias name strings to DeviceAliasName objects
                if (dto.AliasNames != null && dto.AliasNames.Count > 0)
                {
                    request.AliasNames = dto.AliasNames
                        .Where(a => !string.IsNullOrWhiteSpace(a))
                        .Select(aliasName => new DeviceAliasName
                        {
                            DeviceId = request.DeviceId,
                            AliasId = 0, // New alias, will be assigned by stored procedure
                            AliasName = aliasName.Trim(),
                            DeviceFamilyId = request.DeviceFamilyId
                        })
                        .ToList();
                }

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

    // DTO to accept alias names as strings from frontend and handle field name differences
    public class DeviceMasterRequestDto
    {
        public int DeviceId { get; set; } = -1;
        public string DeviceName { get; set; } = string.Empty;
        public int DeviceFamilyId { get; set; }
        public int CustomerID { get; set; }
        public bool IsActive { get; set; } = true;
        public string? TestDevice { get; set; }
        public string? ReliabilityDevice { get; set; }
        public List<string>? AliasNames { get; set; } // Accept as strings from frontend
        public string? SKU { get; set; }
        // Frontend may send partType as string or partTypeId as int
        public object? PartType { get; set; }
        public int? PartTypeId { get; set; }
        // Frontend may send lotType as string or deviceTypeId as int  
        public object? LotType { get; set; }
        public int? DeviceTypeId { get; set; }
        public bool LabelMapping { get; set; }
        public bool IsDeviceBasedTray { get; set; }
        public string? TrayTubeMapping { get; set; } // Frontend sends this, convert to IsDeviceBasedTray
        public int? CountryOfOriginId { get; set; }
        public decimal? UnitCost { get; set; }
        public int? MaterialDescriptionId { get; set; }
        [JsonPropertyName("usHtsCodeId")]
        public int? USHTSCodeId { get; set; }
        public int? ECCNId { get; set; }
        public int? LicenseExceptionId { get; set; }
        public string? RestrictedCountriesToShipId { get; set; }
        public bool ScheduleB { get; set; }
        public int? MSLId { get; set; }
        public int? PeakPackageBodyTemperatureId { get; set; }
        public int? ShelfLifeMonthId { get; set; }
        public int? FloorLifeId { get; set; }
        public int? PBFreeId { get; set; }
        public int? PBFreeStickerId { get; set; }
        public int? ROHSId { get; set; }
        public int? TrayTubeStrappingId { get; set; }
        public int? TrayStackingId { get; set; }
        public int? LockId { get; set; } = -1;
        public string? LastModifiedOn { get; set; }
        public string? Labels { get; set; }
        public List<LabelInfo>? LstLabelDetails { get; set; }
    }
}
