using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace trocitos.Data.Migrations
{
    public partial class AddedTablesToSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HighChairRequired",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "Outside",
                table: "Reservations");

            migrationBuilder.AddColumn<int>(
                name: "PartySize",
                table: "Reservations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TableNo",
                table: "Reservations",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PartySize",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "TableNo",
                table: "Reservations");

            migrationBuilder.AddColumn<bool>(
                name: "HighChairRequired",
                table: "Reservations",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Outside",
                table: "Reservations",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }
    }
}
