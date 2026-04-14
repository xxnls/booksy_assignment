using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace booksy.API.Migrations
{
    /// <inheritdoc />
    public partial class RelationsChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RentalRecords_AspNetUsers_UserId",
                table: "RentalRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_RentalRecords_Hardwares_HardwareId",
                table: "RentalRecords");

            migrationBuilder.DropIndex(
                name: "IX_RentalRecords_HardwareId",
                table: "RentalRecords");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Hardwares",
                table: "Hardwares");

            migrationBuilder.RenameTable(
                name: "Hardwares",
                newName: "Hardware");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Hardware",
                table: "Hardware",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_RentalRecords_HardwareId_ReturnedAt",
                table: "RentalRecords",
                columns: new[] { "HardwareId", "ReturnedAt" },
                unique: true,
                filter: "\"ReturnedAt\" IS NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_RentalRecords_AspNetUsers_UserId",
                table: "RentalRecords",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RentalRecords_Hardware_HardwareId",
                table: "RentalRecords",
                column: "HardwareId",
                principalTable: "Hardware",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RentalRecords_AspNetUsers_UserId",
                table: "RentalRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_RentalRecords_Hardware_HardwareId",
                table: "RentalRecords");

            migrationBuilder.DropIndex(
                name: "IX_RentalRecords_HardwareId_ReturnedAt",
                table: "RentalRecords");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Hardware",
                table: "Hardware");

            migrationBuilder.RenameTable(
                name: "Hardware",
                newName: "Hardwares");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Hardwares",
                table: "Hardwares",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_RentalRecords_HardwareId",
                table: "RentalRecords",
                column: "HardwareId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RentalRecords_AspNetUsers_UserId",
                table: "RentalRecords",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RentalRecords_Hardwares_HardwareId",
                table: "RentalRecords",
                column: "HardwareId",
                principalTable: "Hardwares",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
