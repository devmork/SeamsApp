using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using SeamsApp.Interfaces.Repositories;
using SeamsApp.Models.Base;

namespace SeamsApp.Data.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        // 1 - Active, 0 - Deleted
        private readonly string _connectionString;
        public StudentRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }
        public async Task<IEnumerable<Student>> GetAllStudentAsync()
        {
            string query = @"SELECT * FROM Students
                             WHERE Status = 1";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                return await connection.QueryAsync<Student>(query);
                            }
        }
        public async Task<int> AddStudentAsync(Student student)
        {
            string query = @"INSERT INTO Students (
                                    FirstName, 
                                    MiddleName, 
                                    LastName, 
                                    SchoolStudentId, 
                                    Course, 
                                    YearLevel, 
                                    Email, 
                                    QRCode,
                                    Status,
                                    AddedAt)
                             VALUES (
                                    @FirstName, 
                                    @MiddleName, 
                                    @LastName, 
                                    @SchoolStudentId, 
                                    @Course, 
                                    @YearLevel, 
                                    @Email, 
                                    @QRCode,
                                    @Status,
                                    @AddedAt)";

            var parameters = new DynamicParameters();
            parameters.Add("@FirstName", student.FirstName);
            parameters.Add("@MiddleName", student.MiddleName);
            parameters.Add("@LastName", student.LastName);
            parameters.Add("@SchoolStudentId", student.SchoolStudentId);
            parameters.Add("@Course", student.Course);
            parameters.Add("@YearLevel", student.YearLevel);
            parameters.Add("@Email", student.Email);
            parameters.Add("@QRCode", student.QRCode);
            parameters.Add("@Status", student.Status);
            parameters.Add("@AddedAt", student.AddedAt);

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                return await connection.ExecuteScalarAsync<int>(query, parameters);
            }
        }
        public async Task<int> UpdateStudentAsync(Student student)
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
        public async Task<Student> GetStudentByIdAsync(string schoolStudentID)
        {
            string query = @"SELECT * FROM Students 
                             WHERE SchoolStudentID = @SchoolStudentID";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var student = await connection.QueryFirstOrDefaultAsync<Student>(query, new { SchoolStudentID = schoolStudentID});
                return student!;
            }
        }
        public async Task<Student> GetStudentQRCodeAsync(string schoolStudentId)
        {
            string query = @"SELECT QRCode FROM Students WHERE SchoolStudentID = @SchoolStudentID";
            
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var student = await connection.QueryFirstOrDefaultAsync<Student>(query, new { SchoolStudentID = schoolStudentId});
                return student!;
            }
        }
    }
}
