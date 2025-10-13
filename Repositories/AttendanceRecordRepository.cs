using Dapper;
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
        public void RecordStudentAttendance(AttendanceRecord attendanceRecord)
        {
            string sql = @"
                            INSERT INTO AttendanceRecords 
                            (
                                AttendanceId,
                                AttendanceName,
                                LogType,
                                SchoolStudentId,
                                Name,
                                Course,
                                YearLevel,
                                Timestamp,
                                Status
                            )
                            VALUES 
                            (
                                @AttendanceId,
                                @AttendanceName,
                                @LogType,
                                @SchoolStudentId,
                                @Name,
                                @Course,
                                @YearLevel,
                                @Timestamp,
                                @Status
                            )";

            var parameters = new DynamicParameters();
            parameters.Add("AttendanceId", attendanceRecord.AttendanceID);
            parameters.Add("AttendanceName", attendanceRecord.AttendanceName);
            parameters.Add("LogType", attendanceRecord.LogType);
            parameters.Add("SchoolStudentId", attendanceRecord.SchoolStudentID);
            parameters.Add("Name", attendanceRecord.Name);  
            parameters.Add("Course", attendanceRecord.Course);
            parameters.Add("YearLevel", attendanceRecord.YearLevel);
            parameters.Add("Timestamp", DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
            parameters.Add("Status", 1);

            using ()
            {
                connection.Open();
                connection.Execute(sql, parameters);
            }
        }
        public bool CheckDuplicateAttendance(int attendanceId, string schoolStudentId)
        {
            string sql = @"SELECT COUNT(*) FROM AttendanceRecords 
                           WHERE AttendanceId = @AttendanceId 
                           AND SchoolStudentId = @SchoolStudentId 
                           AND DATE(Timestamp) = DATE('now')";

            var parameters = new DynamicParameters();
            parameters.Add("AttendanceId", attendanceId);
            parameters.Add("SchoolStudentId", schoolStudentId);

            using ()
            {
                connection.Open();
                return connection.ExecuteScalar<int>(sql, parameters) > 0;
            }
        }
        public int GetTotalAbsent(string schoolStudentId)
        {
            int allAttendanceId = GetTotalAttendance();
            string presentAttendanceIdSQL = @"SELECT COUNT(*) FROM AttendanceRecords 
                                              WHERE SchoolStudentId = @SchoolStudentId AND Status = 2";

            var parameters = new DynamicParameters();
            parameters.Add("SchoolStudentId", schoolStudentId);

            using ()
            {
                connection.Open();

                int presentAttendanceIdCount = connection.ExecuteScalar<int>(presentAttendanceIdSQL, parameters);
                return allAttendanceId - presentAttendanceIdCount;
            }
        }
        public int GetTotalAttendance()
        {
            string getAttendaceCountSQL = @"SELECT COUNT(AttendanceId) FROM Attendance";

            using ()
            {
                connection.Open();
                return connection.ExecuteScalar<int>(getAttendaceCountSQL);
            }
        }
        public int GetTotalPresent(string schoolStudentId)
        {
            string sql = @"
                            SELECT COUNT(AttendanceId)
                            FROM AttendanceRecords
                            WHERE SchoolStudentId = @SchoolStudentId
                            AND Status = 1";

            var parameters = new DynamicParameters();
            parameters.Add("SchoolStudentId", schoolStudentId);
            
            using ()
            {
                connection.Open();
                return connection.ExecuteScalar<int>(sql, parameters);
            }
        }
        public List<AttendanceRecordsDTO> GetStudentAttendanceRecords(string schoolStudentId)
        {
            string sql = @"SELECT AttendanceName, LogType, TimeStamp 
                           FROM AttendanceRecords 
                           WHERE SchoolStudentId = @SchoolStudentId 
                           AND Status = 1";

            var parameters = new DynamicParameters();
            parameters.Add("SchoolStudentId", schoolStudentId);

            using ()
            {
                connection.Open();
                return connection.Query<AttendanceRecordsDTO>(sql, parameters).ToList();
            }
        }
        public void ResetAttendaceRecord(string schoolStudentId)
        {
            string sql = @"UPDATE AttendanceRecords
                           SET Status = 2
                           WHERE SchoolStudentId = @SchoolStudentId";

            var paramaters = new DynamicParameters();
            paramaters.Add("SchoolStudentId", schoolStudentId);

            using ()
            {
                connection.Open();
                connection.Execute(sql, paramaters);
            }
        }
    }
}
