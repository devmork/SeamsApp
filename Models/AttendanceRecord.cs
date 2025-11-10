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
        public int StudentID { get; set; }
        public DateTime Timestamp { get; set; }
        public int Status { get; set; }

    }
}
