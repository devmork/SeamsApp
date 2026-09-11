using System.ComponentModel.DataAnnotations;

namespace SeamsApp.Models
{
    public class StudentApplication
    {
        [Key]
        public int ApplicationId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string? MiddleName { get; set; }
        public string LastName { get; set; } = string.Empty;
        public string? Suffix { get; set; }
        public string Email { get; set; } = string.Empty;
        public string SchoolStudentId { get; set; } = string.Empty;
        public string YearLevel { get; set; } = string.Empty;
        public string Course { get; set; } = string.Empty;
        public string? PhotoUrl { get; set; }
        [Range(1, 3)]
        public int Status { get; set; }
        public int ReviewedBy { get; set; }
        public DateTime? ReviewedAt { get; set; }
        public DateTime SubmittedAt { get; set; }

    }
}
