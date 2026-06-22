using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using SeamsApp.Interfaces.Repositories.Queries;
using SeamsApp.Models.Base;

namespace SeamsApp.Repositories.Queries
{
    public class StudentRepository : IStudentRepository
    {
        // 1 - Pending, 2 - Approved, 3 - Rejected, 4 - Deleted

        private readonly string _connectionString;
        public StudentRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }
        public async Task<IEnumerable<Student>> GetAllPendingStudentAsync() 
        {
            string query = @"SELECT * FROM Students
                             WHERE Status = 1";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                return await connection.QueryAsync<Student>(query);
            }
        }
        public async Task<IEnumerable<Student>> GetAllApprovedStudentAsync()
        {
            string query = @"SELECT * FROM Students
                             WHERE Status = 2";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                return await connection.QueryAsync<Student>(query);
            }
        }
        public async Task<Student> CreateStudentAsync(Student student)
        {
            string query = @"INSERT INTO Students (
                                    FirstName, 
                                    MiddleName, 
                                    LastName, 
                                    Suffix,
                                    Email,
                                    SchoolStudentId, 
                                    YearLevel, 
                                    Course, 
                                    PhotoUrl,
                                    Status,
                                    SubmittedAt)
                             VALUES (
                                    @FirstName, 
                                    @MiddleName, 
                                    @LastName,
                                    @Suffix,   
                                    @Email,
                                    @SchoolStudentId, 
                                    @YearLevel, 
                                    @Course, 
                                    @PhotoUrl,
                                    @Status,
                                    @SubmittedAt)";

            var parameters = new DynamicParameters();
            parameters.Add("@FirstName", student.FirstName);
            parameters.Add("@MiddleName", student.MiddleName);
            parameters.Add("@LastName", student.LastName);
            parameters.Add("@Suffix", student.Suffix);
            parameters.Add("@Email", student.Email);

            parameters.Add("@SchoolStudentId", student.SchoolStudentId);
            parameters.Add("@YearLevel", student.YearLevel);
            parameters.Add("@Course", student.Course);
            parameters.Add("@PhotoUrl", student.PhotoUrl);

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                await connection.ExecuteAsync(query, parameters);
                return student;
            }
        }
        public async Task<int> UpdateStudentByIdAsync(Student student)
        {
            string query = @"UPDATE Students 
                             SET FirstName = @FirstName, 
                                 MiddleName = @MiddleName, 
                                 LastName = @LastName, 
                                 SchoolStudentId = @SchoolStudentId, 
                                 Course = @Course, 
                                 YearLevel = @YearLevel, 
                                 Email = @Email, 
                                 QRCode = @QRCode,
                                 UpdatedAt = @UpdatedAt 
                             WHERE StudentId = @StudentId";

            var parameters = new DynamicParameters();
            parameters.Add("@StudentId", student.StudentId);
            parameters.Add("@FirstName", student.FirstName);
            parameters.Add("@MiddleName", student.MiddleName);
            parameters.Add("@LastName", student.LastName);
            parameters.Add("@SchoolStudentId", student.SchoolStudentId);
            parameters.Add("@Course", student.Course);
            parameters.Add("@YearLevel", student.YearLevel);
            parameters.Add("@Email", student.Email);
            parameters.Add("@QRCode", student.QRCode);
            parameters.Add("@UpdatedAt", student.UpdatedAt);

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                return await connection.ExecuteScalarAsync<int>(query, parameters);
            }
        } 
        public async Task<Student> GetStudentByIdAsync(int studentId)
        {
            string query = @"SELECT * FROM Students 
                             WHERE StudentId = @StudentId
                             AND Status = 1";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var student = await connection.QueryFirstOrDefaultAsync<Student>(query, new { StudentId = studentId});
                return student!;
            }
        }
        public async Task<Student> GetStudentQRCodeAsync(string schoolStudentId)
        {
            string query = @"SELECT QRCode FROM Students
                             WHERE SchoolStudentId = @SchoolStudentId
                             AND Status = 1";
            
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var student = await connection.QueryFirstOrDefaultAsync<Student>(query, new { SchoolStudentId = schoolStudentId});
                return student!;
            }
        }
        public Task<int> DeleteStudentByIdAsync(int studentId)
        {
            string query = @"UPDATE Students 
                             SET Status = 0 
                             WHERE StudentId = @StudentId";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                return connection.ExecuteScalarAsync<int>(query, new { StudentId = studentId });
            }
        }
        public async Task UpdateStudentStatusToApprovedAsync(int studentId, int status, byte[]? qrCode)
        {
            string query = @"UPDATE Students 
                             SET Status = @Status, 
                                 QRCode = @QRCode,
                                 UpdatedAt = @UpdatedAt 
                             WHERE StudentId = @StudentId";

            var parameters = new DynamicParameters();
            parameters.Add("@StudentId", studentId);
            parameters.Add("@Status", status);
            parameters.Add("@QRCode", qrCode);
            parameters.Add("@UpdatedAt", DateTime.UtcNow);

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                await connection.ExecuteAsync(query, parameters);
            }
        }

        public async Task<int> UpdateStudentStatusToRejectAsync(int studentId, int status)
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
