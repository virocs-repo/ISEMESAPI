using ISEMES.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace ISEMES.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<AppMenuResponse> GetRoles(string? email, string? userName, string? password)
        {
            var response = new AppMenuResponse();

            using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("Inv_GetAppSecurity_By_LoginId", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    if (email != null) { command.Parameters.AddWithValue("@Email", email); }
                    if (userName != null) { command.Parameters.AddWithValue("@UserName", userName); }
                    if (password != null) { command.Parameters.AddWithValue("@Password", password); }

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        // Read Result Set 1 - AppMenus
                        var appMenus = new List<AppMenu>();
                        while (await reader.ReadAsync())
                        {
                            appMenus.Add(new AppMenu
                            {
                                AppMenuID = reader.GetInt32(0),
                                MenuTitle = reader.GetString(1),
                                NavigationUrl = reader.GetString(2),
                                Description = reader.GetString(3),
                                AppFeatureID = reader.GetInt32(4),
                                ParentID = reader.IsDBNull(5) ? null : reader.GetInt32(5),
                                AppMenuIndex = reader.GetInt32(6),
                                SequenceNumber = reader.GetInt32(7),
                                Active = reader.GetBoolean(8)
                            });
                        }
                        response.AppMenus = appMenus;

                        // Move to Result Set 2 - AppFeatures
                        await reader.NextResultAsync();
                        var appFeatures = new List<AppFeature>();
                        while (await reader.ReadAsync())
                        {
                            appFeatures.Add(new AppFeature
                            {
                                AppMenuId = reader.GetInt32(0),
                                AppFeatureId = reader.GetInt32(1),
                                FeatureID = reader.GetInt32(2),
                                FeatureName = reader.GetString(3),
                                Active = reader.GetBoolean(4)
                            });
                        }
                        response.AppFeatures = appFeatures;

                        // Move to Result Set 3 - AppFeatureFields
                        await reader.NextResultAsync();
                        var appFeatureFields = new List<AppFeatureField>();
                        while (await reader.ReadAsync())
                        {
                            appFeatureFields.Add(new AppFeatureField
                            {
                                AppFeatureID = reader.GetInt32(0),
                                FeatureFieldId = reader.GetInt32(1),
                                FeatureFieldName = reader.GetString(2),
                                Active = reader.GetBoolean(3),
                                IsReadOnly = reader.GetBoolean(4),
                                IsWriteOnly = reader.GetBoolean(5)
                            });
                        }
                        response.AppFeatureFields = appFeatureFields;

                        // Move to Result Set 4 - MainMenuItem
                        await reader.NextResultAsync();
                        var mainMenuItem = new List<MainMenu>();
                        while (await reader.ReadAsync())
                        {
                            mainMenuItem.Add(new MainMenu
                            {
                                LoginId = reader.GetInt32(0),
                                UserName = reader.GetString(1),
                                Facility = reader.GetString(2),
                                RoleName = reader.GetString(3)
                            });
                        }
                        response.MainMenuItem = mainMenuItem;
                    }
                }
            }

            return response;
        }
    }
}

