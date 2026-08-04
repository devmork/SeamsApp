using System.ComponentModel.DataAnnotations;

namespace SeamsApp.DTOs.AttendanceRecords
{
    public class AttendanceRecordRequest
    {
        public int AttendanceID { get; set; }
        public string SchoolStudentID { get; set; } = null!;
        public int Status { get; set; } = 1;
    }
}
