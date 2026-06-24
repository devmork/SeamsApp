using System.ComponentModel.DataAnnotations;

namespace SeamsApp.DTOs.StudentApplication
{
    public class CreateStudentApplicationRequest
    {
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? Suffix { get; set; }
        public string? Email { get; set; }
        public string? SchoolStudentId { get; set; }
        public string? YearLevel { get; set; }
        public string? Course { get; set; }
        public string? PhotoUrl { get; set; }
    }
}
