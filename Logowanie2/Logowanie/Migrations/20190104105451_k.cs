using Microsoft.EntityFrameworkCore.Migrations;

namespace Logowanie.Migrations
{
    public partial class k : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "stcz",
                table: "Leki",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "stcz",
                table: "Leki");
        }
    }
}
