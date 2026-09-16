using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SeamsApp.Migrations
{
    /// <inheritdoc />
    public partial class AddOtpFieldsToStudentApplication : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EmailVerified",
                table: "StudentApplications",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "OtpCodeHash",
                table: "StudentApplications",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "OtpExpiresAt",
                table: "StudentApplications",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "OtpLastSentAt",
                table: "StudentApplications",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailVerified",
                table: "StudentApplications");

            migrationBuilder.DropColumn(
                name: "OtpCodeHash",
                table: "StudentApplications");

            migrationBuilder.DropColumn(
                name: "OtpExpiresAt",
                table: "StudentApplications");

            migrationBuilder.DropColumn(
                name: "OtpLastSentAt",
                table: "StudentApplications");
        }
    }
}
