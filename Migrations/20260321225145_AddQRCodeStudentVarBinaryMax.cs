using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SeamsApp.Migrations
{
    /// <inheritdoc />
    public partial class AddQRCodeStudentVarBinaryMax : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "QRCode",
                table: "Students",
                type: "varbinary(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QRCode",
                table: "Students");
        }
    }
}
