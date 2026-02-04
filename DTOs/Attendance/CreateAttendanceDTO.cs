namespace SeamsApp.DTOs.Attendance
{
    public class CreateAttendanceDTO
    {
        public string AttendanceName { get; set; } = string.Empty;
        public string AttendanceLocation { get; set; } = string.Empty;
        public string LogType { get; set; } = string.Empty;
        public DateOnly Date { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }

    }
}
