using System.ComponentModel.DataAnnotations;

namespace SeamsApp.DTOs.StudentApplication
{
    public class CreateStudentApplicationRequest
    {
        [Required(ErrorMessage = "First name is required.")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "First name must be between 1 and 100 characters.")]
        public string FirstName { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "Middle name cannot exceed 100 characters.")]
        public string? MiddleName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Last name must be between 1 and 100 characters.")]
        public string LastName { get; set; } = string.Empty;

        [StringLength(20, ErrorMessage = "Suffix cannot exceed 20 characters.")]
        public string? Suffix { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [StringLength(256, ErrorMessage = "Email cannot exceed 256 characters.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "School Student ID is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "School Student ID must be between 3 and 50 characters.")]
        public string SchoolStudentId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Year level is required.")]
        [StringLength(20, ErrorMessage = "Year level cannot exceed 20 characters.")]
        public string YearLevel { get; set; } = string.Empty;

        [Required(ErrorMessage = "Course is required.")]
        [StringLength(100, ErrorMessage = "Course cannot exceed 100 characters.")]
        public string Course { get; set; } = string.Empty;

        [Url(ErrorMessage = "Photo URL must be a valid URL.")]
        [StringLength(500, ErrorMessage = "Photo URL cannot exceed 500 characters.")]
        public string? PhotoUrl { get; set; }
    }
}