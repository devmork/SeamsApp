using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeamsApp.Models.Base
{
    public class Attendance
    {
        public int AttendanceId { get; set; }
        public string? AttendanceName { get; set; }
        public string? AttendanceLocation { get; set; }
        public string ?LogType { get; set; }
        public DateOnly Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public string AttendanceInfo
        {
            get
            {
                return $"{AttendanceId} - {AttendanceName} - {AttendanceLocation}";
            }
        }

    }
}
