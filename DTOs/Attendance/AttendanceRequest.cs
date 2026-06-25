using System.ComponentModel.DataAnnotations;

namespace SeamsApp.DTOs.Attendance
{
    public class AttendanceRequest
    {
        public string? Title { get; set; }
        public DateTime Date { get; set; }
        public string? Session { get; set; }
        public string? LogType { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
