using ISEMES.Models;
using ISEMES.Repositories;

namespace ISEMES.Services
{
    public class ShipmentService : IShipmentService
    {
        private readonly IShipmentRepository _repository;

        public ShipmentService(IShipmentRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Shipment>> ListShipmentAsync(DateTime? fromDate, DateTime? toDate)
        {
            return await _repository.ListShipmentAsync(fromDate, toDate);
        }

        public async Task<List<ShipmentCategory>> ListShipmentCategoryAsync()
        {
            return await _repository.ListShipmentCategoryAsync();
        }

        public async Task<List<ShipmentLineItem>> GetShipmentLineItem(int shipmentID)
        {
            return await _repository.GetShipmentLineItem(shipmentID);
        }

        public async Task<List<ShipmentType>> ListShipmentType()
        {
            return await _repository.ListShipmentType();
        }

        public async Task UpdateShipment(ShipmentRequest request)
        {
            await _repository.UpdateShipment(request);
        }

        public async Task<List<ShipmentInventory>> GetShipmentInventory(int customerID)
        {
            return await _repository.GetShipmentInventory(customerID);
        }

        public async Task UpdateShipmentLabel(ShippmentLabel request)
        {
            await _repository.UpdateShipmentLabel(request);
        }
    }
}

