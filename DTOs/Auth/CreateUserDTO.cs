using System.ComponentModel.DataAnnotations;

namespace SeamsApp.DTOs.Auth
{
    public class CreateUserDTO
    {
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
    }
}
