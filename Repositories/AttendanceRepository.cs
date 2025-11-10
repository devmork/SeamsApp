using Dapper;
using Microsoft.Data.SqlClient;
using SeamsApp.Interfaces.Repositories;
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
        // STATUS 2 - INACTIVE

        private readonly string _connectionString;
        public AttendanceRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }
        public async Task<int> AddAttendance(Attendance attendance)
        {
            string query =
                     @"INSERT INTO Attendance
                     (AttendanceName, AttendanceLocation, LogType, Date, StartTime, EndTime, Status)
                     VALUES (@AttendanceName, @AttendanceLocation, @LogType, @Date, @StartTime, @EndTime, @Status)";

            var parameters = new DynamicParameters();
            parameters.Add("AttendanceName", attendance.AttendanceName);
            parameters.Add("AttendanceLocation", attendance.AttendanceLocation);
            parameters.Add("LogType", attendance.LogType);
            parameters.Add("Date", attendance.Date.ToString("yyyy-MM-dd"));
            parameters.Add("StartTime", attendance.StartTime.ToString("hh:mm tt"));
            parameters.Add("EndTime", attendance.EndTime.ToString("hh:mm tt"));
            parameters.Add("Status", 1);

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                return await connection.ExecuteScalarAsync<int>(query, parameters);
            }
        }
        public async Task<List<Attendance>> GetAllAttendance()
        {
            string query = @"SELECT * FROM Attendance
                           WHERE Status = 1";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var attendance = await connection.QueryAsync<Attendance>(query);
                return attendance.ToList();
            }
        }
        public async Task<int> DeleteAttendance(int attendanceID)
        {
            string query = @"UPDATE Attendance 
                           SET Status = 0
                           WHERE AttendanceID = @AttendanceID";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                return await connection.ExecuteScalarAsync<int>(query, new { AttendanceID = attendanceID});
            }
        }
        public async Task<int> UpdateAttendance(Attendance attendance)
        {
            string query =
                       @"UPDATE Attendance 
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
            parameters.Add("StartTime", attendance.StartTime.ToString("hh:mm tt"));
            parameters.Add("EndTime", attendance.EndTime.ToString("hh:mm tt"));

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                return await connection.ExecuteScalarAsync<int>(query, parameters);
            }
        }
    }
}

