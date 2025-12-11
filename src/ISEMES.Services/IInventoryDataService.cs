using ISEMES.Models;

namespace ISEMES.Services
{
    public interface IInventoryDataService
    {
        Task<List<InventoryDetail>> GetInventoryDetailsAsync(int? customerVendorID, DateTime? fromDate, DateTime? toDate);
        Task<List<InventoryMoveStatus>> GetInventoryMoveDataAsync(DateTime? fromDate, DateTime? toDate);
        Task<LotInfoAreaByFacility> GetInventoryMoveLotInfoById(string lotId);
        Task<List<AreaByFacility>> GetInventoryMoveAreaByFacility(int facilityId);
        Task<bool> UpsertInventoryMove(InventoryMoveRequest request);
        Task<List<ShipmentAddInventory>> GetShipmentInventoryAsync(int? customerId = null, int? locationId = null, int? receivedFromId = null, int? deviceId = null, int? shipmentCategoryID = null, string lotNumber = null);
        Task<bool> UpsertCreateShipmentRecordAsync(CreateAddShipRequest request);
        Task<List<ShippingDeliveryInfo>> GetShipmentdeliveryInfo(int? deliveryInfoId = null);
        Task<List<PackageDimension>> GetPackageDimensionsAsync();
        Task<bool> UpsertPackageDimensionsAsync(UpsertShipPackageDimensionReq request);
        Task<List<ShipmentPackage>> GetPackagesByShipmentIdAsync(int shipmentId);
    }
}

