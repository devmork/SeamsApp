using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SeamsApp.Data.Models;

public partial class Student
{
    [Key]
    public int StudentId { get; set; }

    [StringLength(250)]
    public string FirstName { get; set; } = null!;

    [StringLength(250)]
    public string? MiddleName { get; set; }

    [StringLength(250)]
    public string LastName { get; set; } = null!;

    [StringLength(8)]
    public string SchoolStudentId { get; set; } = null!;

    [StringLength(4)]
    [Unicode(false)]
    public string YearLevel { get; set; } = null!;

    [StringLength(250)]
    public string Course { get; set; } = null!;

    [StringLength(250)]
    public string Email { get; set; } = null!;

    [Column("QRCode")]
    [StringLength(500)]
    public string Qrcode { get; set; } = null!;

    public int Status { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime SubmittedAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime UpdatedAt { get; set; }

    [StringLength(10)]
    public string? Suffix { get; set; }

    [StringLength(500)]
    public string? PhotoUrl { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ApprovedAt { get; set; }

    [InverseProperty("SchoolStudent")]
    public virtual ICollection<AttendanceRecord> AttendanceRecords { get; set; } = new List<AttendanceRecord>();
}
