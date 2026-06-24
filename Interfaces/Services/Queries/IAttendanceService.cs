using SeamsApp.DTOs.Attendance;
using SeamsApp.Models.Base;

namespace SeamsApp.Interfaces.Services.Queries
{
    public interface IAttendanceService
    {
        Task<int> CreateAttendanceAsync(CreateAttendanceDTO createAttendanceDTO);
        Task<IEnumerable<AttendanceDTO>> GetAllAttendanceAsync(); // Fixed: no id, returns DTOs
        Task<AttendanceDTO?> GetAttendanceByIdAsync(int id); // Fixed: returns DTO
        Task<bool> DeleteAttendanceAsync(int attendanceId); // Renamed for clarity (not student-specific)
        Task<bool> UpdateAttendanceAsync(int id, UpdateAttendanceDTO updateAttendanceDTO);
    }
}
