using System.ComponentModel.DataAnnotations;

namespace SeamsApp.Models
{
    public class Admin
    {
        [Key]
        public int AdminId { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
