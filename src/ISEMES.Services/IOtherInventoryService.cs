using ISEMES.Models;

namespace ISEMES.Services
{
    public interface IOtherInventoryService
    {
        Task<List<KeyValueData>> GetOtherInventoryStatusAsync();
        Task<List<AnotherShipment>> GetOtherInventoryShipmentsAsync(int? customerId, int? employeeId, int? statusId, DateTime? fromDate, DateTime? toDate);
        Task<AnotherShipmentDetail> GetOtherInventoryShipmentAsync(int anotherShippingId);
        Task<int> UpsertAntherInventoryShipmentAsync(string anotherShipJson);
        Task<List<KeyValueData>> GetServiceTypesAsync();
        Task<int> VoidAnotherShippingAsync(int anotherShippingID);
        Task<List<KeyValueData>> GetAnotherInventoryLots(int customerTypeId, int customerVendorId);
    }
}

