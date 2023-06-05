using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace trocitos.Data.Migrations
{
    public partial class UpdateTableModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Booked",
                table: "TableCatalogue");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Booked",
                table: "TableCatalogue",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }
    }
}
