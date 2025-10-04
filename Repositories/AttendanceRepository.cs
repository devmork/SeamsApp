using AttendanceManagementSystem.Interfaces.Repositories;
using AttendanceManagementSystem.Models.Base;
using Dapper;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Globalization;
using System.IO;
using System.Linq;
using ZXing.QrCode.Internal;

namespace AttendanceManagementSystem.Data.Repositories
{
    public class AttendanceRepository : IAttendanceRepository
    {
        // STATUS 0 - DELETED
        // STATUS 1 - ACTIVE
        // STATUS 2 - INACTIVE
        public void AddAttendance(Attendance attendance)
        {
            string sql =
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
            parameters.Add("Status", 1); // Active status

            using (SQLiteConnection connection = new SQLiteConnection(SQLiteDataAccess.LoadConnectionString()))
            {
                connection.Open();
                connection.Execute(sql, parameters);
            }
        }
        public List<Attendance> GetAllAttendance()
        {
            string sql = @"SELECT * FROM Attendance
                           WHERE Status = 1";

            using (SQLiteConnection connection = new SQLiteConnection(SQLiteDataAccess.LoadConnectionString()))
            {
                connection.Open();
                var attendance = connection.Query<Attendance>(sql).ToList();
                return attendance;
            }
        }
        public void DeleteAttendance(int attendanceId)
        {
            string sql = @"UPDATE Attendance 
                           SET Status = 0
                           WHERE AttendanceId = @AttendanceId ";
            var parameters = new DynamicParameters();
            parameters.Add("AttendanceId", attendanceId);

            using (SQLiteConnection connection = new SQLiteConnection(SQLiteDataAccess.LoadConnectionString()))
            {
                connection.Open();
                connection.Execute(sql, parameters);
            }
        }
        public void UpdateAttendance(Attendance attendance)
        {
            string sql =
                       @"UPDATE Attendance 
                       SET AttendanceName = @AttendanceName, 
                           AttendanceLocation = @AttendanceLocation, 
                           LogType = @LogType, 
                           Date = @Date, 
                           StartTime = @StartTime, 
                           EndTime = @EndTime
                       WHERE AttendanceId = @AttendanceId";

            var parameters = new DynamicParameters();
            parameters.Add("AttendanceId", attendance.AttendanceId);
            parameters.Add("AttendanceName", attendance.AttendanceName);
            parameters.Add("AttendanceLocation", attendance.AttendanceLocation);
            parameters.Add("LogType", attendance.LogType);
            parameters.Add("Date", attendance.Date.ToString("yyyy-MM-dd"));
            parameters.Add("StartTime", attendance.StartTime.ToString("hh:mm tt"));
            parameters.Add("EndTime", attendance.EndTime.ToString("hh:mm tt"));

            using (SQLiteConnection connection = new SQLiteConnection(SQLiteDataAccess.LoadConnectionString()))
            {
                connection.Open();
                connection.Execute(sql, parameters);
            }
        }
    }
}

