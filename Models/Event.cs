using SeamsApp.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace SeamsApp.Models
{
    public class Event
    {
        [Key]
        public int EventId { get; set; }
        public string? Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Status { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
    }
}
