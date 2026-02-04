using SeamsApp.DTOs.Attendance;
using SeamsApp.Models.Base;

namespace SeamsApp.Interfaces.Services
{
    public interface IAttendanceService
    {
        Task<int> CreateAttendance(CreateAttendanceDTO createAttendanceDTO);
        Task<List<AttendanceDTO>> GetAllAttendanceAsync(); // Fixed: no id, returns DTOs
        Task<AttendanceDTO?> GetAttendanceByIdAsync(int id); // Fixed: returns DTO
        Task<bool> RecordStudentAttendance(int attendanceId, int studentId);
        Task<bool> DeleteAttendance(int attendanceId); // Renamed for clarity (not student-specific)
        Task<bool> UpdateAttendance(int id, UpdateAttendanceDTO updateAttendanceDTO);
    }
}
