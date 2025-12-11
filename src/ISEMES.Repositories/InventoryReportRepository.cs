using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace ISEMES.Repositories
{
    public class InventoryReportRepository : IInventoryReportRepository
    {
        private readonly AppDbContext _context;

        public InventoryReportRepository(AppDbContext context)
        {
            _context = context;
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
            var inventoryList = new List<dynamic>();

            using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
            using (var command = new SqlCommand("usp_GetInventoryReport", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CustomerTypeID", customerTypeId ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@CustomerVendorID", customerVendorId ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@GoodsType", goodsType ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@LotNumber", lotNumber ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@InventoryStatusID", inventoryStatusId ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@DateCode", dateCode ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@FromDate", fromDate ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@ToDate", toDate ?? (object)DBNull.Value);

                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        inventoryList.Add(new
                        {
                            GoodType = reader["GoodType"] != DBNull.Value ? reader["GoodType"].ToString() : null,
                            ReceiptID = reader["ReceiptID"] != DBNull.Value ? (int)reader["ReceiptID"] : (int?)null,
                            InventoryID = reader["InventoryID"] != DBNull.Value ? (int)reader["InventoryID"] : (int?)null,
                            ISELotNumber = reader["ISELotNumber"] != DBNull.Value ? reader["ISELotNumber"].ToString() : null,
                            CustomerLotNumber = reader["CustomerLotNumber"] != DBNull.Value ? reader["CustomerLotNumber"].ToString() : null,
                            Qty = reader["Qty"] != DBNull.Value ? (int)reader["Qty"] : (int?)null,
                            Expedite = reader["Expedite"] != DBNull.Value ? (int)reader["Expedite"] : (int?)null,
                            PartNum = reader["PartNum"] != DBNull.Value ? reader["PartNum"].ToString() : null,
                            Unprocessed = reader["Unprocessed"] != DBNull.Value ? (int)reader["Unprocessed"] : (int?)null,
                            Good = reader["Good"] != DBNull.Value ? (int)reader["Good"] : (int?)null,
                            Reject = reader["Reject"] != DBNull.Value ? (int)reader["Reject"] : (int?)null,
                            COO = reader["COO"] != DBNull.Value ? reader["COO"].ToString() : null,
                            DateCode = reader["DateCode"] != DBNull.Value ? reader["DateCode"].ToString() : null,
                            FGTPartNum = reader["FGTPartNum"] != DBNull.Value ? reader["FGTPartNum"].ToString() : null,
                            Status = reader["Status"] != DBNull.Value ? reader["Status"].ToString() : null,
                            AdditionalInfo = reader["AdditionalInfo"] != DBNull.Value ? reader["AdditionalInfo"].ToString() : null
                        });
                    }
                }
            }
            return inventoryList;
        }
    }
}

