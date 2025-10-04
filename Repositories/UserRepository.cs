using AttendanceManagementSystem.Interfaces.Repositories;
using AttendanceManagementSystem.Models.Derived;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceManagementSystem.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        public void AddUser(User user)
        {
            using (var connection = new SQLiteConnection(SQLiteDataAccess.LoadConnectionString()))
            {
                connection.Open();
                string sql = @"INSERT INTO Users (UserName, Email, Password)
                             VALUES (@UserName, @Email, @Password);";
                
                var parameters = new DynamicParameters();
                parameters.Add("@UserName", user.UserName);
                parameters.Add("@Email", user.Email);
                parameters.Add("@Password", user.Password);
                connection.Execute(sql, parameters);
            }
        }
        public User GetUserByEmail(string email, string password)
        {
            using (var connection = new SQLiteConnection(SQLiteDataAccess.LoadConnectionString()))
            {
                connection.Open();
                string sql = "SELECT Email, Password FROM Users WHERE Email = @Email AND Password = @Password;";

                var parameters = new DynamicParameters();
                parameters.Add("Email", email);
                parameters.Add("Password", password);

                return connection.QueryFirstOrDefault<User>(sql, parameters);
            }
        }
    }
}
