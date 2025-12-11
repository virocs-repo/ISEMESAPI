using ISEMES.Models;
using ISEMES.Repositories;

namespace ISEMES.Services
{
    public class CustomersOrderService : ICustomersOrderService
    {
        private readonly ICustomerOrderRepository _repository;

        public CustomersOrderService(ICustomerOrderRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<CustOrder>> GetCustomerOrdersAsync(DateTime? fromDate = null, DateTime? toDate = null)
        {
            return await _repository.GetCustomerOrdersAsync(fromDate, toDate);
        }

        public async Task<IEnumerable<CustomerInventory>> GetCustomerInventoryAsync(string goodsType, string lotNumber, string customerOrderType, int? customerId = null)
        {
            return await _repository.GetCustomerInventoryAsync(goodsType, lotNumber, customerOrderType, customerId);
        }

        public async Task<bool> ProcessCustomerOrderAsync(int loginId, OrderRequestWrapper wrapper)
        {
            return await _repository.ProcessCustomerOrderAsync(loginId, wrapper);
        }

        public async Task<List<CustomerEditDetail>> GetVieweditorder(int customerOrderID, bool editdata)
        {
            var customerOrderDetails = await _repository.GetVieweditorder(customerOrderID);

            if (editdata)
            {
                return customerOrderDetails;
            }
            else
            {
                return customerOrderDetails.Where(detail => detail.CustomerOrderID.HasValue).ToList();
            }
        }

        public async Task<IEnumerable<string>> GetInventoryLots(int? customerId = null)
        {
            return await _repository.GetInventoryLots(customerId);
        }

        public async Task<IEnumerable<MasterListItem>> GetMasterListItemsAsync(string listName, int? serviceId)
        {
            return await _repository.GetMasterListItemsAsync(listName, serviceId);
        }

        public async Task<IEnumerable<ShippingAddress>> GetShippingAddressesAsync(int customerId, bool isBilling, int? vendorId, int? courierId, bool isDomestic)
        {
            return await _repository.GetShippingAddressesAsync(customerId, isBilling, vendorId, courierId, isDomestic);
        }

        public async Task<IEnumerable<MasterListItem>> GetListItemsAsync(string listName, int? parentId)
        {
            return await _repository.GetListItemsAsync(listName, parentId);
        }

        public async Task<IEnumerable<ContactPersonDetails>> GetContactPersonDetailsAsync(int customerId, int? shippingContactId)
        {
            return await _repository.GetContactPersonDetailsAsync(customerId, shippingContactId);
        }
    }
}

