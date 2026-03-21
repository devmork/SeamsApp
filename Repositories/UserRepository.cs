using Dapper;
using Microsoft.Data.SqlClient;
using SeamsApp.Interfaces.Repositories;
using SeamsApp.Models;
using SeamsApp.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace SeamsApp.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;
        public UserRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;

        }
        public async Task<User> CreateUserAsync(User user)
        {
            var sql = @"INSERT INTO Users (
                            Email, 
                            PasswordHash,
                            UserRole,
                            CreatedAt )
                        VALUES (
                            @Email, 
                            @PasswordHash,
                            @UserRole,
                            @CreatedAt);
                        SELECT CAST(SCOPE_IDENTITY() as int)";

            var parameters = new DynamicParameters();
            parameters.Add("@Email", user.Email);
            parameters.Add("@UserRole", user.UserRole);
            parameters.Add("@PasswordHash", user.PasswordHash);
            parameters.Add("@CreatedAt", user.CreatedAt);

            using (var connection = new SqlConnection(_connectionString))
            {
                var userId = await connection.ExecuteScalarAsync<int>(sql, parameters);
                user.UserId = userId;
                return user;
            }
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            var sql = @"SELECT * FROM Users";

            using (var connection = new SqlConnection(_connectionString))
            {
                var users = await connection.QueryAsync<User>(sql);
                return users;
            }
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            var sql = @"SELECT * FROM Users WHERE Email = @Email";

            using (var connection = new SqlConnection(_connectionString))
            {
                var user = await connection.QueryFirstOrDefaultAsync<User>(sql, new { Email = email });
                return user!;
            }
        }
        public async Task<User> GetUserByIdAsync(int userId)
        {
            var sql = @"SELECT * FROM Users WHERE UserId = @UserId";
            using (var connection = new SqlConnection(_connectionString))
            {
                var user = await connection.QueryFirstOrDefaultAsync<User>(sql, new { UserId = userId });
                return user!;
            }
        }
    }
}
