using SeamsApp.DTOs.Attendance;

namespace SeamsApp.Interfaces.Services.Commands
{
    public interface IAttendanceService
    {
        Task<AttendanceRequest> CreateAttendanceAsync(int eventId, AttendanceRequest attendanceRequest);
        Task<IEnumerable<AttendanceResponse>> GetAllAttendanceAsync();
        Task<IEnumerable<AttendanceResponse>> GetAttendanceByEventId(int eventId);
        Task<AttendanceResponse> GetAttendanceByIdAsync(int attendanceId);
        Task<AttendanceResponse> DeleteAttendanceAsync(int attendanceId);
        Task<AttendanceResponse> UpdateAttendanceAsync(int attendanceId, AttendanceRequest attendanceRequest);

    }
}
