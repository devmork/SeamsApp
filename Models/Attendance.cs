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
        public int AttendanceID { get; set; }
        [Required]
        public string? AttendanceName { get; set; }
        [Required]
        public string? AttendanceLocation { get; set; }
        [Required]
        public string? LogType { get; set; }
        [Required]
        public DateOnly Date { get; set; }
        [Required]
        public TimeOnly StartTime { get; set; }
        [Required]
        public TimeOnly EndTime { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Status { get; set; } // Added: Matches DB for soft deletes
    }
}
