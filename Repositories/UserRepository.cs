using Dapper;
using Microsoft.Data.SqlClient;
using SeamsApp.Interfaces.Repositories;
using SeamsApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace SeamsApp.Data.Repositories
{
    public class UserRepository
    {
        private readonly string _connectionString;
        public UserRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }
        public async Task<int> AddUser(User user)
        {
            var query = @"
                INSERT INTO [dbo].[Users]
                (UserName, Email, PasswordHash, CreatedAt)
                VALUES
                (@UserName, @Email, @PasswordHash, @CreatedAt)
                SELECT SCOPE_IDENTITY();
            ";

            var parameters = new DynamicParameters();
            parameters.Add("@UserName", user.UserName);
            parameters.Add("@Email", user.Email);
            parameters.Add("@PasswordHash", user.PasswordHash);
            parameters.Add("@CreatedAt", DateTime.Now);

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var newUserId = await connection.ExecuteScalarAsync<int>(query, parameters, commandType: System.Data.CommandType.Text);
                return newUserId;
            }
        }

        public async Task<User> GetUserById(int userID)
        {
            var sql = @"
                SELECT * FROM [dbo].[Users]
                WHERE UserID = @UserID;
            ";

            var parameters = new DynamicParameters();
            parameters.Add("@UserID", userID);

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var user = await connection.QueryFirstOrDefaultAsync<User>(sql, parameters, commandType: System.Data.CommandType.Text);
                return user!;
            }
        }

        public async Task<List<string>> GetAllUsernames()
        {
            var query = @"SELECT UserName FROM [dbo].[Users];";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var userNames = await connection.QueryAsync<string>(query, commandType: System.Data.CommandType.Text);
                return userNames.ToList();
            }
        }

        public async Task<User> GetByUsername(string identifier)
        {
            var query = @"SELECT * FROM [dbo].[Users] 
                          WHERE UserName = @UserID";
            
            var parameters = new DynamicParameters();
            parameters.Add("@UserID", identifier);

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var userName = await connection.QueryFirstOrDefaultAsync<User>(query, parameters, commandType: System.Data.CommandType.Text);
                return userName!;
            }
        }
    }
}
