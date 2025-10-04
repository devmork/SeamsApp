using AttendanceManagementSystem.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceManagementSystem.Interfaces.Repositories
{
    public interface IAttendanceRepository
    {
        void AddAttendance(Attendance attendance);
        List<Attendance> GetAllAttendance();
        void UpdateAttendance(Attendance attendance);
        void DeleteAttendance(int attendanceId);
    }
}
