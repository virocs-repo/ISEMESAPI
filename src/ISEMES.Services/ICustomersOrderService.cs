using ISEMES.Models;

namespace ISEMES.Services
{
    public interface ICustomersOrderService
    {
        Task<IEnumerable<CustOrder>> GetCustomerOrdersAsync(DateTime? fromDate = null, DateTime? toDate = null);
        Task<IEnumerable<CustomerInventory>> GetCustomerInventoryAsync(string goodsType, string lotNumber, string customerOrderType, int? customerId = null);
        Task<bool> ProcessCustomerOrderAsync(int loginId, OrderRequestWrapper wrapper);
        Task<List<CustomerEditDetail>> GetVieweditorder(int customerOrderID, bool editdata);
        Task<IEnumerable<string>> GetInventoryLots(int? customerId = null);
        Task<IEnumerable<MasterListItem>> GetMasterListItemsAsync(string listName, int? serviceId);
        Task<IEnumerable<MasterListItem>> GetListItemsAsync(string listName, int? parentId);
        Task<IEnumerable<ShippingAddress>> GetShippingAddressesAsync(int customerId, bool isBilling, int? vendorId, int? courierId, bool isDomestic);
        Task<IEnumerable<ContactPersonDetails>> GetContactPersonDetailsAsync(int customerId, int? shippingContactId);
    }
}



