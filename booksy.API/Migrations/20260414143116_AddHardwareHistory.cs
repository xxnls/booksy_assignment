using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace booksy.API.Migrations
{
    /// <inheritdoc />
    public partial class AddHardwareHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "History",
                table: "Hardwares",
                type: "TEXT",
                maxLength: 2000,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "History",
                table: "Hardwares");
        }
    }
}
