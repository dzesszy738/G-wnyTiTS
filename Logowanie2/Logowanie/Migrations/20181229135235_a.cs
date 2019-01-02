using Microsoft.EntityFrameworkCore.Migrations;

namespace Logowanie.Migrations
{
    public partial class a : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Opis",
                table: "Wizyty",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Zalecenia",
                table: "Wizyty",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Opis",
                table: "Wizyty");

            migrationBuilder.DropColumn(
                name: "Zalecenia",
                table: "Wizyty");
        }
    }
}
