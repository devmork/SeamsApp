using System.ComponentModel.DataAnnotations;

namespace SeamsApp.DTOs.Student
{
    public class StudentDTO
    {
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? SchoolStudentId { get; set; }
        public string? YearLevel { get; set; }
        public string? Course { get; set; }
        public string? Email { get; set; }
        public byte[]? QRCode { get; set; }
        public DateTime AddedAt { get; set; }
    }
}
