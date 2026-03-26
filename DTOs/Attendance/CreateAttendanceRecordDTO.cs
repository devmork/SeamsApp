using System.ComponentModel.DataAnnotations;
namespace SeamsApp.DTOs.Attendance
{
    public class CreateAttendanceRecordDTO
    {
        public int AttendanceID { get; set; }
        [Required(ErrorMessage = "School Student ID is required")]
        [StringLength(9, ErrorMessage = "School Student ID must not exceed 9 characters")]
        public string SchoolStudentID { get; set; } = null!;
    }
}