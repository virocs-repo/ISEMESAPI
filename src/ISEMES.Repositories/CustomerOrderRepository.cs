using ISEMES.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System;

namespace ISEMES.Repositories
{
    public class CustomerOrderRepository : ICustomerOrderRepository
    {
        private readonly AppDbContext _context;
        public CustomerOrderRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CustOrder>> GetCustomerOrdersAsync(DateTime? fromDate = null, DateTime? toDate = null)
        {
            var query = "EXEC usp_GetCustomerOrders @FromDate = {0}, @ToDate = {1}";
            var result = await _context.CustomerOrders
                .FromSqlRaw(query, fromDate ?? (object)DBNull.Value, toDate ?? (object)DBNull.Value)
                .ToListAsync();
            return result;
        }

        public async Task<IEnumerable<CustomerInventory>> GetCustomerInventoryAsync(string goodsType, string lotNumber, string customerOrderType, int? customerId = null)
        {
            var inventoryList = new List<CustomerInventory>();

            using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("usp_GetCustomerInventory", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CustomerId", customerId.HasValue ? customerId.Value : DBNull.Value);
                    command.Parameters.AddWithValue("@GoodsType", goodsType);
                    command.Parameters.AddWithValue("@LotNumber", string.IsNullOrEmpty(lotNumber) || string.Equals("null", lotNumber.ToLower()) ? DBNull.Value : lotNumber);
                    command.Parameters.AddWithValue("@CustomerOrderType", customerOrderType);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var inventory = new CustomerInventory
                            {
                                ReceiptID = reader.GetInt32(0),
                                GoodType = reader.IsDBNull(1) ? null : reader.GetString(1),
                                InventoryID = reader.GetInt32(2),
                                HardwareTypeId = reader.IsDBNull(3) ? null : reader.GetInt32(3),
                                HardwareType = reader.IsDBNull(4) ? null : reader.GetString(4),
                                ISELotNum = reader.GetString(5),
                                CustomerLotNum = reader.IsDBNull(6) ? null : reader.GetString(6),
                                ExpectedQty = reader.GetInt32(7),
                                Expedite = reader.IsDBNull(8) ? (bool?)null : reader.GetBoolean(8),
                                PartNum = reader.IsDBNull(9) ? null : reader.GetString(9),
                                LabelCount = reader.IsDBNull(reader.GetOrdinal("LabelCount")) ? 0 : reader.GetInt32(reader.GetOrdinal("LabelCount")),
                                COO = reader.IsDBNull(11) ? null : reader.GetString(11),
                                DateCode = reader.IsDBNull(12) ? null : reader.GetString(12),
                                IsHold = reader.IsDBNull(13) ? (bool?)null : reader.GetBoolean(13),
                                HoldComments = reader.IsDBNull(14) ? null : reader.GetString(14),
                                AdditionalInfo = reader.IsDBNull(15) ? null : reader.GetString(15),
                                ShippedQty = reader.IsDBNull(16) ? null : reader.GetInt32(16),
                                CreatedOn = reader.GetDateTime(17),
                                ModifiedOn = reader.GetDateTime(18),
                                Active = reader.GetBoolean(19)
                            };
                            inventoryList.Add(inventory);
                        }
                    }
                }
            }
            return inventoryList;
        }

        public async Task<bool> ProcessCustomerOrderAsync(int loginId, OrderRequestWrapper orderRequest)
        {
            using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
            {
                await connection.OpenAsync();
                using (SqlCommand cmd = new SqlCommand("usp_UpsertCustomerOrder", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@LoginId", SqlDbType.Int) { Value = loginId });
                    cmd.Parameters.Add(new SqlParameter("@InputJSON", SqlDbType.NVarChar) { Value = orderRequest.InputJSON });
                    var result = await cmd.ExecuteScalarAsync();
                    return result != null && result.ToString().ToLower() == "success";
                }
            }
        }

        public async Task<List<CustomerEditDetail>> GetVieweditorder(int customerOrderID)
        {
            var customerOrderDetails = new List<CustomerEditDetail>();

            using (var conn = new SqlConnection(_context.Database.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand("usp_GetCustomerOrderDetail", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CustomerOrderID", customerOrderID);
                    await conn.OpenAsync();

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var detail = new CustomerEditDetail
                            {
                                CustomerOrderID = reader.IsDBNull(0) ? (int?)null : reader.GetInt32(0),
                                CustomerOrderDetailID = reader.IsDBNull(1) ? (int?)null : reader.GetInt32(1),
                                GoodsType = reader.IsDBNull(2) ? null : reader.GetString(2),
                                InventoryID = reader.GetInt32(3),
                                ISELotNum = reader.IsDBNull(4) ? null : reader.GetString(4),
                                CustomerLotNum = reader.IsDBNull(5) ? null : reader.GetString(5),
                                ExpectedQty = reader.GetInt32(6),
                                Expedite = reader.IsDBNull(7) ? (bool?)null : reader.GetBoolean(7),
                                PartNum = reader.IsDBNull(8) ? null : reader.GetString(8),
                                Unprocessed = reader.GetInt32(9),
                                Good = reader.GetInt32(10),
                                Reject = reader.GetInt32(11),
                                COO = reader.IsDBNull(12) ? null : reader.GetString(12),
                                DateCode = reader.IsDBNull(13) ? null : reader.GetString(13),
                                ShippedQty = reader.IsDBNull(14) ? (int?)null : reader.GetInt32(14)
                            };
                            customerOrderDetails.Add(detail);
                        }
                    }
                }
            }
            return customerOrderDetails;
        }

        public async Task<IEnumerable<string>> GetInventoryLots(int? customerId = null)
        {
            var lotNumList = new List<string>();
            using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("usp_GetInventoryLot", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CustomerID", customerId.HasValue ? customerId.Value : DBNull.Value);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            lotNumList.Add(reader.GetString(0));
                        }
                    }
                }
            }
            return lotNumList;
        }

        public async Task<IEnumerable<MasterListItem>> GetMasterListItemsAsync(string listName, int? serviceId)
        {
            var masterList = new List<MasterListItem>();
            using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("PRD_P_GetMasterListItems", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ListName", listName);
                    command.Parameters.AddWithValue("@ServiceId", serviceId.HasValue ? serviceId.Value : DBNull.Value);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            masterList.Add(new MasterListItem
                            {
                                MasterListItemId = reader.GetInt32(0),
                                ItemText = reader.GetString(2)
                            });
                        }
                    }
                }
            }
            return masterList;
        }

        public async Task<IEnumerable<MasterListItem>> GetListItemsAsync(string listName, int? parentId)
        {
            var masterList = new List<MasterListItem>();
            using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("PRD_Shipping_P_GetMasterListItems", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ListName", listName);
                    command.Parameters.AddWithValue("@ParentId", parentId.HasValue ? parentId.Value : DBNull.Value);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            masterList.Add(new MasterListItem
                            {
                                MasterListItemId = reader.GetInt32(0),
                                ItemText = reader.GetString(2),
                                Parent = reader.IsDBNull(4) ? (int?)null : reader.GetInt32(4)
                            });
                        }
                    }
                }
            }
            return masterList;
        }

        public async Task<List<ShippingAddress>> GetShippingAddressesAsync(int customerId, bool isBilling, int? vendorId, int? courierId, bool isDomestic)
        {
            var addresses = new List<ShippingAddress>();
            try
            {
                using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("PRD_P_GetShippingAddress", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@CustomerId", customerId);
                        command.Parameters.AddWithValue("@IsBilling", isBilling ? 1 : 0);
                        command.Parameters.AddWithValue("@VendorId", vendorId ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@CourierId", courierId ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@IsDomestic", isDomestic ? 1 : 0);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                addresses.Add(new ShippingAddress
                                {
                                    AddressId = reader.IsDBNull(reader.GetOrdinal("AddressId")) ? 0 : reader.GetInt32(reader.GetOrdinal("AddressId")),
                                    AddressType = reader.IsDBNull(reader.GetOrdinal("AddressType")) ? string.Empty : reader.GetString(reader.GetOrdinal("AddressType")),
                                    ContactPerson = reader.IsDBNull(reader.GetOrdinal("ContactPerson")) ? string.Empty : reader.GetString(reader.GetOrdinal("ContactPerson")),
                                    CompanyName = reader.IsDBNull(reader.GetOrdinal("CompanyName")) ? string.Empty : reader.GetString(reader.GetOrdinal("CompanyName")),
                                    Address1 = reader.IsDBNull(reader.GetOrdinal("Address1")) ? string.Empty : reader.GetString(reader.GetOrdinal("Address1")),
                                    Address2 = reader.IsDBNull(reader.GetOrdinal("Address2")) ? string.Empty : reader.GetString(reader.GetOrdinal("Address2")),
                                    Address3 = reader.IsDBNull(reader.GetOrdinal("Address3")) ? string.Empty : reader.GetString(reader.GetOrdinal("Address3")),
                                    City = reader.IsDBNull(reader.GetOrdinal("City")) ? string.Empty : reader.GetString(reader.GetOrdinal("City")),
                                    State = reader.IsDBNull(reader.GetOrdinal("State")) ? string.Empty : reader.GetString(reader.GetOrdinal("State")),
                                    Zip = reader.IsDBNull(reader.GetOrdinal("Zip")) ? string.Empty : reader.GetString(reader.GetOrdinal("Zip")),
                                    CountryId = reader.IsDBNull(reader.GetOrdinal("CountryId")) ? 0 : reader.GetInt32(reader.GetOrdinal("CountryId")),
                                    Country = reader.IsDBNull(reader.GetOrdinal("Country")) ? string.Empty : reader.GetString(reader.GetOrdinal("Country")),
                                    Phone = reader.IsDBNull(reader.GetOrdinal("Phone")) ? string.Empty : reader.GetString(reader.GetOrdinal("Phone")),
                                    Extension = reader.IsDBNull(reader.GetOrdinal("Extension")) ? string.Empty : reader.GetString(reader.GetOrdinal("Extension")),
                                    ShipTo = reader.IsDBNull(reader.GetOrdinal("ShipTo")) ? string.Empty : reader.GetString(reader.GetOrdinal("ShipTo")),
                                    AEType = reader.IsDBNull(reader.GetOrdinal("AEType")) ? string.Empty : reader.GetString(reader.GetOrdinal("AEType")),
                                    Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? string.Empty : reader.GetString(reader.GetOrdinal("Email")),
                                    CustomerId = reader.IsDBNull(reader.GetOrdinal("CustomerId")) ? 0 : reader.GetInt32(reader.GetOrdinal("CustomerId")),
                                    CustomerName = reader.IsDBNull(reader.GetOrdinal("CustomerName")) ? string.Empty : reader.GetString(reader.GetOrdinal("CustomerName")),
                                    Address = reader.IsDBNull(reader.GetOrdinal("Address")) ? string.Empty : reader.GetString(reader.GetOrdinal("Address")),
                                    PickUpAddress = reader.IsDBNull(reader.GetOrdinal("PickUpAddress")) ? string.Empty : reader.GetString(reader.GetOrdinal("PickUpAddress")),
                                    CanSelect = reader.IsDBNull(reader.GetOrdinal("CanSelect")) ? false : reader.GetInt32(reader.GetOrdinal("CanSelect")) == 1,
                                    Alert = reader.IsDBNull(reader.GetOrdinal("Alert")) ? null : reader.GetString(reader.GetOrdinal("Alert")),
                                    IsDomestic = reader.IsDBNull(reader.GetOrdinal("IsDomestic")) ? false : reader.GetBoolean(reader.GetOrdinal("IsDomestic"))
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving shipping addresses: {ex.Message}. Inner exception: {ex.InnerException?.Message}", ex);
            }
            return addresses;
        }

        public async Task<List<ContactPersonDetails>> GetContactPersonDetailsAsync(int customerId, int? shippingContactId)
        {
            var contactPersonDetailsList = new List<ContactPersonDetails>();
            using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("PRD_Shipping_AddressBook_P_GetByCustomerId", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CusomerId", customerId);
                    command.Parameters.AddWithValue("@ShippingContactId", shippingContactId ?? (object)DBNull.Value);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            contactPersonDetailsList.Add(new ContactPersonDetails
                            {
                                ShippingContactId = reader.IsDBNull(reader.GetOrdinal("ShippingContactId")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("ShippingContactId")),
                                ShippingContactName = reader.IsDBNull(reader.GetOrdinal("ShippingContactName")) ? null : reader.GetString(reader.GetOrdinal("ShippingContactName")),
                                CustomerId = reader.IsDBNull(reader.GetOrdinal("CustomerId")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("CustomerId")),
                                ShippingMethodId = reader.IsDBNull(reader.GetOrdinal("ShippingMethodId")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("ShippingMethodId")),
                                AddressId = reader.IsDBNull(reader.GetOrdinal("AddressId")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("AddressId")),
                                ContactPerson = reader.IsDBNull(reader.GetOrdinal("ContactPerson")) ? null : reader.GetString(reader.GetOrdinal("ContactPerson")),
                                CompanyName = reader.IsDBNull(reader.GetOrdinal("CompanyName")) ? null : reader.GetString(reader.GetOrdinal("CompanyName")),
                                Address1 = reader.IsDBNull(reader.GetOrdinal("Address1")) ? null : reader.GetString(reader.GetOrdinal("Address1")),
                                Address2 = reader.IsDBNull(reader.GetOrdinal("Address2")) ? null : reader.GetString(reader.GetOrdinal("Address2")),
                                Address3 = reader.IsDBNull(reader.GetOrdinal("Address3")) ? null : reader.GetString(reader.GetOrdinal("Address3")),
                                Country = reader.IsDBNull(reader.GetOrdinal("Country")) ? null : reader.GetString(reader.GetOrdinal("Country")),
                                Zip = reader.IsDBNull(reader.GetOrdinal("Zip")) ? null : reader.GetString(reader.GetOrdinal("Zip")),
                                StateProvince = reader.IsDBNull(reader.GetOrdinal("StateProvince")) ? null : reader.GetString(reader.GetOrdinal("StateProvince")),
                                City = reader.IsDBNull(reader.GetOrdinal("City")) ? null : reader.GetString(reader.GetOrdinal("City")),
                                Phone = reader.IsDBNull(reader.GetOrdinal("Phone")) ? null : reader.GetString(reader.GetOrdinal("Phone")),
                                Extension = reader.IsDBNull(reader.GetOrdinal("Extension")) ? null : reader.GetString(reader.GetOrdinal("Extension")),
                                Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString(reader.GetOrdinal("Email")),
                                CourierId = reader.IsDBNull(reader.GetOrdinal("CourierId")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("CourierId")),
                                AWBOrTracking = reader.IsDBNull(reader.GetOrdinal("AWBOrTracking")) ? null : reader.GetString(reader.GetOrdinal("AWBOrTracking")),
                                ExpectedTime = reader.IsDBNull(reader.GetOrdinal("ExpectedTime")) ? null : reader.GetString(reader.GetOrdinal("ExpectedTime")),
                                Comments = reader.IsDBNull(reader.GetOrdinal("Comments")) ? null : reader.GetString(reader.GetOrdinal("Comments")),
                                DropOffCity = reader.IsDBNull(reader.GetOrdinal("DropOffCity")) ? null : reader.GetString(reader.GetOrdinal("DropOffCity")),
                                SendToCountry = reader.IsDBNull(reader.GetOrdinal("SendToCountry")) ? null : reader.GetString(reader.GetOrdinal("SendToCountry")),
                                DestinationId = reader.IsDBNull(reader.GetOrdinal("DestinationId")) ? (int?)null : (reader.GetInt32(reader.GetOrdinal("DestinationId")) < 0 ? (int?)null : reader.GetInt32(reader.GetOrdinal("DestinationId"))),
                                ServiceType = reader.IsDBNull(reader.GetOrdinal("ServiceType")) ? null : reader.GetString(reader.GetOrdinal("ServiceType")),
                                AccountNumber = reader.IsDBNull(reader.GetOrdinal("AccountNumber")) ? null : reader.GetString(reader.GetOrdinal("AccountNumber")),
                                BillTransportationTo = reader.IsDBNull(reader.GetOrdinal("BillTransportationTo")) ? null : reader.GetString(reader.GetOrdinal("BillTransportationTo")),
                                BillTransportationAcct = reader.IsDBNull(reader.GetOrdinal("BillTransportationAcct")) ? null : reader.GetString(reader.GetOrdinal("BillTransportationAcct")),
                                BillDutyTaxFeesTo = reader.IsDBNull(reader.GetOrdinal("BillDutyTaxFeesTo")) ? null : reader.GetString(reader.GetOrdinal("BillDutyTaxFeesTo")),
                                BillDutyTaxFeesAcct = reader.IsDBNull(reader.GetOrdinal("BillDutyTaxFeesAcct")) ? null : reader.GetString(reader.GetOrdinal("BillDutyTaxFeesAcct")),
                                CommodityDescription = reader.IsDBNull(reader.GetOrdinal("CommodityDescription")) ? null : reader.GetString(reader.GetOrdinal("CommodityDescription")),
                                ShipDate = reader.IsDBNull(reader.GetOrdinal("ShipDate")) ? null : reader.GetString(reader.GetOrdinal("ShipDate")),
                                Active = reader.IsDBNull(reader.GetOrdinal("Active")) ? (bool?)null : reader.GetBoolean(reader.GetOrdinal("Active")),
                                ShipAlertEmail = reader.IsDBNull(reader.GetOrdinal("ShipAlertEmail")) ? null : reader.GetString(reader.GetOrdinal("ShipAlertEmail")),
                                UnitValue = reader.IsDBNull(reader.GetOrdinal("UnitValue")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("UnitValue")),
                                LicenseType = reader.IsDBNull(reader.GetOrdinal("LicenseType")) ? null : reader.GetString(reader.GetOrdinal("LicenseType")),
                                CustomsTermsOfTradeId = reader.IsDBNull(reader.GetOrdinal("CustomsTermsOfTradeId")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("CustomsTermsOfTradeId")),
                                IsForwarder = reader.IsDBNull(reader.GetOrdinal("IsForwarder")) ? (bool?)null : reader.GetBoolean(reader.GetOrdinal("IsForwarder"))
                            });
                        }
                    }
                }
            }
            return contactPersonDetailsList;
        }
    }
}

