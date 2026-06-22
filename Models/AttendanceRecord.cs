using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeamsApp.Models
{
    public class AttendanceRecord
    {
        [Key]
        public int RecordID { get; set; }
        public int AttendanceID { get; set; }
        public string? SchoolStudentID { get; set; }
        public int Status { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
