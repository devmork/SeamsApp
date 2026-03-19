using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SeamsApp.Data.Models;

public partial class User
{
    [Key]
    public int UserId { get; set; }

    [StringLength(250)]
    public string? Email { get; set; }

    [StringLength(250)]
    public string? PasswordHash { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdatedAt { get; set; }

    public int? StudentId { get; set; }

    [StringLength(100)]
    public string? UserRole { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? AssignedAt { get; set; }
}
