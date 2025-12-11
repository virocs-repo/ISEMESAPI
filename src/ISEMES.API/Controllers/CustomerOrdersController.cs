using ISEMES.Models;
using ISEMES.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ISEMES.API.Controllers
{
    [Route("api/v1/ise/inventory/customerorder")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "inventory")]
    public class CustomerOrdersController : ControllerBase
    {
        private readonly ICustomersOrderService _OrderService;

        public CustomerOrdersController(ICustomersOrderService OrderService)
        {
            _OrderService = OrderService;
        }

        [HttpGet("all")]
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(typeof(string), 401)]
        [ProducesResponseType(typeof(string), 403)]
        [ProducesResponseType(typeof(void), 404)]
        public async Task<IActionResult> GetCustomerOrders([FromQuery] DateTime? fromDate = null, [FromQuery] DateTime? toDate = null)
        {
            try
            {
                var customerOrders = await _OrderService.GetCustomerOrdersAsync(fromDate, toDate);
                return Ok(customerOrders);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        [HttpGet("inventory")]
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(typeof(string), 401)]
        [ProducesResponseType(typeof(string), 403)]
        [ProducesResponseType(typeof(void), 404)]
        public async Task<IActionResult> GetCustomerInventory([FromQuery] string goodsType, [FromQuery] string? lotNumber, [FromQuery] string? customerOrderType, [FromQuery] int? customerId = null)
        {
            try
            {
                var inventory = await _OrderService.GetCustomerInventoryAsync(goodsType, lotNumber, customerOrderType, customerId);
                return Ok(inventory);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        [HttpPost("addcustomerorder")]
        public async Task<IActionResult> ProcessCustomerOrder([FromBody] OrderRequestWrapper requestWrapper)
        {
            try
            {
                int loginId = requestWrapper.LoginId;
                var orderRequest = JsonConvert.DeserializeObject<OrderRequest>(requestWrapper.InputJSON);

                if (orderRequest == null || !orderRequest.CustomerOrder.Any())
                {
                    return BadRequest("Invalid order data");
                }

                bool result = await _OrderService.ProcessCustomerOrderAsync(loginId, requestWrapper);

                if (result)
                {
                    return Ok();
                }
                else
                {
                    return StatusCode(500, "Error processing the order.");
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        [HttpGet("vieweditorder")]
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(typeof(string), 401)]
        [ProducesResponseType(typeof(string), 403)]
        [ProducesResponseType(typeof(void), 404)]
        public async Task<IActionResult> GetVieweditorder([FromQuery] int customerOrderID, [FromQuery] bool editdata)
        {
            try
            {
                var inventory = await _OrderService.GetVieweditorder(customerOrderID, editdata);
                return Ok(inventory);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        [HttpGet("invlotnums")]
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(typeof(string), 401)]
        [ProducesResponseType(typeof(string), 403)]
        [ProducesResponseType(typeof(void), 404)]
        public async Task<IActionResult> GetInventoryLots([FromQuery] int? customerId = null)
        {
            try
            {
                var customerOrders = await _OrderService.GetInventoryLots(customerId);
                return Ok(customerOrders);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        [HttpGet("getMasterListItems")]
        [ProducesResponseType(typeof(IEnumerable<MasterListItem>), 200)]
        [ProducesResponseType(typeof(string), 401)]
        [ProducesResponseType(typeof(string), 403)]
        [ProducesResponseType(typeof(void), 404)]
        public async Task<IActionResult> GetMasterListItems([FromQuery] string listName, [FromQuery] int? serviceId = null)
        {
            try
            {
                var result = await _OrderService.GetMasterListItemsAsync(listName, serviceId);
                if (result == null || !result.Any())
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        [HttpGet("getListItems")]
        [ProducesResponseType(typeof(IEnumerable<MasterListItem>), 200)]
        [ProducesResponseType(typeof(string), 401)]
        [ProducesResponseType(typeof(string), 403)]
        [ProducesResponseType(typeof(void), 404)]
        public async Task<IActionResult> GetListItems([FromQuery] string listName, [FromQuery] int? parentId = null)
        {
            try
            {
                var result = await _OrderService.GetListItemsAsync(listName, parentId);
                if (result == null || !result.Any())
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        [HttpGet("getShippingAddresses")]
        [ProducesResponseType(typeof(IEnumerable<ShippingAddress>), 200)]
        [ProducesResponseType(typeof(string), 401)]
        [ProducesResponseType(typeof(string), 403)]
        [ProducesResponseType(typeof(void), 404)]
        public async Task<IActionResult> GetShippingAddresses([FromQuery] int customerId, [FromQuery] bool isBilling = false, [FromQuery] int? vendorId = null, [FromQuery] int? courierId = null, [FromQuery] bool isDomestic = false)
        {
            try
            {
                var addresses = await _OrderService.GetShippingAddressesAsync(customerId, isBilling, vendorId, courierId, isDomestic);
                if (addresses == null || !addresses.Any())
                {
                    return NotFound();
                }
                return Ok(addresses);
            }
            catch (Exception ex)
            {
                // Log the full exception details for debugging
                var errorMessage = $"Internal server error: {ex.Message}";
                if (ex.InnerException != null)
                {
                    errorMessage += $" Inner exception: {ex.InnerException.Message}";
                }
                return StatusCode(500, new { Message = errorMessage, StackTrace = ex.StackTrace });
            }
        }

        [HttpGet("getContactPersonDetails")]
        [ProducesResponseType(typeof(IEnumerable<ContactPersonDetails>), 200)]
        [ProducesResponseType(typeof(string), 401)]
        [ProducesResponseType(typeof(string), 403)]
        [ProducesResponseType(typeof(void), 404)]
        public async Task<IActionResult> GetContactPersonDetailsAsync([FromQuery] int customerId, [FromQuery] int? shippingContactId = null)
        {
            try
            {
                var contactPersonDetails = await _OrderService.GetContactPersonDetailsAsync(customerId, shippingContactId);
                if (contactPersonDetails == null || !contactPersonDetails.Any())
                {
                    return NotFound();
                }
                return Ok(contactPersonDetails);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}

