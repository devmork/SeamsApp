using System.ComponentModel.DataAnnotations;

namespace SeamsApp.DTOs.Attendance
{
    public class AttendanceDTO
    {
        public string? Name { get; set; }
        public string? Note { get; set; }
        public DateTime Date { get; set; }
        public string? LogType { get; set; }
        public int Semester { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Status { get; set; }
    }
}
