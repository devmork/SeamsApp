namespace SeamsApp.Models
{
    public class UserRole
    {
        public int UserRoleId { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
    }
}
