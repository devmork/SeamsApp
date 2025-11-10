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
        Task<int> RecordStudentAttendance(AttendanceRecord attendanceRecord);
        Task<bool> CheckDuplicateAttendance(int attendanceID, string schoolStudentID);
    }
}
