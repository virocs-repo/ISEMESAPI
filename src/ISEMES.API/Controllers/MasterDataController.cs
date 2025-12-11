using ISEMES.Models;
using ISEMES.Services;
using ISEMES.API.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ISEMES.API.Controllers
{
    [Route("api/v1/ise/inventory/")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "inventory")]
    public class MasterDataController : ControllerBase
    {
        private readonly TokenProvider _tokenProvider;
        private readonly IMasterDataService _masterDataService;
        private readonly IConfiguration _configuration;

        public MasterDataController(TokenProvider tokenProvider, IMasterDataService masterDataService, IConfiguration configuration)
        {
            _tokenProvider = tokenProvider;
            _masterDataService = masterDataService;
            _configuration = configuration;
        }

        [HttpGet("masterdata")]
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(typeof(string), 401)]
        [ProducesResponseType(typeof(string), 403)]
        [ProducesResponseType(typeof(void), 404)]
        public IActionResult MasterData()
        {
            try
            {
                var masterData = _masterDataService.GetMasterData();
                return Ok(JsonConvert.DeserializeObject<MasterData>(masterData.Result[0].ToString()));
            }
            catch (Exception ex)
            {
                throw (ex);                
            }            
        }

        [HttpGet("entity/{entityType}")]
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(typeof(string), 401)]
        [ProducesResponseType(typeof(string), 403)]
        [ProducesResponseType(typeof(void), 404)]
        public IActionResult EntityDetail(string entityType)
        {
            try
            {
                var entity = _masterDataService.GetEntityDetailByType(entityType);
                string jsonResult = JsonConvert.SerializeObject(entity);
                return Ok(jsonResult);                
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        [HttpGet("address")]
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(typeof(string), 401)]
        [ProducesResponseType(typeof(string), 403)]
        [ProducesResponseType(typeof(void), 404)]
        public async Task<IActionResult> AddressData()
        {
            try
            {
                var address = await _masterDataService.GetAddressData();
                return Ok(address);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        [HttpGet("hardwareType")]
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(typeof(string), 401)]
        [ProducesResponseType(typeof(string), 403)]
        [ProducesResponseType(typeof(void), 404)]
        public async Task<IActionResult> HardwareType()
        {
            try
            {
                var address = await _masterDataService.GetHardwareTypeData();
                return Ok(address);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        [HttpGet("getInvUserByRole")]
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(typeof(string), 401)]
        [ProducesResponseType(typeof(string), 403)]
        [ProducesResponseType(typeof(void), 404)]
        public async Task<IActionResult> GetInvUserByRoleAsync(string filterKey, int isActive, string condition)
        {
            try
            {
                var address = await _masterDataService.GetInvUserByRoleAsync(filterKey, isActive, condition);
                return Ok(address);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
    }
}

