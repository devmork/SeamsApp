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
        [Key]
        public int AttendanceId { get; set; }
        public int EventId { get; set; }
        public string? Title { get; set; }
        public DateTime Date { get; set; }
        public string? LogType { get; set; }  
        [Required]
        public DateTime StartTime { get; set; }
        [Required]
        public DateTime EndTime { get; set; }
        public int Status { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
