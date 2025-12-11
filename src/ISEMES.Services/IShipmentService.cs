using ISEMES.Models;

namespace ISEMES.Services
{
    public interface IShipmentService
    {
        Task<List<Shipment>> ListShipmentAsync(DateTime? fromDate, DateTime? toDate);
        Task<List<ShipmentCategory>> ListShipmentCategoryAsync();
        Task<List<ShipmentType>> ListShipmentType();
        Task<List<ShipmentLineItem>> GetShipmentLineItem(int shipmentID);
        Task<List<ShipmentInventory>> GetShipmentInventory(int customerID);
        Task UpdateShipment(ShipmentRequest receiptRequest);
        Task UpdateShipmentLabel(ShippmentLabel request);
    }
}

