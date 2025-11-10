using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SeamsApp.Interfaces.Repositories;
using SeamsApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeamsApp.Repositories
{
    public class AttendanceRecordRepository : IAttendanceRecordRepository
    {
        // STATUS 0 - DELETED
        // STATUS 1 - PRESENT
        // STATUS 2 - ABSENT
        // STATUS 3 - RESETTED

        private readonly string _connectionString;
        public AttendanceRecordRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }
        public async Task<int> RecordStudentAttendance(AttendanceRecord attendanceRecord)
        {
            string query = @"
                            INSERT INTO AttendanceRecords 
                            (
                                AttendanceID,
                                SchoolStudentID,
                                Timestamp,
                                Status
                            )
                            VALUES 
                            (
                                @AttendanceID,
                                @SchoolStudentID,
                                @Timestamp,
                                @Status
                            )";

            var parameters = new DynamicParameters();
            parameters.Add("AttendanceID", attendanceRecord.AttendanceID);
            parameters.Add("StudentID", attendanceRecord.StudentID);
            parameters.Add("Timestamp", DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
            parameters.Add("Status", 1);

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                return await connection.ExecuteScalarAsync<int>(query, parameters);
            }
        }
        public async Task<bool> CheckDuplicateAttendance(int attendanceID, string schoolStudentID)
        {
            string sql = @"SELECT COUNT(*) FROM AttendanceRecords 
                           WHERE AttendanceID = @AttendanceID 
                           AND SchoolStudentID = @SchoolStudentID 
                           AND DATE(Timestamp) = DATE('now')";

            var parameters = new DynamicParameters();
            parameters.Add("AttendanceID", attendanceID);
            parameters.Add("SchoolStudentID", schoolStudentID);

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                return await connection.ExecuteScalarAsync<int>(sql, parameters) > 0;
            }
        }
    }
}
