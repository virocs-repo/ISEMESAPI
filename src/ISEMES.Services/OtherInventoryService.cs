using ISEMES.Models;
using ISEMES.Repositories;

namespace ISEMES.Services
{
    public class OtherInventoryService : IOtherInventoryService
    {
        private readonly IOtherInventoryRepository _repository;

        public OtherInventoryService(IOtherInventoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<KeyValueData>> GetOtherInventoryStatusAsync()
        {
            return await _repository.GetOtherInventoryStatusAsync();
        }

        public async Task<List<AnotherShipment>> GetOtherInventoryShipmentsAsync(int? customerId, int? employeeId, int? statusId, DateTime? fromDate, DateTime? toDate)
        {
            return await _repository.GetOtherInventoryShipmentsAsync(customerId, employeeId, statusId, fromDate, toDate);
        }

        public async Task<AnotherShipmentDetail> GetOtherInventoryShipmentAsync(int anotherShippingId)
        {
            return await _repository.GetOtherInventoryShipmentAsync(anotherShippingId);
        }

        public async Task<int> UpsertAntherInventoryShipmentAsync(string anotherShipJson)
        {
            return await _repository.UpsertAntherInventoryShipmentAsync(anotherShipJson);
        }

        public async Task<List<KeyValueData>> GetServiceTypesAsync()
        {
            return await _repository.GetServiceTypesAsync();
        }

        public async Task<int> VoidAnotherShippingAsync(int anotherShippingID)
        {
            return await _repository.VoidAnotherShippingAsync(anotherShippingID);
        }

        public async Task<List<KeyValueData>> GetAnotherInventoryLots(int customerTypeId, int customerVendorId)
        {
            return await _repository.GetAnotherInventoryLots(customerTypeId, customerVendorId);
        }
    }
}

