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
        public int StudentID { get; set; }
        [Required]
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        [Required]
        public string? LastName { get; set; }
        [Required]
        public string? SchoolStudentID { get; set; }
        [Required]
        public string? YearLevel { get; set; }
        [Required]
        public string? Course { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        public byte[]? QRCode { get; set; }
    }
}
