using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SeamsApp.Migrations
{
    /// <inheritdoc />
    public partial class AddEventTargetYearLevels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TargetYearLevels",
                table: "Events",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TargetYearLevels",
                table: "Events");
        }
    }
}
