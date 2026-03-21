using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SeamsApp.Migrations
{
    /// <inheritdoc />
    public partial class DroppedQRCodeStudentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "QRCode",
                table: "Students");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StudentId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QRCode",
                table: "Students",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }
    }
}
