using ISEMES.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace ISEMES.Repositories
{
    public class ShipmentRepository : IShipmentRepository
    {
        private readonly AppDbContext _context;
        public ShipmentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Shipment>> ListShipmentAsync(DateTime? fromDate, DateTime? toDate)
        {
            return await _context.ListShipment.FromSqlRaw("EXEC usp_GetShipment @FromDate = {0}, @ToDate = {1}",
                fromDate.HasValue ? fromDate : DBNull.Value, toDate.HasValue ? toDate : DBNull.Value).ToListAsync();
        }

        public async Task<List<ShipmentCategory>> ListShipmentCategoryAsync()
        {
            return await _context.ListShipmentCategory.FromSqlRaw("EXEC usp_GetShipmentCategory").ToListAsync();
        }

        public async Task<List<ShipmentLineItem>> GetShipmentLineItem(int shipmentID)
        {
            var rawResults = await _context.ListShipmentDetails
                .FromSqlRaw("EXEC usp_GetShipmentLineItem @ShipmentID = {0}", shipmentID)
                .AsNoTracking()
                .ToListAsync();

            var results = rawResults.Select(x => new ShipmentLineItem
            {
                InventoryID = x.InventoryID,
                ShipmentLineItemID = x.ShipmentLineItemID,
                CustomerLotNum = x.CustomerLotNum,
                ISELotNum = x.ISELotNum,
                GoodsType = x.GoodsType,
                PartNum = x.PartNum,
                CurrentQty = x.CurrentQty,
                ShipmentQty = x.ShipmentQty,
                ShipmentTypeID = x.ShipmentTypeID,
                ShipmentType = x.ShipmentType,
                Address = x.Address,
                AdditionalInfo = x.AdditionalInfo,
                ToStreet1 = x.ToStreet1,
                ToCity = x.ToCity,
                ToState = x.ToState,
                ToZip = x.ToZip,
                ToCountry = x.ToCountry,
                ToName = x.ToName,
                ToPhone = x.ToPhone,
                DeliveryInfoid = x.DeliveryInfoid,
            }).ToList();

            return results;
        }

        public async Task<List<ShipmentInventory>> GetShipmentInventory(int customerID)
        {
            return await _context.GetShipmentInventory.FromSqlRaw("EXEC usp_GetShipmentInventory @CustomerID = {0}", customerID).ToListAsync();
        }

        public async Task<List<ShipmentType>> ListShipmentType()
        {
            return await _context.ListShipmentType.FromSqlRaw("EXEC usp_GetShipmentType").ToListAsync();
        }

        public async Task UpdateShipment(ShipmentRequest request)
        {
            string jsonInput = JsonSerializer.Serialize(request, new JsonSerializerOptions { WriteIndented = true });
            var parameters = new[] { new Microsoft.Data.SqlClient.SqlParameter("@InputJSON", jsonInput) };
            await _context.Database.ExecuteSqlRawAsync("EXEC usp_UpsertShipment @InputJSON", parameters);
        }

        public async Task UpdateShipmentLabel(ShippmentLabel request)
        {
            var parameters = new[]{
                new Microsoft.Data.SqlClient.SqlParameter("@ShipmentID", request.ShipmentID),
                new Microsoft.Data.SqlClient.SqlParameter("@Trackingturl", request.Trackingurl),
                new Microsoft.Data.SqlClient.SqlParameter("@Trackingnum", request.Trackingnum),
                new Microsoft.Data.SqlClient.SqlParameter("@UserID", request.UserID)
            };
            await _context.Database.ExecuteSqlRawAsync("EXEC usp_UpdateShippmentLabel @ShipmentID, @Trackingturl, @Trackingnum, @UserID", parameters);
        }
    }
}

