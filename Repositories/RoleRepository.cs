using Dapper;
using Microsoft.Data.SqlClient;
using SeamsApp.Interfaces.Repositories;
using SeamsApp.Models;

namespace SeamsApp.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly string _connectionString;
        public RoleRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;

        }
        public async Task<IEnumerable<Role>> GetAllRolesAsync()
        {
            var sql = @"SELECT * FROM Roles";
            using (var connection = new SqlConnection(_connectionString))
            {
                var roles = await connection.QueryAsync<Role>(sql);
                return roles;
            }
        }

        public async Task<Role?> GetRoleByIdAsync(int roleId)
        {
            var sql = @"SELECT * FROM Roles WHERE RoleId = @RoleId";

            using (var connection = new SqlConnection(_connectionString))
            {
                var role = await connection.QueryFirstOrDefaultAsync<Role>(sql, new { RoleId = roleId });
                return role!;
            }
        }
    }
}
