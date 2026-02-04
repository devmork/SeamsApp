using SeamsApp.Models;
using SeamsApp.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeamsApp.Interfaces.Repositories
{
    public interface IAttendanceRepository
    {
        Task<int> AddAttendance(Attendance attendance);
        Task<List<Attendance>> GetAllAttendance();
        Task<Attendance?> GetAttendanceById(int id); // Added for efficiency
        Task<int> DeleteAttendance(int attendanceID);
        Task<int> UpdateAttendance(Attendance attendance);
        Task<bool> CheckDuplicateRecord(int attendanceId, int studentId);
        Task<int> RecordStudentAttendance(AttendanceRecord record);
    }
}
