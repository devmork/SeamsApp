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
        public string SchoolStudentID { get; set; } = null!;
        public DateTime Timestamp { get; set; } // Used for when attendance was logged
        public int Status { get; set; } // Added usage in service
        // Removed RecordedAt: Not in DB; use Timestamp instead
    }
}
