using Microsoft.EntityFrameworkCore.Migrations;

namespace StreetLegal.Migrations
{
    public partial class PartsFixes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DriverId",
                table: "Parts",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Price",
                table: "Parts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Parts_DriverId",
                table: "Parts",
                column: "DriverId");

            migrationBuilder.AddForeignKey(
                name: "FK_Parts_Drivers_DriverId",
                table: "Parts",
                column: "DriverId",
                principalTable: "Drivers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Parts_Drivers_DriverId",
                table: "Parts");

            migrationBuilder.DropIndex(
                name: "IX_Parts_DriverId",
                table: "Parts");

            migrationBuilder.DropColumn(
                name: "DriverId",
                table: "Parts");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Parts");
        }
    }
}
