using Microsoft.EntityFrameworkCore.Migrations;

namespace Logowanie.Migrations
{
    public partial class poprawka : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Telefon",
                table: "Pacjenci",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Uczulenia",
                table: "Pacjenci",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Telefon",
                table: "Pacjenci");

            migrationBuilder.DropColumn(
                name: "Uczulenia",
                table: "Pacjenci");
        }
    }
}
