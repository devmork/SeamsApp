using System.ComponentModel.DataAnnotations;

namespace SeamsApp.DTOs.Student
{
    public class StudentCreationDTO
    {
        [Required]
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        [Required]
        public string? LastName { get; set; }
        [Required]
        public string? SchoolStudentId { get; set; }
        [Required]
        public string? YearLevel { get; set; }
        [Required]
        public string? Course { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        public byte[]? QRCode { get; set; }
    }
}
