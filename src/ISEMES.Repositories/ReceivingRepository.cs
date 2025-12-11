using ISEMES.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Data;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace ISEMES.Repositories
{
    public class ReceivingRepository : IReceivingRepository
    {
        private readonly AppDbContext _context;
        private readonly TFSDbContext _tfsContext;
        private readonly IConfiguration _configuration;

        public ReceivingRepository(AppDbContext context, TFSDbContext tfsContext, IConfiguration configuration)
        {
            _context = context;
            _tfsContext = tfsContext;
            _configuration = configuration;
        }

        public async Task<List<DeviceDetails>> GetDeviceDetailsAsync(string? mailRoomNo, string? stagingLocation)
        {
            return await _context.ListDeviceDetails
                .FromSqlRaw("EXEC usp_GetDevicesForCustomer @MailRoomNo = {0}, @StagingLocation = {1}",
                    string.IsNullOrEmpty(mailRoomNo) ? DBNull.Value : mailRoomNo,
                    string.IsNullOrEmpty(stagingLocation) ? DBNull.Value : stagingLocation)
                .ToListAsync();
        }

        public async Task<List<HardwareDetails>> GetHardwareDetailsAsync(int receiptId)
        {
            return await _context.ListHardwareDetails.FromSqlRaw("EXEC usp_GetHardware @ReceiptId = {0}", receiptId).ToListAsync();
        }

        public async Task<List<ReceiptDetail>> GetReceiptDetailsAsync(int? receiptID, DateTime? fromDate, DateTime? toDate, string? receiptStatus, string? facilityIDStr)
        {
            var rawReceiptDetails = await _context.ListReceiptDetails.FromSqlRaw(
                "EXEC usp_GetReceipt @ReceiptID= {0}, @FromDate = {1}, @ToDate = {2},@ReceiptStatus = {3},@FacilityIDStr = {4}",
                receiptID.HasValue ? receiptID : DBNull.Value,
                fromDate.HasValue ? fromDate : DBNull.Value,
                toDate.HasValue ? toDate : DBNull.Value,
                string.IsNullOrEmpty(receiptStatus) ? DBNull.Value : receiptStatus,
                string.IsNullOrEmpty(facilityIDStr) ? DBNull.Value : facilityIDStr).ToListAsync();

            foreach (var detail in rawReceiptDetails)
            {
                if (!string.IsNullOrEmpty(detail.Signature))
                {
                    detail.Signaturebase64Data = "data:image/png;base64," + detail.Signature;
                }
            }
            return rawReceiptDetails;
        }

        public async Task<List<MiscellaneousGoods>> GetMiscellaneousGoodsAsync(int receiptId)
        {
            return await _context.ListMiscellaneousGoods.FromSqlRaw("EXEC usp_GetMiscellaneousGoods @ReceiptId = {0}", receiptId).ToListAsync();
        }

        public async Task UpdateDeviceAsync(DeviceDetailsRequest request)
        {
            string jsonInput = JsonSerializer.Serialize(request, new JsonSerializerOptions { WriteIndented = true });
            var parameters = new[] { new SqlParameter("@InputJSON", jsonInput) };
            await _context.Database.ExecuteSqlRawAsync("EXEC usp_UpsertDevice @InputJSON", parameters);
        }

        public async Task UpdateHardwareAsync(HardwareDetailsRequest request)
        {
            string jsonInput = JsonSerializer.Serialize(request, new JsonSerializerOptions { WriteIndented = true });
            var parameters = new[] { new SqlParameter("@InputJSON", jsonInput) };
            await _context.Database.ExecuteSqlRawAsync("EXEC usp_UpsertHardware @InputJSON", parameters);
        }

        public async Task UpdateReceiptDetailsAsync(ReceiptRequestDetails request)
        {
            foreach (var detail in request.ReceiptDetails)
            {
                if (!string.IsNullOrEmpty(detail.Signaturebase64Data))
                {
                    detail.Signature = detail.Signaturebase64Data.Split(',').Last();
                }
            }
            string jsonInput = JsonSerializer.Serialize(request);
            var parameters = new[] { new SqlParameter("@InputJSON", jsonInput) };
            await _context.Database.ExecuteSqlRawAsync("EXEC usp_UpsertReceipt @InputJSON", parameters);
        }

        public async Task UpdateMiscellaneousGoodsAsync(MiscellaneousGoodsDetailsRequest request)
        {
            string jsonInput = JsonSerializer.Serialize(request, new JsonSerializerOptions { WriteIndented = true });
            var parameters = new[] { new SqlParameter("@InputJSON", jsonInput) };
            await _context.Database.ExecuteSqlRawAsync("EXEC usp_UpsertMiscellaneousGoods @InputJSON", parameters);
        }

        public async Task<List<ReceiptEmployee>> GetReceiptEmployeeAsync(int receiptId)
        {
            return await _context.ListReceiptEmployee.FromSqlRaw("EXEC usp_GetReceiptXEmployee @ReceiptId = {0}", receiptId).ToListAsync();
        }

        public async Task VoidReceipt(int receiptId)
        {
            await _context.Database.ExecuteSqlRawAsync("EXEC usp_VoidReceipt @ReceiptID = {0}", receiptId);
        }

        public async Task<List<DeviceTypes>> GetDeviceTypeByCustomer(int? customerId)
        {
            var customerIdParam = new SqlParameter("@CustomerId", customerId.HasValue ? customerId.Value : (object)DBNull.Value);
            return await _context.ListDeviceTypes.FromSqlRaw("EXEC usp_GetDeviceTypeByCustomer @CustomerId", customerIdParam).ToListAsync();
        }

        public async Task<string> GetGeneratedLineItemNumber()
        {
            using var connection = new SqlConnection(_tfsContext.Database.GetConnectionString());
            using var command = new SqlCommand("dbo.PRD_P_GenNo", connection) { CommandType = CommandType.StoredProcedure };
            command.Parameters.AddWithValue("@Prefix", "L");
            command.Parameters.AddWithValue("@RecType", "Lot");

            var receivingNoParam = new SqlParameter("@RecevingNo", SqlDbType.NVarChar, 50) { Direction = ParameterDirection.Output };
            command.Parameters.Add(receivingNoParam);
            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
            var dynamicObject = new { data = receivingNoParam.Value?.ToString() };
            return JsonConvert.SerializeObject(dynamicObject);
        }

        public async Task UpsertAttachment(Attachment attachmentrequest)
        {
            using (SqlConnection connection = new SqlConnection(_context.Database.GetConnectionString()))
            {
                using (SqlCommand command = new SqlCommand("usp_UpsertAttachment", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@AttachmentId", (object)attachmentrequest.AttachmentId ?? DBNull.Value);
                    command.Parameters.AddWithValue("@ObjectID", attachmentrequest.ObjectID);
                    command.Parameters.AddWithValue("@AttachmentName", attachmentrequest.AttachmentName);
                    command.Parameters.AddWithValue("@Path", attachmentrequest.Path);
                    command.Parameters.AddWithValue("@Active", attachmentrequest.Active ? 1 : 0);
                    command.Parameters.AddWithValue("@LoginId", attachmentrequest.LoginId);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public async Task<List<Attachment>> ListReceiptAttachment(int receiptId)
        {
            return await _context.ListReceiptAttachment.FromSqlRaw("EXEC usp_GetAttachmentByID @ObjectID = {0}, @AttachmentName= {1}", receiptId, "Receipt").ToListAsync();
        }

        public async Task<List<Attachment>> ListAttachmentById(int Id, string? name = null)
        {
            if (string.IsNullOrEmpty(name)) name = "ShipAlert";
            return await _context.ListReceiptAttachment.FromSqlRaw("EXEC usp_GetAttachmentByID @ObjectID = {0}, @AttachmentName= {1}", Id, name).ToListAsync();
        }

        public async Task<List<InventoryLosts>> GetInterimLotsAsync()
        {
            var invLosts = new List<InventoryLosts>();
            using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("usp_GetinterimLots", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            invLosts.Add(new InventoryLosts { InventoryID = reader.GetInt32(0), ISELotNumber = reader.GetString(1) });
                        }
                    }
                }
            }
            return invLosts;
        }

        public async Task<List<InterimDevice>> GetInterimDeviceDataAsync(int interimReceiptID)
        {
            var intermDevices = new List<InterimDevice>();
            using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("usp_GetinterimDevicedetails", connection))
                {
                    command.Parameters.AddWithValue("@ReceiptID", interimReceiptID);
                    command.CommandType = CommandType.StoredProcedure;
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var intermDevice = new InterimDevice
                            {
                                DeviceID = reader.GetInt32(0),
                                InventoryID = reader.GetInt32(1),
                                ReceiptID = reader.GetInt32(2),
                                ISELotNumber = reader.GetString(3),
                                CustomerLotNumber = reader.GetString(4),
                                CustomerCount = reader.GetInt32(5),
                                LabelCount = reader.GetInt32(6),
                                DeviceTypeID = reader.GetInt32(7),
                                LotCategoryID = reader.GetInt32(8),
                                IsHold = reader.GetBoolean(9),
                                Active = reader.GetBoolean(11),
                                ReceivedQTY = reader.GetInt32(12),
                                GoodQty = reader.GetInt32(13),
                                RejectedQty = reader.GetInt32(14),
                                DeviceType = reader.GetString(18)
                            };
                            intermDevices.Add(intermDevice);
                        }
                    }
                }
            }
            return intermDevices;
        }

        public async Task<List<InterimDevice>> GetdetailsByInventoryIdAsync(int inventoryID)
        {
            var intermDevices = new List<InterimDevice>();
            using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("usp_GetdetailsByInventoryID", connection))
                {
                    command.Parameters.AddWithValue("@InventoryID", inventoryID);
                    command.CommandType = CommandType.StoredProcedure;
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var intermDevice = new InterimDevice
                            {
                                DeviceID = reader.GetInt32(0),
                                InventoryID = reader.GetInt32(1),
                                ReceiptID = reader.GetInt32(2),
                                ISELotNumber = reader.GetString(3),
                                CustomerLotNumber = reader.GetString(4),
                                CustomerCount = reader.GetInt32(5),
                                LabelCount = reader.GetInt32(6),
                                DeviceTypeID = reader.GetInt32(7),
                                DeviceType = reader.GetString(8),
                                LotCategoryID = reader.GetInt32(9),
                                IsHold = reader.GetBoolean(11),
                                Active = reader.GetBoolean(13),
                                LotIdentifier = reader[14] == DBNull.Value ? null : reader.GetString(14)
                            };
                            intermDevices.Add(intermDevice);
                        }
                    }
                }
            }
            return intermDevices;
        }

        public async Task UpdateInterimDeviceAsync(InterimDeviceDetail interimDevice)
        {
            using (SqlConnection connection = new SqlConnection(_context.Database.GetConnectionString()))
            {
                using (SqlCommand command = new SqlCommand("usp_UpsertInterimDevice", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@InventoryID", interimDevice.InventoryID);
                    command.Parameters.AddWithValue("@InterimReceiptID", interimDevice.InterimReceiptID);
                    command.Parameters.AddWithValue("@UserID", interimDevice.UserID);
                    command.Parameters.AddWithValue("@ReceivedQTY", interimDevice.ReceivedQTY);
                    command.Parameters.AddWithValue("@GoodQty", interimDevice.GoodQty);
                    command.Parameters.AddWithValue("@RejectQty", interimDevice.RejectedQty);
                    command.Parameters.AddWithValue("@InterimStatusID", interimDevice.InterimStatusID);
                    command.Parameters.AddWithValue("@IsReceived", interimDevice.IsReceived);
                    command.Parameters.AddWithValue("@Active", interimDevice.Active);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public async Task<bool> CanUndoReceiveLotAsync(int inventoryID)
        {
            bool retValue = false;
            using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("CanUndoReceiveLot", connection))
                {
                    command.Parameters.AddWithValue("@InventoryID", inventoryID);
                    command.CommandType = CommandType.StoredProcedure;
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync()) retValue = reader.GetBoolean(0);
                    }
                }
            }
            return retValue;
        }

        public async Task<int> CheckingIsReceiptEditableAsync(int receiptId, int loginId)
        {
            int isAllowed = 0;
            using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("usp_InvIsReceiptEditable", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ReceiptID", receiptId);
                    command.Parameters.AddWithValue("@LoginID", loginId);
                    var isAllowedParam = new SqlParameter("@IsAllowed", SqlDbType.Bit) { Direction = ParameterDirection.Output };
                    command.Parameters.Add(isAllowedParam);
                    await command.ExecuteNonQueryAsync();
                    isAllowed = isAllowedParam.Value != DBNull.Value ? Convert.ToInt32(isAllowedParam.Value) : 0;
                }
            }
            return isAllowed;
        }

        public async Task<List<ReceiptInternalDetail>> GetReceiverFormInternalAsync(string? status, bool? isExpected, DateTime? fromDate, DateTime? toDate)
        {
            var receiverFormInternalDetails = new List<ReceiptInternalDetail>();
            using (var connection = new SqlConnection(_configuration.GetConnectionString("InventoryConnection")))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("usp_SearchReceiptForm", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Status", status ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@ISExpected", isExpected ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@FromDate", fromDate ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@ToDate", toDate ?? (object)DBNull.Value);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            receiverFormInternalDetails.Add(new ReceiptInternalDetail
                            {
                                ReceiptID = reader.IsDBNull(0) ? (int?)null : reader.GetInt32(0),
                                ReceivingNo = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                                MailNo = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                                CustomerName = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                                CreateDate = reader.IsDBNull(4) ? (DateTime?)null : reader.GetDateTime(4),
                                Recipient = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                                DeliveryMethod = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                                ExpectedDate = reader.IsDBNull(7) ? (DateTime?)null : reader.GetDateTime(7),
                                NoofPackages = reader.IsDBNull(8) ? (int?)null : reader.GetInt32(8),
                                ReceivingStatus = reader.IsDBNull(9) ? string.Empty : reader.GetString(9),
                                MailStatus = reader.IsDBNull(10) ? string.Empty : reader.GetString(10),
                                SenderName = reader.IsDBNull(11) ? string.Empty : reader.GetString(11),
                                AWB = reader.IsDBNull(12) ? string.Empty : reader.GetString(12),
                                Location = reader.IsDBNull(13) ? string.Empty : reader.GetString(13),
                                CreatedBy = reader.IsDBNull(14) ? string.Empty : reader.GetString(14),
                                Device = reader.IsDBNull(15) ? string.Empty : reader.GetString(15),
                                ISELotNumbers = reader.IsDBNull(16) ? string.Empty : reader.GetString(16),
                                CustomerLotNumbers = reader.IsDBNull(17) ? string.Empty : reader.GetString(17),
                                ExpectedUnexpected = reader.IsDBNull(18) ? string.Empty : reader.GetString(18),
                            });
                        }
                    }
                }
            }
            return receiverFormInternalDetails;
        }

        public async Task<List<CustomerReceiptDetail>> SearchCustomerReceiverForm(int? receivingInfoId, int? customerId, int? deviceFamilyId, int? deviceId, string? customerLotsStr, int? statusId, bool? isExpected, string? isElot, int? serviceCategoryId, int? locationId, string? mail, DateTime? fromDate, DateTime? toDate, string? receiptStatus, string? facilityIdStr)
        {
            var receiverFormInternalDetails = new List<CustomerReceiptDetail>();
            using (var connection = new SqlConnection(_configuration.GetConnectionString("InventoryConnection")))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("usp_SearchReceiptForm", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ReceivingInfoID", receivingInfoId ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@CustomerID", customerId ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@DeviceFamilyID", deviceFamilyId ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@DeviceID", deviceId ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@CustomerLotsstr", customerLotsStr ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@StatusID", statusId ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@ISExpected", isExpected ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@ISElot", isElot ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@ServiceCategoryID", serviceCategoryId ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@LocationID", locationId ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Mail", mail ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@ReceiptStatus", receiptStatus ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@FacilityIDStr", facilityIdStr ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@FromDate", fromDate ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@ToDate", toDate ?? (object)DBNull.Value);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            receiverFormInternalDetails.Add(new CustomerReceiptDetail
                            {
                                ReceivingInfo = reader.IsDBNull(47) ? string.Empty : reader.GetString(47),
                                Mail = reader.IsDBNull(48) ? string.Empty : reader.GetString(48),
                                CustomerVendor = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                                DeliveryMode = reader.IsDBNull(10) ? string.Empty : reader.GetString(10),
                                TrackingNumber = reader.IsDBNull(14) ? string.Empty : reader.GetString(14),
                                Sender = reader.IsDBNull(49) ? string.Empty : reader.GetString(49),
                                SenderFrom = reader.IsDBNull(50) ? string.Empty : reader.GetString(50),
                                SubCategory = reader.IsDBNull(51) ? string.Empty : reader.GetString(51),
                                MailStatus = reader.IsDBNull(30) ? string.Empty : reader.GetString(30),
                                TravelerStatus = reader.IsDBNull(52) ? string.Empty : reader.GetString(52),
                                ModifiedOn = reader.IsDBNull(46) ? (DateTime?)null : reader.GetDateTime(46),
                                IseLots = reader.IsDBNull(43) ? string.Empty : reader.GetString(43),
                                CustomerLots = reader.IsDBNull(44) ? string.Empty : reader.GetString(44),
                                ReceiptStatus = reader.IsDBNull(40) ? string.Empty : reader.GetString(40),
                                Modifiedby = reader.IsDBNull(45) ? string.Empty : reader.GetString(45),
                                Recipient = reader.IsDBNull(53) ? string.Empty : reader.GetString(53),
                                Location = reader.IsDBNull(54) ? string.Empty : reader.GetString(54),
                            });
                        }
                    }
                }
            }
            return receiverFormInternalDetails;
        }

        public async Task<List<InventoryReceiptStatus>> GetInventoryReceiptStatusesAsync()
        {
            var statuses = new List<InventoryReceiptStatus>();
            using (var connection = new SqlConnection(_configuration.GetConnectionString("InventoryConnection")))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("PRD_GetStatusForReceiving", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            statuses.Add(new InventoryReceiptStatus
                            {
                                MasterListItemId = reader.GetInt32(0),
                                MasterListId = reader.GetInt32(1),
                                ItemText = reader.GetString(2),
                                ItemValue = reader.GetString(3)
                            });
                        }
                    }
                }
            }
            return statuses;
        }

        public async Task<List<DeviceFamily>> GetDeviceFamiliesAsync(int customerId)
        {
            var deviceFamilies = new List<DeviceFamily>();
            using (var connection = new SqlConnection(_configuration.GetConnectionString("InventoryConnection")))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("PRD_Device_P_SearchDeviceFamily", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@CustomerId", customerId));
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            deviceFamilies.Add(new DeviceFamily { DeviceFamilyId = reader.GetInt32(0), DeviceFamilyName = reader.GetString(1) });
                        }
                    }
                }
            }
            return deviceFamilies;
        }

        public async Task<List<DeviceFamily>> DeviceFamiliesAsync(int customerId)
        {
            var deviceFamilies = new List<DeviceFamily>();
            using (var connection = new SqlConnection(_configuration.GetConnectionString("InventoryTFS_Prod2Connection")))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("PRD_Device_P_SearchDeviceFamily", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@CustomerId", customerId));
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            deviceFamilies.Add(new DeviceFamily { DeviceFamilyId = reader.GetInt32(0), DeviceFamilyName = reader.GetString(1) });
                        }
                    }
                }
            }
            return deviceFamilies;
        }

        public async Task<List<Device>> GetDevicesAsync(int customerId, int deviceFamilyId)
        {
            var devices = new List<Device>();
            using (var connection = new SqlConnection(_configuration.GetConnectionString("InventoryConnection")))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("PRD_Device_P_GetDevices", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CustomerId", customerId);
                    command.Parameters.AddWithValue("@DeviceFamilyId", deviceFamilyId);
                    command.Parameters.AddWithValue("@DeviceId", DBNull.Value);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            devices.Add(new Device { DeviceId = reader.GetInt32(0), DeviceName = reader.GetString(3) });
                        }
                    }
                }
            }
            return devices;
        }

        public async Task<List<ServiceCategory>> GetInventoryReceiptServiceCategoryAsync(string ListName)
        {
            var statuses = new List<ServiceCategory>();
            using (var connection = new SqlConnection(_configuration.GetConnectionString("InventoryTFS_Prod2Connection")))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("PRD_P_GetMasterListItems", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ListName", ListName);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            statuses.Add(new ServiceCategory { ServiceCategoryId = reader.GetInt32(0), ServiceCategoryName = reader.GetString(3) });
                        }
                    }
                }
            }
            return statuses;
        }

        public async Task<List<LotOwners>> GetInventoryReceiptLotOwnersAsync()
        {
            var lotOwners = new List<LotOwners>();
            using (var connection = new SqlConnection(_configuration.GetConnectionString("InventoryTFS_Prod2Connection")))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("PRD_Lot_P_GetLotOwnersList", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            lotOwners.Add(new LotOwners { employeeID = reader.GetInt32(0), employeeName = reader.GetString(2) });
                        }
                    }
                }
            }
            return lotOwners;
        }

        public async Task<List<TrayVendor>> GetInventoryReceiptTrayVendorAsync(int customerId)
        {
            var trayVendor = new List<TrayVendor>();

            using (var connection = new SqlConnection(_configuration.GetConnectionString("InventoryConnection")))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("INV_GetTrayVendor", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CustomerId", customerId);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            trayVendor.Add(new TrayVendor
                            {
                                TrayVendorId = reader.GetInt32(0),
                                VendorName = reader.GetString(1)
                            });
                        }
                    }
                }
            }

            return trayVendor;
        }

        public async Task<List<TrayPart>> GetInventoryReceiptTraysByVendorIdAsync(int customerId, int deviceFamilyId)
        {
            var trayPart = new List<TrayPart>();
            using (var connection = new SqlConnection(_configuration.GetConnectionString("InventoryConnection")))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("INV_GetTrays_ByVendorId", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CustomerId", customerId);
                    command.Parameters.AddWithValue("@VendorId", deviceFamilyId);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            trayPart.Add(new TrayPart { TrayPartId = reader.GetInt32(0), TrayNumber = reader.GetString(2) });
                        }
                    }
                }
            }
            return trayPart;
        }

        public async Task<List<PurchaseOrder>> GetPurchaseOrdersAsync(int customerId, int? divisionId, bool? isFreezed)
        {
            var purchaseOrders = new List<PurchaseOrder>();
            using (var connection = new SqlConnection(_configuration.GetConnectionString("InventoryTFS_Prod2Connection")))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("tfsp_GetPurchaseOrdersByCustomerId", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CustomerId", customerId);
                    command.Parameters.AddWithValue("@DivisionId", divisionId ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@IsFreezed", isFreezed ?? (object)DBNull.Value);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            purchaseOrders.Add(new PurchaseOrder
                            {
                                PurchaseOrderId = reader["PurchaseOrderId"] != DBNull.Value ? Convert.ToInt32(reader["PurchaseOrderId"]) : 0,
                                CustomerPoNumber = reader["CustomerPoNumber"] != DBNull.Value ? reader["CustomerPoNumber"].ToString() : string.Empty
                            });
                        }
                    }
                }
            }
            return purchaseOrders;
        }

        public async Task<List<PackageCategory>> GetPackageCategoryAsync(string categoryName)
        {
            var list = new List<PackageCategory>();
            using (var connection = new SqlConnection(_configuration.GetConnectionString("InventoryConnection")))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("INV_GetPackageCategory", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Type", categoryName);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            list.Add(new PackageCategory
                            {
                                Id = reader["Id"] != DBNull.Value ? Convert.ToInt32(reader["Id"]) : 0,
                                CategoryName = reader["CategoryName"] != DBNull.Value ? reader["CategoryName"].ToString() : string.Empty
                            });
                        }
                    }
                }
            }
            return list;
        }

        public async Task<List<Quotes>> GetQuotesAsync(int customerId)
        {
            var list = new List<Quotes>();

            using (var connection = new SqlConnection(_configuration.GetConnectionString("InventoryTFS_Prod2Connection")))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("sosp_GetQuotes", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@CustomerId", customerId);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            list.Add(new Quotes
                            {
                                QuoteId = reader.GetString(1),
                                Quote = reader.GetString(2)
                            });
                        }
                    }
                }
            }

            return list;
        }

        public async Task<List<ServiceCaetgory>> GetServiceCaetgoryAsync()
        {
            var list = new List<ServiceCaetgory>();
            using (var connection = new SqlConnection(_configuration.GetConnectionString("InventoryConnection")))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("INV_Get_ServiceCaetgory", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            list.Add(new ServiceCaetgory { ServiceCategoryId = reader.GetInt32(0), ServiceCategoryName = reader.GetString(1) });
                        }
                    }
                }
            }
            return list;
        }

        public async Task<(int ReturnCode, string ReturnMessage)> SaveMailRoomInfoAsync(int? mailId, int loginId, string mailJson, string packageLabelJson, string shipmentPaperJson)
        {
            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("InventoryConnection")))
                {
                    await connection.OpenAsync();
                    using (var cmd = new SqlCommand("usp_UpsertMailRoom", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MailId", SqlDbType.Int) { Value = (object?)mailId ?? DBNull.Value });
                        cmd.Parameters.Add(new SqlParameter("@MailJson", SqlDbType.NVarChar) { Value = mailJson });
                        cmd.Parameters.Add(new SqlParameter("@LoginId", SqlDbType.Int) { Value = loginId });
                        cmd.Parameters.Add(new SqlParameter("@PackageLabelJson", SqlDbType.NVarChar) { Value = packageLabelJson });
                        cmd.Parameters.Add(new SqlParameter("@ShipmentPaperJson", SqlDbType.NVarChar) { Value = shipmentPaperJson });
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                int returnCode = reader["ReturnCode"] != DBNull.Value ? Convert.ToInt32(reader["ReturnCode"]) : 0;
                                string returnMessage = reader["ReturnString"]?.ToString() ?? "Unknown error";
                                return (returnCode, returnMessage);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return (-1, $"Error: {ex.Message}");
            }
            return (-1, "No response from stored procedure");
        }

        public async Task<(int ReturnCode, string ReturnMessage)> ValidateMailRoomInfoAsync(int? mailId, string mailJson)
        {
            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("InventoryConnection")))
                {
                    await connection.OpenAsync();
                    using (var cmd = new SqlCommand("Inv_ValidateMailRoom", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MailId", SqlDbType.Int) { Value = (object?)mailId ?? DBNull.Value });
                        cmd.Parameters.Add(new SqlParameter("@MailJson", SqlDbType.NVarChar) { Value = mailJson });
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                int returnCode = reader["ReturnCode"] != DBNull.Value ? Convert.ToInt32(reader["ReturnCode"]) : 0;
                                string returnMessage = reader["ReturnString"]?.ToString() ?? "Validation failed";
                                return (returnCode, returnMessage);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return (-1, $"Validation error: {ex.Message}");
            }
            return (-1, "No response from validation procedure");
        }

        public async Task<(int ReturnCode, string ReturnMessage)> SaveInventoryReceiptAsync(int? customerId, int loginId, string receiptJson, string attachmentsJson)
        {
            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("InventoryConnection")))
                {
                    using (var command = new SqlCommand("usp_UpsertReceipt", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@ReceiptId", customerId);
                        command.Parameters.AddWithValue("@LoginId", loginId);
                        command.Parameters.AddWithValue("@ReceiptJson ", receiptJson);
                        command.Parameters.AddWithValue("@AttachmentsJson ", attachmentsJson);
                        await connection.OpenAsync();
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                int returnCode = reader["ReturnCode"] != DBNull.Value ? Convert.ToInt32(reader["ReturnCode"]) : 0;
                                string returnMessage = reader["ReturnString"]?.ToString() ?? "Unknown error";
                                return (returnCode, returnMessage);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return (-1, $"Error: {ex.Message}");
            }
            return (-1, "No response from stored procedure");
        }

        public async Task<List<MailRoomStatusList>> GetMailRoomStatusListAsync(string listName)
        {
            var list = new List<MailRoomStatusList>();
            using (var connection = new SqlConnection(_configuration.GetConnectionString("InventoryConnection")))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("PRD_P_GetMasterListItems", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ListName", listName);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            list.Add(new MailRoomStatusList
                            {
                                MasterListItemId = reader["MasterListItemId"] != DBNull.Value ? Convert.ToInt32(reader["MasterListItemId"]) : 0,
                                ItemText = reader["ItemText"] != DBNull.Value ? reader["ItemText"].ToString() : string.Empty
                            });
                        }
                    }
                }
            }
            return list;
        }

        public async Task<List<MailReceiptSearchResult>> SearchMailReceiptAsync(string? status, DateTime? fromDate, DateTime? toDate)
        {
            var result = new List<MailReceiptSearchResult>();
            using (var connection = new SqlConnection(_configuration.GetConnectionString("InventoryConnection")))
            {
                using (var command = new SqlCommand("usp_SearchMailReceipt", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Status", (object?)status ?? DBNull.Value);
                    command.Parameters.AddWithValue("@FromDate", (object?)fromDate ?? DBNull.Value);
                    command.Parameters.AddWithValue("@ToDate", (object?)toDate ?? DBNull.Value);
                    await connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            result.Add(new MailReceiptSearchResult
                            {
                                MailId = reader.IsDBNull(reader.GetOrdinal("MailId")) ? null : reader.GetInt32(reader.GetOrdinal("MailId")),
                                Customer = reader.IsDBNull(reader.GetOrdinal("Customer")) ? null : reader.GetString(reader.GetOrdinal("Customer")),
                                MailNo = reader.IsDBNull(reader.GetOrdinal("MailNo")) ? null : reader.GetString(reader.GetOrdinal("MailNo")),
                                ReceivingNo = reader.IsDBNull(reader.GetOrdinal("ReceivingNo")) ? null : reader.GetString(reader.GetOrdinal("ReceivingNo")),
                                CreateDateTime = reader.IsDBNull(reader.GetOrdinal("CreateDateTime")) ? null : reader.GetDateTime(reader.GetOrdinal("CreateDateTime")),
                                Recipient = reader.IsDBNull(reader.GetOrdinal("Recipient")) ? null : reader.GetString(reader.GetOrdinal("Recipient")),
                                DeliveryMethod = reader.IsDBNull(reader.GetOrdinal("DeliveryMethod")) ? null : reader.GetString(reader.GetOrdinal("DeliveryMethod")),
                                ExpectedDate = reader.IsDBNull(reader.GetOrdinal("ExpectedDate")) ? null : reader.GetDateTime(reader.GetOrdinal("ExpectedDate")),
                                NoOfPackages = reader.IsDBNull(reader.GetOrdinal("NoOfPackages")) ? null : reader.GetInt32(reader.GetOrdinal("NoOfPackages")),
                                Status = reader.IsDBNull(reader.GetOrdinal("Status")) ? null : reader.GetString(reader.GetOrdinal("Status")),
                                Received = reader.IsDBNull(reader.GetOrdinal("Received")) ? null : reader.GetString(reader.GetOrdinal("Received")),
                                Damage = reader.IsDBNull(reader.GetOrdinal("Damage")) ? null : reader.GetString(reader.GetOrdinal("Damage")),
                                PartialDelivery = reader.IsDBNull(reader.GetOrdinal("PartialDelivery")) ? null : reader.GetString(reader.GetOrdinal("PartialDelivery")),
                                Sendor = reader.IsDBNull(reader.GetOrdinal("Sendor")) ? null : reader.GetString(reader.GetOrdinal("Sendor")),
                                AWB = reader.IsDBNull(reader.GetOrdinal("AWB")) ? null : reader.GetString(reader.GetOrdinal("AWB")),
                                TrackingNumber = reader.IsDBNull(reader.GetOrdinal("TrackingNumber")) ? null : reader.GetString(reader.GetOrdinal("TrackingNumber")),
                                Location = reader.IsDBNull(reader.GetOrdinal("Location")) ? null : reader.GetString(reader.GetOrdinal("Location")),
                                CreatedBy = reader.IsDBNull(reader.GetOrdinal("CreatedBy")) ? null : reader.GetString(reader.GetOrdinal("CreatedBy")),
                                Lot = reader.IsDBNull(reader.GetOrdinal("Lot")) ? null : reader.GetString(reader.GetOrdinal("Lot")),
                                CustomerLot = reader.IsDBNull(reader.GetOrdinal("CustomerLot")) ? null : reader.GetString(reader.GetOrdinal("CustomerLot")),
                                CanEdit = reader["CanEdit"] != DBNull.Value ? Convert.ToBoolean(reader["CanEdit"]) : null,
                            });
                        }
                    }
                }
            }
            return result;
        }

        public async Task<MailRoomDetails> GetMailRoomDetailsAsync(int mailId)
        {
            // Implementation continues - truncated for brevity
            throw new NotImplementedException("Full implementation needed");
        }

        public async Task<ReceiptFullDetailDto> GetReceiverFormInternalListAsync(int? receiptId, int? mailId)
        {
            // Implementation continues - truncated for brevity
            throw new NotImplementedException("Full implementation needed");
        }

        public async Task<List<ReceivingSearchResult>> SearchReceivingAsync(string? receivingTypes, DateTime? fromDate, DateTime? toDate, string? statusIds)
        {
            var results = new List<ReceivingSearchResult>();
            using (var connection = new SqlConnection(_configuration.GetConnectionString("InventoryConnection")))
            {
                using (var command = new SqlCommand("usp_SearchReceiving", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ReceivingTypes", (object?)receivingTypes ?? DBNull.Value);
                    command.Parameters.AddWithValue("@FromDate", (object?)fromDate ?? DBNull.Value);
                    command.Parameters.AddWithValue("@ToDate", (object?)toDate ?? DBNull.Value);
                    command.Parameters.AddWithValue("@StatusIds", (object?)statusIds ?? DBNull.Value);
                    await connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            results.Add(new ReceivingSearchResult
                            {
                                ReceiptId = reader.IsDBNull("ReceiptId") ? null : reader.GetInt32(reader.GetOrdinal("ReceiptId")),
                                ReceivingNo = reader.IsDBNull("ReceivingNo") ? null : reader.GetString(reader.GetOrdinal("ReceivingNo")),
                                MailNo = reader.IsDBNull("MailNo") ? null : reader.GetString(reader.GetOrdinal("MailNo")),
                                CustomerName = reader.IsDBNull("CustomerName") ? null : reader.GetString(reader.GetOrdinal("CustomerName")),
                            });
                        }
                    }
                }
            }
            return results;
        }

        public async Task SaveReceivingAsync(string jsonData, int receiptId, int loginId)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("InventoryConnection")))
            {
                using (var command = new SqlCommand("INV_UpdateReceiving", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@json_data", jsonData ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@receiptId", receiptId);
                    command.Parameters.AddWithValue("@LoginId", loginId);
                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<List<CustomersLogin>> GetCustomersLoginIdAsync(int loginId)
        {
            var list = new List<CustomersLogin>();
            using (var connection = new SqlConnection(_configuration.GetConnectionString("InventoryConnection")))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("Inv_Customers_LoginId", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@LoginId", loginId);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            list.Add(new CustomersLogin
                            {
                                CustomerId = reader["CustomerId"] != DBNull.Value ? Convert.ToInt32(reader["CustomerId"]) : 0,
                                CustomerName = reader["CustomerName"] != DBNull.Value ? reader["CustomerName"].ToString() : string.Empty
                            });
                        }
                    }
                }
            }
            return list;
        }

        public async Task<ReceivingDetails?> GetReceivingByIdAsync(int receiptId)
        {
            // Implementation continues - truncated for brevity
            throw new NotImplementedException("Full implementation needed");
        }

        public async Task<List<Interim>> GetSearchInterimCustomerIdAsync(int receiptId, int customerId)
        {
            var list = new List<Interim>();
            using (var connection = new SqlConnection(_configuration.GetConnectionString("InventoryTFS_Prod2Connection")))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("INV_Rec_P_SearchInterimShippingForMail", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ReceiptId", receiptId);
                    command.Parameters.AddWithValue("@CustomerId", customerId);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            list.Add(new Interim
                            {
                                LotId = reader["LotId"] != DBNull.Value ? Convert.ToInt32(reader["LotId"]) : 0,
                                InterimShippingId = reader["InterimShippingId"] != DBNull.Value ? Convert.ToInt32(reader["InterimShippingId"]) : 0,
                                LotNumber = reader["LotNumber"] != DBNull.Value ? reader["LotNumber"].ToString() : string.Empty,
                                CustomerLotNumber = reader["CustomerLotNumber"] != DBNull.Value ? reader["CustomerLotNumber"].ToString() : string.Empty,
                                CustomerId = reader["CustomerId"] != DBNull.Value ? Convert.ToInt32(reader["CustomerId"]) : 0,
                                CustomerName = reader["CustomerName"] != DBNull.Value ? reader["CustomerName"].ToString() : string.Empty,
                            });
                        }
                    }
                }
            }
            return list;
        }
    }
}

