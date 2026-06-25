using SeamsApp.DTOs.Attendance;
using SeamsApp.DTOs.AttendanceRecords;

namespace SeamsApp.Interfaces.Services.Queries
{
    public interface IAttendanceRecordService
    {
        Task<int> CreateAttendanceRecordAsync(CreateAttendanceRecordDTO attendanceRecordDTO);
        Task<List<AttendanceRecordDTO>> GetListOfAttendanceRecordByAttendanceEventName(
            string attendanceEventName,
            string logType,
            int semester,
            int year
        );
    }
}