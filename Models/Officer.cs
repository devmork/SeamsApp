using System.ComponentModel.DataAnnotations;

namespace SeamsApp.Models
{
    public class Officer
    {
        [Key]
        public int OfficerId { get; set; }
        public int UserId { get; set; }
        public int AddedBy { get; set; }
        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
    }
}
