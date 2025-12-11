using ISEMES.Repositories;

namespace ISEMES.Services
{
    public class InventoryReportService : IInventoryReportService
    {
        private readonly IInventoryReportRepository _inventoryReportRepository;

        public InventoryReportService(IInventoryReportRepository inventoryReportRepository)
        {
            _inventoryReportRepository = inventoryReportRepository;
        }

        public async Task<IEnumerable<dynamic>> GetInventoryReportAsync(
            int? customerTypeId,
            int? customerVendorId,
            string goodsType,
            string lotNumber,
            int? inventoryStatusId,
            string dateCode,
            DateTime? fromDate,
            DateTime? toDate)
        {
            return await _inventoryReportRepository.GetInventoryReportAsync(
                customerTypeId, customerVendorId, goodsType, lotNumber,
                inventoryStatusId, dateCode, fromDate, toDate);
        }
    }
}

