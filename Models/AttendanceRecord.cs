using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeamsApp.Models
{
    public class AttendanceRecord
    {
        public int AttendanceRecordID { get; set; }
        public int AttendanceID { get; set; }
        public string? AttendanceName { get; set; }
        public string? LogType { get; set; }
        public string? SchoolStudentID { get; set; }
        public string? Name { get; set; }
        public string? Course { get; set; }
        public string? YearLevel { get; set; }
        public DateOnly Timestamp { get; set; }
        public int Status { get; set; }
    }
}
