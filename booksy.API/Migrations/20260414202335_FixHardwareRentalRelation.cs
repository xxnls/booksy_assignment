using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace booksy.API.Migrations
{
    /// <inheritdoc />
    public partial class FixHardwareRentalRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_RentalRecords_HardwareId",
                table: "RentalRecords",
                column: "HardwareId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RentalRecords_HardwareId",
                table: "RentalRecords");
        }
    }
}
