using ISEMES.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace ISEMES.Repositories
{
    public class IntTranferReceivingRepository : IIntTranferReceivingRepository
    {
        private readonly AppDbContext _context;

        public IntTranferReceivingRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<IntTranferReceiving>> SearchInternalTransferReceivingAsync(DateTime? fromDate, DateTime? toDate, string? statusString, string? facilityId)
        {
            var results = new List<IntTranferReceiving>();
            using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("usp_SearchInternalTransferReceiving", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@FromDate", (object?)fromDate ?? DBNull.Value);
                    command.Parameters.AddWithValue("@ToDate", (object?)toDate ?? DBNull.Value);
                    command.Parameters.AddWithValue("@StatusString", (object?)statusString ?? DBNull.Value);
                    command.Parameters.AddWithValue("@FacilityID", (object)facilityId ?? DBNull.Value);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            results.Add(new IntTranferReceiving
                            {
                                InternalTransferId = reader.GetInt32(reader.GetOrdinal("InternalTransferId")),
                                LotNum = reader.GetString(reader.GetOrdinal("LotNum")),
                                LotQty = reader.GetInt32(reader.GetOrdinal("LotQty")),
                                Device = reader.GetString(reader.GetOrdinal("Device")),
                                CustomerName = reader.IsDBNull(reader.GetOrdinal("Customer Name")) ? null : reader.GetString(reader.GetOrdinal("Customer Name")),
                                DeviceFamily = reader.GetString(reader.GetOrdinal("DeviceFamily")),
                                CustomerID = reader.IsDBNull(reader.GetOrdinal("CustomerID")) ? null : reader.GetInt32(reader.GetOrdinal("CustomerID")),
                                ReceivedQty = reader.GetInt32(reader.GetOrdinal("Received Qty")),
                                CustomerLotNumber = reader.GetString(reader.GetOrdinal("CustomerLotNumber")),
                                Status = reader.GetString(reader.GetOrdinal("Status"))
                            });
                        }
                    }
                }
            }
            return results;
        }

        public async Task<IEnumerable<InternalTransferLot>> GetInternalTransferLotAsync(int customerVendorID, int customerTypeID)
        {
            var results = new List<InternalTransferLot>();
            using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("usp_GetInternalTransferLot", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CustomerVendorID", customerVendorID);
                    command.Parameters.AddWithValue("@CustomerTypeID", customerTypeID);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            results.Add(new InternalTransferLot
                            {
                                ISELotNum = reader.GetString(reader.GetOrdinal("ISELotNum")),
                                InventoryID = reader.GetInt32(reader.GetOrdinal("InventoryID"))
                            });
                        }
                    }
                }
            }
            return results;
        }

        public async Task<int> UpsertInternalTransferReceiptAsync(IntTransferReceiptReq request)
        {
            using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("usp_UpsertInternalTransferReceipt", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@InternalTransferID", string.IsNullOrEmpty(request.InternalTransferID) ? (object)DBNull.Value : request.InternalTransferID);
                    command.Parameters.AddWithValue("@ReceivedQty", request.ReceivedQty);
                    command.Parameters.AddWithValue("@UserID", request.UserId);
                    command.Parameters.AddWithValue("@Active", request.Active ? 1 : 0);
                    command.Parameters.AddWithValue("@InventoryID", request.InventoryID);
                    return await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}

