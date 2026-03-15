using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeamsApp.Models.Base
{
    public class Student
    {
        public int StudentId { get; set; }

        // PERSONAL INFORMATION
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? Suffix { get; set; }
        public string? Email { get; set; }

        // SCHOOL INFORMATION
        public string? SchoolStudentId { get; set; }
        public string? YearLevel { get; set; }
        public string? Course { get; set; }

        // PHOTO URL
        public string? PhotoUrl { get; set; }
        public byte[]? QRCode { get; set; }
        public int Status { get; set; } = 1; // 1 - Pending, 2 - Approved, 3 - Rejected, 4 - Deleted
        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    }
}
