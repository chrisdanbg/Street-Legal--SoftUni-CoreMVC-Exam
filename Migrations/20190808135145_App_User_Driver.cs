using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace StreetLegal.Migrations
{
    public partial class App_User_Driver : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DriverName",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "DriverId",
                table: "Cars",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DriverId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Drivers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    MainCarId = table.Column<int>(nullable: true),
                    Level = table.Column<int>(nullable: false),
                    Experience = table.Column<int>(nullable: false),
                    Money = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drivers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Drivers_Cars_MainCarId",
                        column: x => x.MainCarId,
                        principalTable: "Cars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cars_DriverId",
                table: "Cars",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_DriverId",
                table: "AspNetUsers",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_Drivers_MainCarId",
                table: "Drivers",
                column: "MainCarId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Drivers_DriverId",
                table: "AspNetUsers",
                column: "DriverId",
                principalTable: "Drivers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_Drivers_DriverId",
                table: "Cars",
                column: "DriverId",
                principalTable: "Drivers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Drivers_DriverId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Cars_Drivers_DriverId",
                table: "Cars");

            migrationBuilder.DropTable(
                name: "Drivers");

            migrationBuilder.DropIndex(
                name: "IX_Cars_DriverId",
                table: "Cars");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_DriverId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DriverId",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "DriverId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "DriverName",
                table: "AspNetUsers",
                nullable: true);
        }
    }
}
