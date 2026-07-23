namespace SeamsApp.DTOs.Student
{
    public class StudentResponse
    {
        public int StudentId { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? Suffix { get; set; }
        public string? Email { get; set; }
        public string? SchoolStudentId { get; set; }
        public string? YearLevel { get; set; }
        public string? Course { get; set; }
        public string? PhotoUrl { get; set; }
        public byte[]? QRCode { get; set; }
    }
}
