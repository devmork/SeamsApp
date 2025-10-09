using SeamsApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeamsApp.Interfaces.Repositories
{
    public interface IAttendanceRecordRepository
    {
        void RecordStudentAttendance(AttendanceRecord attendanceRecord);
        bool CheckDuplicateAttendance(int attendanceId, string schoolStudentId);
        int GetTotalAbsent(string schoolStudentId);
        int GetTotalPresent(string schoolStudentId);
        int GetTotalAttendance();
        //List<AttendanceRecordsDTO> GetStudentAttendanceRecords(string schoolStudentId);
        void ResetAttendaceRecord(string schoolStudentId);
    }
}
