using ISEMES.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Text;

namespace ISEMES.Repositories
{
    public class MasterDataRepository : IMasterDataRepository
    {
        private readonly AppDbContext _context;
        public MasterDataRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Address>> GetAddressData()
        {            
            return await _context.ListAddress.FromSqlRaw("EXEC usp_GetAddress").ToListAsync();
        }

        public DataTable GetEntityDetailByType(string entityType)
        {
            var dataTable = new DataTable();

            using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
            {
                connection.Open();

                using (var command = new SqlCommand("usp_GetEntityDetail", connection))
                {
                    command.Parameters.AddWithValue("@EntityType", entityType);
                    command.CommandType = CommandType.StoredProcedure;

                    using (var reader = command.ExecuteReader())
                    {
                        dataTable.Load(reader);
                    }
                }
            }

            return dataTable;
        }      

        public async Task<List<string>> GetMasterData()
        {
            var data = new List<string>();
            var stringBuilder = new StringBuilder();
            using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
            {
                connection.Open();
                var command = new SqlCommand("usp_GetMasterData", connection);

                command.CommandType = CommandType.StoredProcedure;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        stringBuilder.Append(reader.GetString(0));
                    }
                }
            }

            data.Add(stringBuilder.ToString());
            return await Task.FromResult(data);
        }

        public async Task<List<HardwareTypeDetails>> GetHardwareTypeData()
        {
            return await _context.ListHardwareTypeDetails.FromSqlRaw("EXEC usp_GetHardwareType").ToListAsync();
        }

        public async Task<List<EmployeeMaster>> GetInvUserByRoleAsync(string filterKey, int isActive, string condition)
        {
            var empList = new List<EmployeeMaster>();
            EmployeeMaster emp = null;
            using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
            {
                connection.Open();
                var command = new SqlCommand("usp_InvGetuserbyAccount", connection);
                command.Parameters.AddWithValue("@FilterKey", filterKey);
                command.Parameters.AddWithValue("@Active", isActive);
                command.Parameters.AddWithValue("@Condition", condition);
                command.CommandType = CommandType.StoredProcedure;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        emp = new EmployeeMaster();
                        emp.employeeID = reader.GetInt32(0);
                        emp.employeeName = reader.GetString(1);
                        empList.Add(emp);
                    }
                }
            }
            return await Task.FromResult(empList);
        }
    }
}

