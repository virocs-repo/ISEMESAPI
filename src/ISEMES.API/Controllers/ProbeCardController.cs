using ISEMES.Models;
using ISEMES.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ISEMES.API.Controllers
{
    [Route("api/v1/ise/hardware/probecard")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "hardware")]
    public class ProbeCardController : ControllerBase
    {
        private readonly IProbeCardService _probeCardService;
        private readonly ILogger<ProbeCardController> _logger;

        public ProbeCardController(IProbeCardService probeCardService, ILogger<ProbeCardController> logger)
        {
            _probeCardService = probeCardService;
            _logger = logger;
        }

        [HttpGet("search")]
        [ProducesResponseType(typeof(List<ProbeCardResponse>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 401)]
        [ProducesResponseType(typeof(string), 403)]
        public async Task<IActionResult> SearchProbeCard([FromQuery] ProbeCardSearchRequest request)
        {
            try
            {
                // Set default HardwareTypeId if not provided (ProbeCard = 4)
                if (request.HardwareTypeId == 0)
                {
                    request.HardwareTypeId = 4;
                }

                var result = await _probeCardService.SearchProbeCard(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching probe cards");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("details")]
        [ProducesResponseType(typeof(ProbeCardDetailsResponse), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 401)]
        [ProducesResponseType(typeof(string), 403)]
        public async Task<IActionResult> GetProbeCardDetails([FromQuery] int masterId)
        {
            if (masterId <= 0)
            {
                return BadRequest("Master ID must be greater than 0");
            }

            try
            {
                var result = await _probeCardService.GetProbeCardDetails(masterId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting probe card details for MasterId: {MasterId}", masterId);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("masterdata/platforms")]
        [ProducesResponseType(typeof(List<MasterDataItem>), 200)]
        public async Task<IActionResult> GetPlatforms()
        {
            try
            {
                var result = await _probeCardService.GetPlatforms();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting platforms");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("masterdata/probecardtypes")]
        [ProducesResponseType(typeof(List<MasterDataItem>), 200)]
        public async Task<IActionResult> GetProbeCardTypes()
        {
            try
            {
                var result = await _probeCardService.GetProbeCardTypes();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting probe card types");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("masterdata/probers")]
        [ProducesResponseType(typeof(List<MasterDataItem>), 200)]
        public async Task<IActionResult> GetProbers()
        {
            try
            {
                var result = await _probeCardService.GetProbers();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting probers");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("masterdata/boardtypes")]
        [ProducesResponseType(typeof(List<MasterDataItem>), 200)]
        public async Task<IActionResult> GetBoardTypes()
        {
            try
            {
                var result = await _probeCardService.GetBoardTypes();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting board types");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("masterdata/slots")]
        [ProducesResponseType(typeof(List<SlotItem>), 200)]
        public async Task<IActionResult> GetSlots([FromQuery] int? hardwareTypeId, [FromQuery] string? platformId)
        {
            try
            {
                var result = await _probeCardService.GetSlots(hardwareTypeId, platformId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting slots");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("masterdata/slots/{slotId}/subslots")]
        [ProducesResponseType(typeof(List<SubSlotItem>), 200)]
        public async Task<IActionResult> GetSubSlots(int slotId)
        {
            try
            {
                var result = await _probeCardService.GetSubSlots(slotId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting subslots for slot {SlotId}", slotId);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("masterdata/subslots/{subSlotId}/locations")]
        [ProducesResponseType(typeof(List<LocationInfoItem>), 200)]
        public async Task<IActionResult> GetLocationsInfo(int subSlotId)
        {
            try
            {
                var result = await _probeCardService.GetLocationsInfo(subSlotId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting locations info for subslot {SubSlotId}", subSlotId);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
