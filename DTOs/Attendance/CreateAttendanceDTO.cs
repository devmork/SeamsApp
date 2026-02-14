using System.ComponentModel.DataAnnotations;

namespace SeamsApp.DTOs.Attendance
{
    public class CreateAttendanceDTO
    { [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string? Name { get; set; }

        [StringLength(500, ErrorMessage = "Note cannot exceed 500 characters.")]
        public string? Note { get; set; }

        [Required(ErrorMessage = "Date is required.")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "LogType is required.")]
        [StringLength(50, ErrorMessage = "LogType cannot exceed 50 characters.")]
        public string? LogType { get; set; }

        [Required(ErrorMessage = "Semester is required.")]
        [Range(1, 10, ErrorMessage = "Semester must be between 1 and 10.")]
        public int Semester { get; set; }

        [Required(ErrorMessage = "StartTime is required.")]
        public DateTime StartTime { get; set; }

        [Required(ErrorMessage = "EndTime is required.")]
        public DateTime EndTime { get; set; }

    }
}
