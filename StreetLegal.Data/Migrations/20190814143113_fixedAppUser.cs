using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace StreetLegal.Migrations
{
    public partial class fixedAppUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cars_Drivers_DriverId",
                table: "Cars");

            migrationBuilder.RenameColumn(
                name: "DriverId",
                table: "Cars",
                newName: "GarageId");

            migrationBuilder.RenameIndex(
                name: "IX_Cars_DriverId",
                table: "Cars",
                newName: "IX_Cars_GarageId");

            migrationBuilder.AddColumn<int>(
                name: "GarageId",
                table: "Drivers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Garages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Garages", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Drivers_GarageId",
                table: "Drivers",
                column: "GarageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_Garages_GarageId",
                table: "Cars",
                column: "GarageId",
                principalTable: "Garages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Drivers_Garages_GarageId",
                table: "Drivers",
                column: "GarageId",
                principalTable: "Garages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cars_Garages_GarageId",
                table: "Cars");

            migrationBuilder.DropForeignKey(
                name: "FK_Drivers_Garages_GarageId",
                table: "Drivers");

            migrationBuilder.DropTable(
                name: "Garages");

            migrationBuilder.DropIndex(
                name: "IX_Drivers_GarageId",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "GarageId",
                table: "Drivers");

            migrationBuilder.RenameColumn(
                name: "GarageId",
                table: "Cars",
                newName: "DriverId");

            migrationBuilder.RenameIndex(
                name: "IX_Cars_GarageId",
                table: "Cars",
                newName: "IX_Cars_DriverId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_Drivers_DriverId",
                table: "Cars",
                column: "DriverId",
                principalTable: "Drivers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
