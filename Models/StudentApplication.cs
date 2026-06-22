namespace SeamsApp.Models
{
    public class StudentApplication
    {
        public int ApplicationId { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? Suffix { get; set; }
        public string? Email { get; set; }
        public string? SchoolStudentId { get; set; }
        public string? YearLevel { get; set; }
        public string? Course { get; set; }
        public string? PhotoUrl { get; set; }
        public int Status { get; set; }
        public string? DeclineReason { get; set; }
        public int ReviewedBy { get; set; }
        public DateTime ReviewedAt { get; set; } = DateTime.UtcNow;
        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;

    }
}
