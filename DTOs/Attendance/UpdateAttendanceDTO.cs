using System.ComponentModel.DataAnnotations;

namespace SeamsApp.DTOs.Attendance
{
    public class UpdateAttendanceDTO
    {
        public string? Name { get; set; }
        public string? Note { get; set; }
        public DateOnly Date { get; set; }
        public string? LogType { get; set; }
        public int Semester { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
    }
}
