using ISEMES.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System;

namespace ISEMES.Repositories
{
    public class ProbeCardRepository : IProbeCardRepository
    {
        private readonly IConfiguration _configuration;
        private const string ConnectionStringName = "InventoryTFS_Prod2Connection";

        public ProbeCardRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private string GetConnectionString()
        {
            return _configuration.GetConnectionString(ConnectionStringName) 
                ?? throw new InvalidOperationException($"Connection string '{ConnectionStringName}' not found.");
        }

        public async Task<DataTable> SearchProbeCard(ProbeCardSearchRequest request)
        {
            using (var connection = new SqlConnection(GetConnectionString()))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("HW_SearchProbeCard", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    
                    // CustomerId
                    if (request.CustomerId.HasValue && request.CustomerId.Value != -1)
                        command.Parameters.AddWithValue("@CustomerId", request.CustomerId.Value);
                    else
                        command.Parameters.AddWithValue("@CustomerId", DBNull.Value);
                    
                    // ISEId
                    if (!string.IsNullOrEmpty(request.ISEId))
                        command.Parameters.AddWithValue("@ISEId", request.ISEId);
                    else
                        command.Parameters.AddWithValue("@ISEId", DBNull.Value);
                    
                    // Customer_NO
                    if (!string.IsNullOrEmpty(request.CustomerHWId))
                        command.Parameters.AddWithValue("@Customer_NO", request.CustomerHWId);
                    else
                        command.Parameters.AddWithValue("@Customer_NO", DBNull.Value);
                    
                    // PlatformId
                    if (request.PlatformId.HasValue && request.PlatformId.Value != -1)
                        command.Parameters.AddWithValue("@PlatformId", request.PlatformId.Value);
                    else
                        command.Parameters.AddWithValue("@PlatformId", DBNull.Value);
                    
                    // ProbeType
                    if (request.ProbeCardTypeId.HasValue && request.ProbeCardTypeId.Value != -1)
                        command.Parameters.AddWithValue("@ProbeType", request.ProbeCardTypeId.Value);
                    else
                        command.Parameters.AddWithValue("@ProbeType", DBNull.Value);
                    
                    // EquipmentId
                    if (request.EquipmentId.HasValue && request.EquipmentId.Value != -1)
                        command.Parameters.AddWithValue("@EquipmentId", request.EquipmentId.Value);
                    else
                        command.Parameters.AddWithValue("@EquipmentId", DBNull.Value);
                    
                    // HardwareTypeId
                    command.Parameters.AddWithValue("@HardwareTypeId", request.HardwareTypeId);
                    
                    // IsExternal
                    command.Parameters.AddWithValue("@IsExternal", false);
                    
                    // Active
                    if (request.IsActive.HasValue && request.IsActive.Value != -1)
                        command.Parameters.AddWithValue("@Active", request.IsActive.Value);
                    else
                        command.Parameters.AddWithValue("@Active", DBNull.Value);
                    
                    // Note: BoardTypeId parameter removed as it's not in the stored procedure definition
                    // The stored procedure HW_SearchProbeCard only accepts 9 parameters (not 10)

                    using (var adapter = new SqlDataAdapter(command))
                    {
                        var dataSet = new DataSet();
                        // Use synchronous Fill - connection is properly disposed via using statement
                        adapter.Fill(dataSet);
                        return dataSet.Tables[0];
                    }
                }
            }
        }

        public async Task<DataSet> GetProbeCardDetails(int masterId)
        {
            using (var connection = new SqlConnection(GetConnectionString()))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("HW_GetProbeCardDetails", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@MasterId", masterId);

                    using (var adapter = new SqlDataAdapter(command))
                    {
                        var dataSet = new DataSet();
                        // Use synchronous Fill - connection is properly disposed via using statement
                        adapter.Fill(dataSet);
                        return dataSet;
                    }
                }
            }
        }

        public async Task<DataTable> GetPlatforms()
        {
            using (var connection = new SqlConnection(GetConnectionString()))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("tfsp_GetPlatforms", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    using (var adapter = new SqlDataAdapter(command))
                    {
                        var dataSet = new DataSet();
                        // Use synchronous Fill - connection is properly disposed via using statement
                        adapter.Fill(dataSet);
                        return dataSet.Tables.Count > 0 ? dataSet.Tables[0] : new DataTable();
                    }
                }
            }
        }

        public async Task<DataTable> GetProbeCardTypes()
        {
            using (var connection = new SqlConnection(GetConnectionString()))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("HW_GetProbType", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    using (var adapter = new SqlDataAdapter(command))
                    {
                        var dataSet = new DataSet();
                        // Use synchronous Fill - connection is properly disposed via using statement
                        adapter.Fill(dataSet);
                        return dataSet.Tables.Count > 0 ? dataSet.Tables[0] : new DataTable();
                    }
                }
            }
        }

        public async Task<DataTable> GetProbers()
        {
            using (var connection = new SqlConnection(GetConnectionString()))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("HW_GetInterfaceProber", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    using (var adapter = new SqlDataAdapter(command))
                    {
                        var dataSet = new DataSet();
                        // Use synchronous Fill - connection is properly disposed via using statement
                        adapter.Fill(dataSet);
                        return dataSet.Tables.Count > 0 ? dataSet.Tables[0] : new DataTable();
                    }
                }
            }
        }

        public async Task<DataTable> GetBoardTypes()
        {
            using (var connection = new SqlConnection(GetConnectionString()))
            {
                await connection.OpenAsync();
                // Use HW_WS_GetBoardType as per TFS MasterInfoDAL.cs GetWSBoardTypes method
                // This matches the exact stored procedure name used in TFS for Probe Card Board Type functionality
                // Note: Board Type values in TFS include "Probe Card" and "Probe Board" (or "Probe Noard" if there's a typo in the database)
                using (var command = new SqlCommand("HW_WS_GetBoardType", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        using (var adapter = new SqlDataAdapter(command))
                        {
                            var dataSet = new DataSet();
                            // Use synchronous Fill - connection is properly disposed via using statement
                            adapter.Fill(dataSet);
                            return dataSet.Tables.Count > 0 ? dataSet.Tables[0] : new DataTable();
                        }
                    }
                    catch (SqlException ex)
                    {
                        // If HW_WS_GetBoardType doesn't exist, try HW_GetBoardType as fallback
                        if (ex.Message.Contains("Could not find stored procedure") || ex.Message.Contains("Invalid object name"))
                        {
                            using (var fallbackCommand = new SqlCommand("HW_GetBoardType", connection))
                            {
                                fallbackCommand.CommandType = CommandType.StoredProcedure;
                        using (var adapter = new SqlDataAdapter(fallbackCommand))
                        {
                            var dataSet = new DataSet();
                            // Use synchronous Fill - connection is properly disposed via using statement
                            adapter.Fill(dataSet);
                            return dataSet.Tables.Count > 0 ? dataSet.Tables[0] : new DataTable();
                        }
                            }
                        }
                        throw;
                    }
                }
            }
        }

        public async Task<DataTable> GetSlots(int? hardwareTypeId, string? platformId)
        {
            using (var connection = new SqlConnection(GetConnectionString()))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("HW_GetSlotByHType", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    
                    if (hardwareTypeId.HasValue && hardwareTypeId.Value > 0)
                        command.Parameters.AddWithValue("@HardWareTypeId", hardwareTypeId.Value);
                    else
                        command.Parameters.AddWithValue("@HardWareTypeId", DBNull.Value);
                    
                    if (!string.IsNullOrEmpty(platformId))
                        command.Parameters.AddWithValue("@PlatformId", platformId);
                    else
                        command.Parameters.AddWithValue("@PlatformId", DBNull.Value);
                    
                    using (var adapter = new SqlDataAdapter(command))
                    {
                        var dataSet = new DataSet();
                        adapter.Fill(dataSet);
                        return dataSet.Tables.Count > 0 ? dataSet.Tables[0] : new DataTable();
                    }
                }
            }
        }

        public async Task<DataTable> GetSubSlots(int slotId)
        {
            using (var connection = new SqlConnection(GetConnectionString()))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("HW_GetSlotDetails", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@SlotId", slotId);
                    
                    using (var adapter = new SqlDataAdapter(command))
                    {
                        var dataSet = new DataSet();
                        adapter.Fill(dataSet);
                        // Returns 2 tables, we need Table[1] for SubSlots
                        return dataSet.Tables.Count > 1 ? dataSet.Tables[1] : new DataTable();
                    }
                }
            }
        }

        public async Task<DataTable> GetLocationsInfo(int subSlotId)
        {
            using (var connection = new SqlConnection(GetConnectionString()))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("hw_getlocationdetails", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@SubSLotId", subSlotId);
                    
                    using (var adapter = new SqlDataAdapter(command))
                    {
                        var dataSet = new DataSet();
                        adapter.Fill(dataSet);
                        return dataSet.Tables.Count > 0 ? dataSet.Tables[0] : new DataTable();
                    }
                }
            }
        }
    }
}
