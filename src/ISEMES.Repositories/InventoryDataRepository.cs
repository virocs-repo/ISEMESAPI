using ISEMES.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Text.Json;

namespace ISEMES.Repositories
{
    public class InventoryDataRepository : IInventoryDataRepository
    {
        private readonly AppDbContext _context;
        private readonly TFSDbContext _tfsContext;

        public InventoryDataRepository(AppDbContext context, TFSDbContext tfsContext)
        {
            _context = context;
            _tfsContext = tfsContext;
        }

        public async Task<List<InventoryDetail>> GetInventoryDetailsAsync(int? customerVendorID = null, DateTime? fromDate = null, DateTime? toDate = null)
        {
            var inventoryDetails = new List<InventoryDetail>();
            using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("usp_GetInventoryDetails", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CustomerID", customerVendorID ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@FromDate", fromDate ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@ToDate", toDate ?? (object)DBNull.Value);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            inventoryDetails.Add(new InventoryDetail
                            {
                                GoodType = reader["GoodType"] != DBNull.Value ? reader["GoodType"].ToString() : null,
                                InventoryID = Convert.ToInt32(reader["InventoryID"]),
                                ReceiptID = reader["ReceiptID"] != DBNull.Value ? Convert.ToInt32(reader["ReceiptID"]) : (int?)null,
                                ISELotNumber = reader["ISELotNumber"] != DBNull.Value ? reader["ISELotNumber"].ToString() : null,
                                Qty = reader["Qty"] != DBNull.Value ? Convert.ToInt32(reader["Qty"]) : 0,
                                Expedite = reader["Expedite"] != DBNull.Value ? Convert.ToInt32(reader["Expedite"]) : 0,
                                PartNum = reader["PartNum"] != DBNull.Value ? reader["PartNum"].ToString() : null,
                                FGTPartNum = reader["FGTPartNum"] != DBNull.Value ? reader["FGTPartNum"].ToString() : null,
                                Unprocessed = reader["Unprocessed"] != DBNull.Value ? Convert.ToInt32(reader["Unprocessed"]) : 0,
                                Good = reader["Good"] != DBNull.Value ? Convert.ToInt32(reader["Good"]) : 0,
                                Reject = reader["Reject"] != DBNull.Value ? Convert.ToInt32(reader["Reject"]) : 0,
                                COO = reader["COO"] != DBNull.Value ? reader["COO"].ToString() : null,
                                DateCode = reader["DateCode"] != DBNull.Value ? reader["DateCode"].ToString() : null,
                                Status = reader["Status"] != DBNull.Value ? reader["Status"].ToString() : null,
                                Hold = reader["Hold"] != DBNull.Value ? reader["Hold"].ToString() : null
                            });
                        }
                    }
                }
            }
            return inventoryDetails;
        }

        public async Task<List<InventoryMoveStatus>> GetInventoryMoveDataAsync(DateTime? fromDate = null, DateTime? toDate = null)
        {
            var inventoryMoveDetails = new List<InventoryMoveStatus>();
            using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("usp_SearchInventoryMoveStatus", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    if (fromDate != null && toDate != null)
                    {
                        command.Parameters.AddWithValue("@FromDate", fromDate ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@ToDate", toDate ?? (object)DBNull.Value);
                    }

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            inventoryMoveDetails.Add(new InventoryMoveStatus
                            {
                                InventoryID = reader["InventoryID"] != DBNull.Value ? Convert.ToInt32(reader["InventoryID"]) : 0,
                                LotNum = reader["LotNum"] != DBNull.Value ? reader["LotNum"].ToString() : null,
                                LocationID = reader["LocationID"] != DBNull.Value ? Convert.ToInt32(reader["LocationID"]) : (int?)null,
                                Location = reader["Location"] != DBNull.Value ? reader["Location"].ToString() : null,
                                ReceivedFrom = reader["ReceivedFrom"] != DBNull.Value ? reader["ReceivedFrom"].ToString() : null,
                                Person = reader["Person"] != DBNull.Value ? reader["Person"].ToString() : null,
                                Qty = reader["Qty"] != DBNull.Value ? Convert.ToInt32(reader["Qty"]) : 0,
                                CheckInOutQty = reader["Checkin/CheckOut QTY"] != DBNull.Value ? Convert.ToInt32(reader["Checkin/CheckOut QTY"]) : 0,
                                SystemUser = reader["SystemUser"] != DBNull.Value ? reader["SystemUser"].ToString() : null,
                                InventoryStatusID = reader["InventoryStatusID"] != DBNull.Value ? Convert.ToInt32(reader["InventoryStatusID"]) : 0,
                                InventoryStatus = reader["InventoryStatus"] != DBNull.Value ? reader["InventoryStatus"].ToString() : null,
                                GoodsType = reader["GoodsType"] != DBNull.Value ? reader["GoodsType"].ToString() : null,
                                ModifiedOn = reader["ModifiedOn"] != DBNull.Value ? Convert.ToDateTime(reader["ModifiedOn"]) : DateTime.MinValue,
                                Area = reader["Area"] != DBNull.Value ? reader["Area"].ToString() : null,
                                WIPLocation = reader["WIP Location"] != DBNull.Value ? reader["WIP Location"].ToString() : null,
                                CustomerName = reader["CustomerName"] != DBNull.Value ? reader["CustomerName"].ToString() : null,
                                Device = reader["Device"] != DBNull.Value ? reader["Device"].ToString() : null,
                                ReceivedFromOrCheckOut = reader["Received From/CheckOut"] != DBNull.Value ? reader["Received From/CheckOut"].ToString() : null,
                                CheckInOutTime = reader["CheckIn/Out Time"] != DBNull.Value ? Convert.ToDateTime(reader["CheckIn/Out Time"]) : DateTime.MinValue,
                                SystemCheckInOutPerson = reader["System CheckIn/Out Person"] != DBNull.Value ? reader["System CheckIn/Out Person"].ToString() : null,
                                CurrentLocation = reader["Current Location"] != DBNull.Value ? reader["Current Location"].ToString() : null,
                                FacilityArea = reader["Facility Area"] != DBNull.Value ? reader["Facility Area"].ToString() : null,
                                Facility = reader["Facility"] != DBNull.Value ? reader["Facility"].ToString() : null,
                                CustomerLotNumber = reader["CustomerLotNumber"] != DBNull.Value ? reader["CustomerLotNumber"].ToString() : null
                            });
                        }
                    }
                }
            }
            return inventoryMoveDetails;
        }

        public async Task<LotInfoAreaByFacility> GetInventoryMoveLotInfoById(string lotId)
        {
            using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
            using (var command = new SqlCommand("dbo.INV_GETLotInfoByID", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@LotNumber", lotId);
                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new LotInfoAreaByFacility
                        {
                            InventoryID = reader["InventoryID"] != DBNull.Value ? Convert.ToInt32(reader["InventoryID"]) : 0,
                            CurrentLocation = reader["Location"] != DBNull.Value ? reader["Location"].ToString() : null,
                            Area_FacilityID = reader["Area_FacilityID"] != DBNull.Value ? Convert.ToInt32(reader["Area_FacilityID"]) : (int?)null,
                            FacilityID = reader["FacilityID"] != DBNull.Value ? Convert.ToInt32(reader["FacilityID"]) : (int?)null
                        };
                    }
                }
            }
            return null;
        }

        public async Task<List<AreaByFacility>> GetInventoryMoveAreaByFacility(int facilityId)
        {
            var areas = new List<AreaByFacility>();
            using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
            using (var command = new SqlCommand("dbo.usp_InvGetAreaByFacility", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@FacilityId", facilityId);
                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        areas.Add(new AreaByFacility
                        {
                            Area_FacilityId = reader["Area_FacilityId"] != DBNull.Value ? Convert.ToInt32(reader["Area_FacilityId"]) : 0,
                            AreaID = reader["AreaID"] != DBNull.Value ? Convert.ToInt32(reader["AreaID"]) : 0,
                            Area_Name = reader["Area_Name"] != DBNull.Value ? reader["Area_Name"].ToString() : null,
                            FacilityId = reader["FacilityId"] != DBNull.Value ? Convert.ToInt32(reader["FacilityId"]) : 0
                        });
                    }
                }
            }
            return areas;
        }

        public async Task<bool> UpsertInventoryMove(InventoryMoveRequest request)
        {
            using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
            using (var command = new SqlCommand("usp_UpsertInventoryMove", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@InventoryId", request.InventoryId);
                command.Parameters.AddWithValue("@Area_FacilityID", request.AreaFacilityId.HasValue ? (object)request.AreaFacilityId : DBNull.Value);
                command.Parameters.AddWithValue("@FacilityID", request.FacilityId.HasValue ? (object)request.FacilityId : DBNull.Value);
                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        var result = reader["Result"]?.ToString();
                        return result == "Success";
                    }
                }
            }
            return false;
        }

        public async Task<List<ShipmentAddInventory>> GetShipmentInventoryAsync(int? customerId, int? locationId, int? receivedFromId, int? deviceId, int? shipmentCategoryID, string lotNumber)
        {
            var inventoryList = new List<ShipmentAddInventory>();
            using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("usp_GetShipmentInventoryforAllCustomers", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CustomerID", (object?)customerId ?? DBNull.Value);
                    command.Parameters.AddWithValue("@locationID", (object?)locationId ?? DBNull.Value);
                    command.Parameters.AddWithValue("@ReceivedFromID", (object?)receivedFromId ?? DBNull.Value);
                    command.Parameters.AddWithValue("@DeviceID", (object?)deviceId ?? DBNull.Value);
                    command.Parameters.AddWithValue("@lotnumber", (object?)lotNumber ?? DBNull.Value);
                    command.Parameters.AddWithValue("@ShipmentCategoryID", (object?)shipmentCategoryID ?? DBNull.Value);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            inventoryList.Add(new ShipmentAddInventory
                            {
                                InventoryID = reader.IsDBNull(reader.GetOrdinal("InventoryID")) ? 0 : reader.GetInt32("InventoryID"),
                                CustomerLotNum = reader.IsDBNull(reader.GetOrdinal("CustomerLotNum")) ? string.Empty : reader.GetString("CustomerLotNum"),
                                ISELotNum = reader.IsDBNull(reader.GetOrdinal("ISELotNum")) ? string.Empty : reader.GetString("ISELotNum"),
                                GoodsType = reader.IsDBNull(reader.GetOrdinal("GoodsType")) ? string.Empty : reader.GetString("GoodsType"),
                                PartNum = reader.IsDBNull(reader.GetOrdinal("PartNum")) ? null : reader.GetString("PartNum"),
                                CurrentQty = reader.IsDBNull(reader.GetOrdinal("CurrentQty")) ? 0 : reader.GetInt32("CurrentQty"),
                                ShipmentQty = reader.IsDBNull(reader.GetOrdinal("ShipmentQty")) ? 0 : reader.GetInt32("ShipmentQty"),
                                ShipmentTypeID = reader.IsDBNull(reader.GetOrdinal("ShipmentTypeID")) ? 0 : reader.GetInt32("ShipmentTypeID"),
                                ShipmentType = reader.IsDBNull(reader.GetOrdinal("ShipmentType")) ? string.Empty : reader.GetString("ShipmentType"),
                                Address = reader.IsDBNull(reader.GetOrdinal("Address")) ? string.Empty : reader.GetString("Address"),
                                LocationID = reader.IsDBNull(reader.GetOrdinal("LocationID")) ? (int?)null : reader.GetInt32("LocationID"),
                                CustomerID = reader.IsDBNull(reader.GetOrdinal("CustomerID")) ? 0 : reader.GetInt32("CustomerID"),
                                CustomerName = reader.IsDBNull(reader.GetOrdinal("CustomerName")) ? string.Empty : reader.GetString("CustomerName")
                            });
                        }
                    }
                }
            }
            return inventoryList;
        }

        public async Task<bool> UpsertCreateShipmentRecordAsync(CreateAddShipRequest request)
        {
            try
            {
                using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("usp_UpsertCreateShipmentRecord", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@CustomerID", request.CustomerID);
                        command.Parameters.AddWithValue("@CurrentLocationID", request.CurrentLocationID ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@strInventoryId", request.InventoryIds);
                        command.Parameters.AddWithValue("@ShipmentCategoryID", request.ShipmentCategoryID ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@userId", request.UserID);
                        await command.ExecuteNonQueryAsync();
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<ShippingDeliveryInfo>> GetShipmentdeliveryInfo(int? deliveryInfoId = null)
        {
            var deliveryInfos = new List<ShippingDeliveryInfo>();
            using (var connection = new SqlConnection(_tfsContext.Database.GetConnectionString()))
            {
                using (SqlCommand command = new SqlCommand("PRD_Shipping_DeliveryInfo_P_GetDetails", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@DeliveryInfoId", deliveryInfoId);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            deliveryInfos.Add(new ShippingDeliveryInfo
                            {
                                DeliveryInfoId = reader.GetInt32(reader.GetOrdinal("DeliveryInfoId")),
                                ShippingMethodId = reader.IsDBNull(reader.GetOrdinal("ShippingMethodId")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("ShippingMethodId")),
                                ShippingMethod = reader.IsDBNull(reader.GetOrdinal("ShippingMethod")) ? null : reader.GetString(reader.GetOrdinal("ShippingMethod")),
                                ContactPerson = reader.IsDBNull(reader.GetOrdinal("ContactPerson")) ? null : reader.GetString(reader.GetOrdinal("ContactPerson")),
                                Address = reader.IsDBNull(reader.GetOrdinal("Address")) ? null : reader.GetString(reader.GetOrdinal("Address")),
                            });
                        }
                    }
                }
            }
            return deliveryInfos;
        }

        public async Task<List<PackageDimension>> GetPackageDimensionsAsync()
        {
            var dimensions = new List<PackageDimension>();
            using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
            {
                using (SqlCommand command = new SqlCommand("INV_GetPackageDimensions", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    await connection.OpenAsync();
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            dimensions.Add(new PackageDimension
                            {
                                MasterListItemId = reader.GetInt32(reader.GetOrdinal("MasterListItemId")),
                                CIPackageDimentions = reader.GetString(reader.GetOrdinal("CIPackageDimentions"))
                            });
                        }
                    }
                }
            }
            return dimensions;
        }

        public async Task<bool> UpsertPackageDimensionsAsync(UpsertShipPackageDimensionReq request)
        {
            try
            {
                string inputJson = JsonSerializer.Serialize(request);
                using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
                using (SqlCommand command = new SqlCommand("usp_Upsert_PackageDimensions", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@InputJson", SqlDbType.NVarChar) { Value = inputJson });
                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<ShipmentPackage>> GetPackagesByShipmentIdAsync(int shipmentId)
        {
            var packages = new List<ShipmentPackage>();
            using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
            using (SqlCommand command = new SqlCommand("INV_GetPackagesByShipmentId", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@ShipmentId", SqlDbType.Int) { Value = shipmentId });
                await connection.OpenAsync();
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        packages.Add(new ShipmentPackage
                        {
                            PackageId = !reader.IsDBNull(reader.GetOrdinal("PackageId")) ? reader.GetInt32(reader.GetOrdinal("PackageId")) : 0,
                            ShipmentId = !reader.IsDBNull(reader.GetOrdinal("ShipmentId")) ? reader.GetInt32(reader.GetOrdinal("ShipmentId")) : 0,
                            PackageNo = !reader.IsDBNull(reader.GetOrdinal("PackageNo")) ? reader.GetInt32(reader.GetOrdinal("PackageNo")) : 0,
                            CIPackageDimentions = !reader.IsDBNull(reader.GetOrdinal("CIPackageDimentions")) ? reader.GetString(reader.GetOrdinal("CIPackageDimentions")) : string.Empty,
                            CIWeight = !reader.IsDBNull(reader.GetOrdinal("CIWeight")) ? reader.GetInt32(reader.GetOrdinal("CIWeight")) : 0,
                            CreatedBy = !reader.IsDBNull(reader.GetOrdinal("CreatedBy")) ? reader.GetInt32(reader.GetOrdinal("CreatedBy")) : 0,
                            CreatedOn = !reader.IsDBNull(reader.GetOrdinal("CreatedOn")) ? reader.GetDateTime(reader.GetOrdinal("CreatedOn")) : DateTime.MinValue,
                            Active = !reader.IsDBNull(reader.GetOrdinal("Active")) ? reader.GetBoolean(reader.GetOrdinal("Active")) : false,
                        });
                    }
                }
            }
            return packages;
        }
    }
}



