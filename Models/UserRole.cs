namespace SeamsApp.Models
{
    public class UserRole
    {
        public int UserRoleId { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; } = 3; // DEFAULT ROLE ID AS 'Student'
        public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
    }
}
