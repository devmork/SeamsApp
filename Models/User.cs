using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeamsApp.Models
{
    public class User
    {
        public Guid Id { get; set; }
        [Required]
        public string? UserName { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
        public string? Role { get; set; }
    }
}
