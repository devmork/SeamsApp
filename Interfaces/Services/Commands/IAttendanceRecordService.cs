using SeamsApp.DTOs.Attendance;
using SeamsApp.DTOs.AttendanceRecords;

namespace SeamsApp.Interfaces.Services.Commands
{
    public interface IAttendanceRecordService
    {
        Task<AttendanceRecordResponse> CreateAttendanceRecordAsync(AttendanceRecordRequest attendanceRecordRequest);
        Task<IEnumerable<AttendanceRecordResponse>> GetAllAttendanceRecordsAsync();
        Task<AttendanceRecordResponse> GetAttendanceRecordByIdAsync(int recordId);
        Task<IEnumerable<AttendanceRecordResponse>> GetAttendanceRecordsByAttendanceIdAsync(int attendanceId);
        Task<AttendanceRecordResponse> UpdateAttendanceRecordAsync(int recordId, AttendanceRecordRequest attendanceRecordRequest);
        Task<AttendanceRecordResponse> DeleteAttendanceRecordAsync(int recordId);
        Task<IEnumerable<EventAttendanceGroupResponse>> GetStudentAttendanceHistoryAsync(int userId);
    }
}