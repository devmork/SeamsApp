using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SeamsApp.Data.Models;

[Index("AttendanceId", Name = "IX_AttendanceRecords_AttendanceID")]
[Index("SchoolStudentId", Name = "IX_AttendanceRecords_SchoolStudentID")]
public partial class AttendanceRecord
{
    [Key]
    [Column("AttendanceRecordID")]
    public int AttendanceRecordId { get; set; }

    [Column("AttendanceID")]
    public int AttendanceId { get; set; }

    [Column("SchoolStudentID")]
    public int SchoolStudentId { get; set; }

    public DateOnly TimeStamp { get; set; }

    public int Status { get; set; }

    [ForeignKey("AttendanceId")]
    [InverseProperty("AttendanceRecords")]
    public virtual Attendance Attendance { get; set; } = null!;

    [ForeignKey("SchoolStudentId")]
    [InverseProperty("AttendanceRecords")]
    public virtual Student SchoolStudent { get; set; } = null!;
}
