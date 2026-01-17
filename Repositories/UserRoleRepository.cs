using Dapper;
using Microsoft.Data.SqlClient;
using SeamsApp.Interfaces.Repositories;
using SeamsApp.Models;

namespace SeamsApp.Repositories
{
    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly string _connectionString;
        public UserRoleRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;

        }
        public async Task<int> AssignRoleAsync(UserRole userRole)
        {
            var sql = @"INSERT INTO UserRoles (UserId, RoleId, AssignedAt) 
                        VALUES (@UserId, @RoleId, @AssignedAt)
                        SELECT CAST(SCOPE_IDENTITY() AS INT)";

            var parameters = new DynamicParameters();
            parameters.Add("UserId", userRole.UserId);
            parameters.Add("RoleId", userRole.RoleId);
            parameters.Add("AssignedAt", userRole.AssignedAt);

            using (var connection = new SqlConnection(_connectionString))
            {
                return await connection.ExecuteScalarAsync<int>(sql, parameters);
            }
        }

        public async Task<List<string>> GetUserRolesAsync(int userId)
        {
            var sql = @"
                        SELECT r.Name
                        FROM  Roles r
                        INNER JOIN UserRoles ur
                            ON r.RoleId = ur.RoleId 
                        WHERE ur.UserId = @UserId;";

            using (var connection = new SqlConnection(_connectionString))
            {
                var roles = await connection.QueryAsync<string>(sql, new { UserId = userId });
                return roles.ToList();
            }
        }

        public async Task<IEnumerable<UserRole>> GetUserRolesByUserIdAsync(int userId)
        {
            var sql = "SELECT * FROM UserRoles WHERE UserId = @UserId";

            using (var connection = new SqlConnection(_connectionString))
            {
                var userRoles = await connection.QueryAsync<UserRole>(sql, new { UserId = userId });
                return userRoles!;
            }
        }
    }
}
