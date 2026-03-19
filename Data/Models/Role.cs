using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SeamsApp.Data.Models;

public partial class Role
{
    [Key]
    public int RoleId { get; set; }

    [StringLength(100)]
    public string? RoleName { get; set; }
}
