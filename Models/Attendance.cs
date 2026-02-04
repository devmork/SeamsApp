using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeamsApp.Models.Base
{
    public class Attendance
    {
        public int AttendanceId { get; set; }
        [Required]
        public string? Name { get; set; }
        public string? Note { get; set; }
        public DateOnly Date { get; set; }
        public string? LogType { get; set; }  
        public int Semester { get; set; }
        [Required]
        public TimeOnly StartTime { get; set; }
        [Required]
        public TimeOnly EndTime { get; set; }
        public int Status { get; set; } = 1;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
