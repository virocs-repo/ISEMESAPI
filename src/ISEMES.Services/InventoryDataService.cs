using ISEMES.Models;
using ISEMES.Repositories;

namespace ISEMES.Services
{
    public class InventoryDataService : IInventoryDataService
    {
        private readonly IInventoryDataRepository _repository;

        public InventoryDataService(IInventoryDataRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<InventoryDetail>> GetInventoryDetailsAsync(int? customerVendorID = null, DateTime? fromDate = null, DateTime? toDate = null)
        {
            return await _repository.GetInventoryDetailsAsync(customerVendorID, fromDate, toDate);
        }

        public async Task<List<InventoryMoveStatus>> GetInventoryMoveDataAsync(DateTime? fromDate, DateTime? toDate)
        {
            return await _repository.GetInventoryMoveDataAsync(fromDate, toDate);
        }

        public async Task<LotInfoAreaByFacility> GetInventoryMoveLotInfoById(string lotId)
        {
            return await _repository.GetInventoryMoveLotInfoById(lotId);
        }

        public async Task<List<AreaByFacility>> GetInventoryMoveAreaByFacility(int facilityId)
        {
            return await _repository.GetInventoryMoveAreaByFacility(facilityId);
        }

        public async Task<bool> UpsertInventoryMove(InventoryMoveRequest request)
        {
            return await _repository.UpsertInventoryMove(request);
        }

        public async Task<List<ShipmentAddInventory>> GetShipmentInventoryAsync(int? customerId = null, int? locationId = null, int? receivedFromId = null, int? deviceId = null, int? shipmentCategoryID = null, string lotNumber = null)
        {
            return await _repository.GetShipmentInventoryAsync(customerId, locationId, receivedFromId, deviceId, shipmentCategoryID, lotNumber);
        }

        public async Task<bool> UpsertCreateShipmentRecordAsync(CreateAddShipRequest request)
        {
            return await _repository.UpsertCreateShipmentRecordAsync(request);
        }

        public async Task<List<ShippingDeliveryInfo>> GetShipmentdeliveryInfo(int? deliveryInfoId = null)
        {
            return await _repository.GetShipmentdeliveryInfo(deliveryInfoId);
        }

        public async Task<List<PackageDimension>> GetPackageDimensionsAsync()
        {
            return await _repository.GetPackageDimensionsAsync();
        }

        public async Task<bool> UpsertPackageDimensionsAsync(UpsertShipPackageDimensionReq request)
        {
            return await _repository.UpsertPackageDimensionsAsync(request);
        }

        public async Task<List<ShipmentPackage>> GetPackagesByShipmentIdAsync(int shipmentId)
        {
            return await _repository.GetPackagesByShipmentIdAsync(shipmentId);
        }
    }
}

