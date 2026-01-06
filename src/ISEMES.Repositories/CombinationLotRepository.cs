using ISEMES.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace ISEMES.Repositories
{
    public class CombinationLotRepository : ICombinationLotRepository
    {
        private readonly AppDbContext _context;

        public CombinationLotRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<CombinationLot>> SearchCombinationLots(string fromDate, string toDate)
        {
            var result = new List<CombinationLot>();
            using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
            {
                using (var command = new SqlCommand("dbo.usp_SearchCombinationLots", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@FromDate", string.IsNullOrEmpty(fromDate) ? "" : fromDate);
                    command.Parameters.AddWithValue("@ToDate", string.IsNullOrEmpty(toDate) ? "" : toDate);

                    await connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            result.Add(new CombinationLot
                            {
                                ComboLotID = reader["ComboLotID"].ToString(),
                                ComboName = reader["ComboName"].ToString(),
                                Customer = reader["Customer"].ToString(),
                                Status = reader["Status"].ToString(),
                                ISELotNumber = reader["ISELotNumber"].ToString(),
                                CustomerLotNumber = reader["CustomerLotNumber"].ToString(),
                                DeviceType = reader["DeviceType"].ToString(),
                            });
                        }
                    }
                }
            }
            return result;
        }

        public async Task<List<CustomerCombLot>> GetCustomerLotsCombine(int customerId, string lotNumber)
        {
            var result = new List<CustomerCombLot>();
            using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
            {
                using (var command = new SqlCommand("dbo.usp_GetCustomerLots_Combine", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CustomerId", customerId);
                    if (!string.IsNullOrEmpty(lotNumber))
                    {
                        command.Parameters.AddWithValue("@LotNumber", lotNumber);
                    }

                    await connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            result.Add(new CustomerCombLot
                            {
                                ReceiptID = reader.GetInt32(reader.GetOrdinal("ReceiptID")),
                                CustomerVendorID = reader.GetInt32(reader.GetOrdinal("CustomerVendorID")),
                                GoodsType = reader["GoodsType"].ToString(),
                                InventoryID = reader.GetInt32(reader.GetOrdinal("InventoryID")),
                                ISELotNum = reader["ISELotNum"].ToString(),
                                CustomerLotNum = reader["CustomerLotNum"].ToString(),
                                ExpectedQty = reader.GetInt32(reader.GetOrdinal("ExpectedQty")),
                                Expedite = reader.GetBoolean(reader.GetOrdinal("Expedite")),
                                PartNum = reader.IsDBNull(reader.GetOrdinal("PartNum")) ? null : reader.GetString(reader.GetOrdinal("PartNum")),
                                LabelCount = reader.IsDBNull(reader.GetOrdinal("LabelCount")) ? 0 : reader.GetInt32(reader.GetOrdinal("LabelCount")),
                                COO = reader.IsDBNull(reader.GetOrdinal("COO")) ? null : reader.GetString(reader.GetOrdinal("COO")),
                                DateCode = reader.IsDBNull(reader.GetOrdinal("DateCode")) ? null : reader.GetString(reader.GetOrdinal("DateCode")),
                                IsHold = reader.GetBoolean(reader.GetOrdinal("IsHold")),
                                Active = reader.GetBoolean(reader.GetOrdinal("Active")),
                                AddressId = reader.IsDBNull(reader.GetOrdinal("AddressId")) ? 0 : reader.GetInt32(reader.GetOrdinal("AddressId")),
                                Address1 = reader.IsDBNull(reader.GetOrdinal("Address1")) ? null : reader.GetString(reader.GetOrdinal("Address1")),
                                RunningCount = reader.IsDBNull(reader.GetOrdinal("RunningCount")) ? 0 : reader.GetInt32(reader.GetOrdinal("RunningCount"))
                            });
                        }
                    }
                }
            }
            return result;
        }

        public async Task<bool> UpsertCombineLot(UpsertCombineLotRequest request)
        {
            using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
            {
                using (var command = new SqlCommand("dbo.usp_UpsertCombineLot", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ComboLotID", (object)request.ComboLotID ?? DBNull.Value);
                    command.Parameters.AddWithValue("@ComboName", request.ComboName);
                    command.Parameters.AddWithValue("@Str_InventoryId", request.Str_InventoryId);
                    command.Parameters.AddWithValue("@Primary_InventoryId", request.Primary_InventoryId);
                    command.Parameters.AddWithValue("@UserID", request.UserID);
                    command.Parameters.AddWithValue("@Active", request.Active);
                    command.Parameters.AddWithValue("@Comments", request.Comments);

                    await connection.OpenAsync();
                    var rowsAffected = await command.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
        }

        public async Task<List<CombineLotsDto>> GetCustomerCombineLotsAsync(int comboLotId)
        {
            var result = new List<CombineLotsDto>();
            using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
            {
                using (var command = new SqlCommand("usp_ViewCustomerCombineLots", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ComboLotID", comboLotId);

                    await connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            result.Add(new CombineLotsDto
                            {
                                ReceiptID = reader.IsDBNull(reader.GetOrdinal("ReceiptID")) ? 0 : reader.GetInt32(reader.GetOrdinal("ReceiptID")),
                                CustomerVendorID = reader.IsDBNull(reader.GetOrdinal("CustomerVendorID")) ? 0 : reader.GetInt32(reader.GetOrdinal("CustomerVendorID")),
                                BehalfID = reader.IsDBNull(reader.GetOrdinal("BehalfID")) ? 0 : reader.GetInt32(reader.GetOrdinal("BehalfID")),
                                GoodsType = reader.IsDBNull(reader.GetOrdinal("GoodsType")) ? null : reader.GetString(reader.GetOrdinal("GoodsType")),
                                InventoryID = reader.IsDBNull(reader.GetOrdinal("InventoryID")) ? 0 : reader.GetInt32(reader.GetOrdinal("InventoryID")),
                                ISELotNum = reader.IsDBNull(reader.GetOrdinal("ISELotNum")) ? null : reader.GetString(reader.GetOrdinal("ISELotNum")),
                                CustomerLotNum = reader.IsDBNull(reader.GetOrdinal("CustomerLotNum")) ? null : reader.GetString(reader.GetOrdinal("CustomerLotNum")),
                                ExpectedQty = reader.IsDBNull(reader.GetOrdinal("ExpectedQty")) ? 0 : reader.GetInt32(reader.GetOrdinal("ExpectedQty")),
                                Expedite = reader.IsDBNull(reader.GetOrdinal("Expedite")) ? false : reader.GetBoolean(reader.GetOrdinal("Expedite")),
                                PartNum = reader.IsDBNull(reader.GetOrdinal("PartNum")) ? null : reader.GetString(reader.GetOrdinal("PartNum")),
                                LabelCount = reader.IsDBNull(reader.GetOrdinal("LabelCount")) ? 0 : reader.GetInt32(reader.GetOrdinal("LabelCount")),
                                COO = reader.IsDBNull(reader.GetOrdinal("COO")) ? null : reader.GetString(reader.GetOrdinal("COO")),
                                DateCode = reader.IsDBNull(reader.GetOrdinal("DateCode")) ? null : reader.GetString(reader.GetOrdinal("DateCode")),
                                IsHold = reader.IsDBNull(reader.GetOrdinal("IsHold")) ? false : reader.GetBoolean(reader.GetOrdinal("IsHold")),
                                Active = reader.IsDBNull(reader.GetOrdinal("Active")) ? false : reader.GetBoolean(reader.GetOrdinal("Active")),
                                ComboLotID = reader.IsDBNull(reader.GetOrdinal("ComboLotID")) ? 0 : reader.GetInt32(reader.GetOrdinal("ComboLotID")),
                                ViewFlag = reader.IsDBNull(reader.GetOrdinal("ViewFlag")) ? 0 : reader.GetInt32(reader.GetOrdinal("ViewFlag")),
                                IsPrimaryAddress = reader.IsDBNull(reader.GetOrdinal("IsPrimaryAddress")) ? false : reader.GetBoolean(reader.GetOrdinal("IsPrimaryAddress")),
                                ComboName = reader.IsDBNull(reader.GetOrdinal("ComboName")) ? null : reader.GetString(reader.GetOrdinal("ComboName")),
                                Comments = reader.IsDBNull(reader.GetOrdinal("Comments")) ? null : reader.GetString(reader.GetOrdinal("Comments")),
                                AddressId = reader.IsDBNull(reader.GetOrdinal("AddressId")) ? 0 : reader.GetInt32(reader.GetOrdinal("AddressId")),
                                Address1 = reader.IsDBNull(reader.GetOrdinal("Address1")) ? null : reader.GetString(reader.GetOrdinal("Address1")),
                                RunningCount = reader.IsDBNull(reader.GetOrdinal("RunningCount")) ? 0 : reader.GetInt32(reader.GetOrdinal("RunningCount"))
                            });
                        }
                    }
                }
            }
            return result;
        }
    }
}



