using Microsoft.EntityFrameworkCore.Migrations;

namespace Logowanie.Migrations
{
    public partial class m : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NaIle",
                table: "Leki",
                newName: "Od");

            migrationBuilder.AddColumn<string>(
                name: "Do",
                table: "Leki",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Do",
                table: "Leki");

            migrationBuilder.RenameColumn(
                name: "Od",
                table: "Leki",
                newName: "NaIle");
        }
    }
}
