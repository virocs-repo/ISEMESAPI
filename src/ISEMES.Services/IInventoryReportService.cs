namespace ISEMES.Services
{
    public interface IInventoryReportService
    {
        Task<IEnumerable<dynamic>> GetInventoryReportAsync(
            int? customerTypeId,
            int? customerVendorId,
            string goodsType,
            string lotNumber,
            int? inventoryStatusId,
            string dateCode,
            DateTime? fromDate,
            DateTime? toDate);
    }
}



