using Microsoft.EntityFrameworkCore.Migrations;

namespace StreetLegal.Migrations
{
    public partial class Added_Tyres : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Tyres",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Tyres");
        }
    }
}
