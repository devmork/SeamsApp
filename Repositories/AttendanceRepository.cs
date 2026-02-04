using Dapper;
using Microsoft.Data.SqlClient;
using SeamsApp.Interfaces.Repositories;
using SeamsApp.Models;
using SeamsApp.Models.Base;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace SeamsApp.Data.Repositories
{
    public class AttendanceRepository : IAttendanceRepository
    {
        // STATUS 0 - DELETED
        // STATUS 1 - ACTIVE

        private readonly string _connectionString;

        public AttendanceRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        public async Task<int> AddAttendance(Attendance attendance)
        {
            const string query = @" INSERT INTO Attendance (
                                        Name,
                                        Note,
                                        Date, 
                                        LogType, 
                                        Semester,                                        
                                        StartTime, 
                                        EndTime,
                                        Status,
                                        CreatedAt)
                                    VALUES (
                                        @Name, 
                                        @Note, 
                                        @Date, 
                                        @LogType,
                                        @Semester,  
                                        @StartTime, 
                                        @EndTime, 
                                        @Status,
                                        @CreatedAt)";

            var parameters = new DynamicParameters();
            parameters.Add("Name", attendance.Name);
            parameters.Add("Note", attendance.Note);
            parameters.Add("Date", attendance.Date.ToString("yyyy-MM-dd"));
parameters.Add("LogType", attendance.LogType);
            parameters.Add("Semester", attendance.Semester);
            parameters.Add("StartTime", attendance.StartTime.ToString("HH:mm"));
            parameters.Add("EndTime", attendance.EndTime.ToString("HH:mm"));
            parameters.Add("Status", attendance.Status);
            parameters.Add("CreatedAt", attendance.CreatedAt);

            using (var connection = new SqlConnection(_connectionString))
            {
                return await connection.ExecuteScalarAsync<int>(query, parameters);
            }
        }

        public async Task<IEnumerable<Attendance>> GetAllAttendance()
        {
            const string query = @"                SELECT * FROM Attendance
                WHERE Status = 1";

            using (var connection = new SqlConnection(_connectionString))
            {
                var attendance = await connection.QueryAsync<Attendance>(query);
                return [.. attendance];
            }
        }

        public async Task<Attendance?> GetAttendanceById(int id)
        {
            const string query = @"
                SELECT * FROM Attendance
                WHERE AttendanceID = @AttendanceID AND Status = 1";

            using (var connection = new SqlConnection(_connectionString))
            {
                return await connection.QuerySingleOrDefaultAsync<Attendance>(query, new { AttendanceID = id });
            }
        }

        public async Task<int> DeleteAttendance(int attendanceID)
        {
            const string query = @"
                UPDATE Attendance 
                SET Status = 0
                WHERE AttendanceID = @AttendanceID";

            using (var connection = new SqlConnection(_connectionString))
            {
                return await connection.ExecuteAsync(query, new { AttendanceID = attendanceID });
            }
        }

        public async Task<int> UpdateAttendance(Attendance attendance)
        {
            const string query = @"
                UPDATE Attendance 
                SET AttendanceName = @AttendanceName, 
                    AttendanceLocation = @AttendanceLocation, 
                    LogType = @LogType, 
                    Date = @Date, 
                    StartTime = @StartTime, 
                    EndTime = @EndTime
                WHERE AttendanceID = @AttendanceID";

            var parameters = new DynamicParameters();
            parameters.Add("AttendanceID", attendance.AttendanceID);
            parameters.Add("AttendanceName", attendance.AttendanceName);
            parameters.Add("AttendanceLocation", attendance.AttendanceLocation);
            parameters.Add("LogType", attendance.LogType);
            parameters.Add("Date", attendance.Date.ToString("yyyy-MM-dd"));
            parameters.Add("StartTime", attendance.StartTime.ToString("HH:mm")); // 24-hour
            parameters.Add("EndTime", attendance.EndTime.ToString("HH:mm")); // 24-hour

            using (var connection = new SqlConnection(_connectionString))
            {
                return await connection.ExecuteAsync(query, parameters);
            }
        }

        public async Task<bool> CheckDuplicateRecord(int attendanceId, int studentId)
        {
            const string sql = @"
                SELECT COUNT(*)
                FROM AttendanceRecord
                WHERE AttendanceID = @AttendanceID AND StudentID = @StudentID AND Status = 1"; // Added Status check

            using (var connection = new SqlConnection(_connectionString))
            {
                int count = await connection.ExecuteScalarAsync<int>(sql, new { AttendanceID = attendanceId, StudentID = studentId });
                return count > 0;
            }
        }

        public async Task<int> RecordStudentAttendance(AttendanceRecord record)
        {
            const string sql = @"
                INSERT INTO AttendanceRecord
                (AttendanceID, StudentID, Timestamp, Status)
                OUTPUT INSERTED.AttendanceRecordID
                VALUES (@AttendanceID, @StudentID, @Timestamp, @Status)";

            var parameters = new DynamicParameters();
            parameters.Add("AttendanceID", record.AttendanceID);
            parameters.Add("StudentID", record.StudentID);
            parameters.Add("Timestamp", record.Timestamp);
            parameters.Add("Status", record.Status);

            using (var connection = new SqlConnection(_connectionString))
            {
                return await connection.ExecuteScalarAsync<int>(sql, parameters);
            }
        }
    }
}


