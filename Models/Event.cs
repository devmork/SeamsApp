using System.ComponentModel.DataAnnotations;

namespace SeamsApp.Models
{
    public class Event
    {
        [Key]
        public int EventId { get; set; }
        public string? Title { get; set; }
        public string? LogType { get; set; }
        public TimeOnly? StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
