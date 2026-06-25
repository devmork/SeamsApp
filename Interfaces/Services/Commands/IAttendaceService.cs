using SeamsApp.DTOs.Attendance;

namespace SeamsApp.Interfaces.Services.Commands
{
    public interface IAttendaceService
    {
        Task<AttendanceRequest> CreateAttendanceAsync(int eventId, AttendanceRequest attendanceRequest);
        Task<IEnumerable<AttendanceResponse>> GetAllAttendanceAsync();
        Task<AttendanceResponse> GetAttendanceByIdAsync(int attendanceId);
        Task<AttendanceResponse> DeleteAttendanceAsync(int attendanceId);
        Task<AttendanceResponse> UpdateAttendanceAsync(int attendanceId, AttendanceRequest attendanceRequest);

    }
}
