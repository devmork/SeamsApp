using System.ComponentModel.DataAnnotations;

namespace SeamsApp.Models
{
    public class StudentApplication
    {
        [Key]
        public int ApplicationId { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [StringLength(100, MinimumLength = 1)]
        public string FirstName { get; set; } = string.Empty;

        [StringLength(100)]
        public string? MiddleName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(100, MinimumLength = 1)]
        public string LastName { get; set; } = string.Empty;

        [StringLength(20)]
        public string? Suffix { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [StringLength(256)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "School Student ID is required.")]
        [StringLength(50, MinimumLength = 3)]
        public string SchoolStudentId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Year level is required.")]
        [StringLength(20)]
        public string YearLevel { get; set; } = string.Empty;

        [Required(ErrorMessage = "Course is required.")]
        [StringLength(100)]
        public string Course { get; set; } = string.Empty;

        [Url(ErrorMessage = "Photo URL must be a valid URL.")]
        [StringLength(500)]
        public string? PhotoUrl { get; set; }

        [Range(1, 3, ErrorMessage = "Status must be 1 (Pending), 2 (Approved), or 3 (Rejected).")]
        public int Status { get; set; }

        [StringLength(500)]
        public string? DeclineReason { get; set; }

        public int ReviewedBy { get; set; }

        public DateTime? ReviewedAt { get; set; }

        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;

    }
}
