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

namespace SeamsApp.Repositories.Queries
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
            parameters.Add("Name", attendance.Title);
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
            // const string query = @"SELECT * FROM Attendance
            //                        WHERE Status = 1";       
            const string query = @"
                 SELECT * FROM Attendance
        WHERE Status = 1 
          AND (
              Date > CONVERT(DATE, GETDATE())
              OR (
                  Date = CONVERT(DATE, GETDATE())
                  AND CONVERT(TIME, EndTime) > CONVERT(TIME, GETDATE())
              )
          )
        ORDER BY Date DESC, StartTime DESC
            ";           

            using (var connection = new SqlConnection(_connectionString))
            {
                var attendance = await connection.QueryAsync<Attendance>(query);
                return [.. attendance];
            }
        }

        public async Task<Attendance?> GetAttendanceById(int attendanceId)
        {
            const string query = @"SELECT * FROM Attendance
                                   WHERE AttendanceId = @AttendanceId AND Status = 1";
                    
            using (var connection = new SqlConnection(_connectionString))
            {
                return await connection.QuerySingleOrDefaultAsync<Attendance>(query, new { AttendanceId = attendanceId });
            }
        }

        public async Task<int> DeleteAttendance(int attendanceId)
        {
            const string query = @"UPDATE Attendance 
                                   SET Status = 0
                                   WHERE AttendanceId = @AttendanceId";
                
            using (var connection = new SqlConnection(_connectionString))
            {
                return await connection.ExecuteAsync(query, new { AttendanceId = attendanceId });
            }
        }

        public async Task<int> UpdateAttendance(Attendance attendance)
        {
            const string query = @"UPDATE Attendance
                                   SET 
                                    Name = @Name,
                                    Note = @Note,
                                    Date = @Date, 
                                    LogType = @LogType,
                                    Semester = @Semester,
                                    StartTime = @StartTime, 
                                    EndTime = @EndTime
                                WHERE AttendanceId = @AttendanceId";

            var parameters = new DynamicParameters();
            parameters.Add("AttendanceId", attendance.AttendanceId);
            parameters.Add("Name", attendance.Title);
            parameters.Add("Date", attendance.Date.ToString("yyyy-MM-dd"));
            parameters.Add("LogType", attendance.LogType);
            parameters.Add("Semester", attendance.Semester);
            parameters.Add("StartTime", attendance.StartTime.ToString("HH:mm"));
            parameters.Add("EndTime", attendance.EndTime.ToString("HH:mm"));
            parameters.Add("UpdatedAt", attendance.UpdatedAt);

            using (var connection = new SqlConnection(_connectionString))
            {
                return await connection.ExecuteAsync(query, parameters);
            }
        }
    }
}


