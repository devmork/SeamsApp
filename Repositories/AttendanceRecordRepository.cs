using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SeamsApp.DTOs.AttendanceRecords;
using SeamsApp.Interfaces.Repositories;
using SeamsApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace SeamsApp.Repositories.Queries
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
            parameters.Add("@AttendanceID", attendanceRecord.AttendanceID);
            parameters.Add("@SchoolStudentID", attendanceRecord.SchoolStudentID);
            parameters.Add("@Timestamp", DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
            parameters.Add("@Status", 1);

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                return await connection.ExecuteScalarAsync<int>(query, parameters);
            }
        }
        public async Task<bool> CheckDuplicateAttendance(int attendanceID, string schoolStudentID)
        {
            string sql = @"
                SELECT COUNT(*) FROM AttendanceRecords 
                WHERE AttendanceID = @AttendanceID 
                AND SchoolStudentID = @SchoolStudentID 
                AND CAST(Timestamp AS DATE) = CAST(GETDATE() AS DATE)";  // Fixed for SQL Server

            var parameters = new DynamicParameters();
            parameters.Add("@AttendanceID", attendanceID);
            parameters.Add("@SchoolStudentID", schoolStudentID);

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                int count = await connection.ExecuteScalarAsync<int>(sql, parameters);
                return count > 0;
            }
        }

        public async Task<List<AttendanceRecordResponse>> GetListOfAttendanceRecordByAttendanceEventName(
            string attendanceEventName,
            string logType,
            int semester,
            int year
        )
        {
            var sql = @"
                SELECT
                    s.FirstName + ' ' + 
                    COALESCE(LEFT(s.MiddleName, 1) + '. ', '') + 
                    s.LastName + 
                    COALESCE(' ' + s.Suffix, '') AS FullName,
                    s.SchoolStudentId AS SchoolStudentId
                FROM 
                    Students s
                    LEFT JOIN AttendanceRecords ar ON s.SchoolStudentId = ar.SchoolStudentID
                    LEFT JOIN Attendance a ON ar.AttendanceID = a.AttendanceId  
                        AND a.Name = @Name 
                        AND a.LogType = @LogType
                        AND a.Semester = @Semester
                        AND YEAR(a.Date) = @Year
                WHERE 
                    ar.Status IS NULL OR ar.Status IN (1, 2)
                ORDER BY 
                    CASE WHEN ar.Status = 1 THEN 0 ELSE 1 END,
                    ar.Timestamp DESC";

            var parameters = new DynamicParameters();
            parameters.Add("@Name", attendanceEventName);
            parameters.Add("@LogType", logType);
            parameters.Add("@Semester", semester);
            parameters.Add("@Year", year);

            using(var connection = new SqlConnection(_connectionString))
            {
                var attendanceRecords = await connection.QueryAsync<AttendanceRecordResponse>(sql, parameters);
                return attendanceRecords.ToList();
            }
        }
    }
}
