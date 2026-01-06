namespace ISEMES.Repositories
{
    public interface IInventoryReportRepository
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



