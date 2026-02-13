using System.ComponentModel.DataAnnotations;

namespace SeamsApp.DTOs.Attendance
{
    public class AttendanceDTO
    {
         [Required]
[StringLength(200, MinimumLength = 1)]
public string? Name { get; set; }

[StringLength(1000)]
public string? Note { get; set; }

[Required]
public DateTime Date { get; set; }

[StringLength(50)]
public string? LogType { get; set; }

[Required]
[Range(1, 20)]
public int Semester { get; set; }

[Required]
public DateTime StartTime { get; set; }

[Required]
public DateTime EndTime { get; set; }

[Required]
[Range(0, 3)]
public int Status { get; set; }
    }
}
