namespace SeamsApp.DTOs.AttendanceRecords
{
    public class AttendanceSessionMarkResponse
    {
        public int RecordId { get; set; }
        public int AttendanceId { get; set; }
        public string? Session { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Status { get; set; } // 0 = Absent, 1 = Present
    }

    public class EventAttendanceGroupResponse
    {
        public int EventId { get; set; }
        public string? EventTitle { get; set; }
        public DateTime Date { get; set; }
        public List<AttendanceSessionMarkResponse> Sessions { get; set; } = new();
    }
}