namespace SeamsApp.DTOs.AttendanceRecords
{
    public class AttendanceRecordResponse
    {
        public int RecordID { get; set; }
        public int AttendanceID { get; set; }
        public string SchoolStudentID { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string? YearLevel { get; set; }
        public string? Course { get; set; }
        public int Status { get; set; }
        public DateTime Timestamp { get; set; }
    }
}