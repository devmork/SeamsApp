using System.ComponentModel.DataAnnotations;

namespace SeamsApp.DTOs.Attendance
{
    public class AttendanceResponse
    {
        public int AttendanceId { get; set; }
        public int EventId { get; set; }
        public string? Title { get; set; }
        public DateTime Date { get; set; }
        public string? Session { get; set; }
        public string? LogType { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
