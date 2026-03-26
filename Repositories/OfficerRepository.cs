using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SeamsApp.DTOs.Officer;
using SeamsApp.Interfaces.Repositories;
using SeamsApp.Models.Base;

namespace SeamsApp.Repositories
{
    public class OfficerRepository : IOfficerRepository
    {
        // 1 - Pending, 2 - Approved, 3 - Rejected, 4 - Deleted

        private readonly string _connectionString;

        public OfficerRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }
        public async Task<int> RemoveOfficerAsync(int studentId, int status)
        {
            string query = @"UPDATE Students 
                             SET Status = @Status, 
                                 UpdatedAt = @UpdatedAt 
                             WHERE StudentId = @StudentId";

            var parameters = new DynamicParameters();
            parameters.Add("@StudentId", studentId);
            parameters.Add("@Status", status);
            parameters.Add("@UpdatedAt", DateTime.UtcNow);

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                return await connection.ExecuteAsync(query, parameters);
            }
        }

        public async Task<IEnumerable<OfficerDTO>> GetAllOfficersAsync()
        {
            string query = @"SELECT 
                                   FirstName
                                  ,MiddleName
                                  ,LastName
                                  ,Suffix
                                  ,Email
                                  ,SchoolStudentId
                                  ,YearLevel
                                  ,Course
                                  ,PhotoUrl
                                  ,Status
                            FROM Students s
                            LEFT JOIN Users u
                            ON s.StudentId = u.StudentId
                            WHERE UserRole = 'Officer'";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                return await connection.QueryAsync<OfficerDTO>(query);
            }
        }

        public async Task<int> UpdateStudentStatusToOfficer(int studentId, int status)
        {
            string query = @"UPDATE Students 
                             SET Status = @Status, 
                                 UpdatedAt = @UpdatedAt 
                             WHERE StudentId = @StudentId";

            var parameters = new DynamicParameters();
            parameters.Add("@StudentId", studentId);
            parameters.Add("@Status", status);
            parameters.Add("@UpdatedAt", DateTime.UtcNow);

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                return await connection.ExecuteAsync(query, parameters);
            }

        }
    }
}
