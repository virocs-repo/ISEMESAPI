using ISEMES.Models;
using ISEMES.Repositories;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace ISEMES.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public UserService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<AppMenuResponse> GetRoles(string? email, string? userName, string? password)
        {
            return await _userRepository.GetRoles(email, userName, password);
        }

        /// <summary>
        /// Gets roles for a user by email using proc_inv_ListAppSecurityByEmail stored procedure
        /// </summary>
        public List<string> GetRolesByEmail(string email)
        {
            var roles = new List<string>();

            using (var connection = new SqlConnection(_configuration.GetConnectionString("InventoryConnection")))
            {
                connection.Open();
                var command = new SqlCommand("proc_inv_ListAppSecurityByEmail", connection);
                command.Parameters.AddWithValue("@Email", email);
                command.CommandType = CommandType.StoredProcedure;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        roles.Add(reader.GetString(0));
                    }
                }
            }

            return roles;
        }

        /// <summary>
        /// Gets roles for a user by username using DefaultConnection
        /// </summary>
        public List<string> GetRolesForUser(string username)
        {
            var roles = new List<string>();

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var command = new SqlCommand("SELECT Role FROM UserRoles WHERE Username = @Username", connection);
                command.Parameters.AddWithValue("@Username", username);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        roles.Add(reader.GetString(0));
                    }
                }
            }

            return roles;
        }

        /// <summary>
        /// Validates external user credentials using DefaultConnection
        /// Note: Password should be hashed and salted in production
        /// </summary>
        public bool ValidateExternalUser(string username, string password)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var command = new SqlCommand("SELECT COUNT(1) FROM Users WHERE Username = @Username AND Password = @Password", connection);
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Password", password); // Note: Password should be hashed and salted in production

                var result = (int)command.ExecuteScalar();
                return result > 0;
            }
        }
    }
}

