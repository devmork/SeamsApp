using SeamsApp.Models;
using SeamsApp.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeamsApp.Interfaces.Repositories.Queries
{
    public interface IAttendanceRepository
    {
        Task<int> AddAttendance(Attendance attendance);
        Task<IEnumerable<Attendance>> GetAllAttendance();
        Task<Attendance?> GetAttendanceById(int attendanceId);
        Task<int> DeleteAttendance(int attendanceId);
        Task<int> UpdateAttendance(Attendance attendance);
    }
}
