using ISEMES.Models;

namespace ISEMES.Repositories
{
    public interface IInventoryDataRepository
    {
        Task<List<InventoryDetail>> GetInventoryDetailsAsync(int? customerVendorID = null, DateTime? fromDate = null, DateTime? toDate = null);
        Task<List<InventoryMoveStatus>> GetInventoryMoveDataAsync(DateTime? fromDate = null, DateTime? toDate = null);
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

