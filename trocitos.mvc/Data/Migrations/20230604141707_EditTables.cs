using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace trocitos.Data.Migrations
{
    public partial class EditTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Tables_TableNo",
                table: "Reservations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tables",
                table: "Tables");

            migrationBuilder.RenameTable(
                name: "Tables",
                newName: "TableCatalogue");

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "TableCatalogue",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TableCatalogue",
                table: "TableCatalogue",
                column: "TableNo");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_TableCatalogue_TableNo",
                table: "Reservations",
                column: "TableNo",
                principalTable: "TableCatalogue",
                principalColumn: "TableNo",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_TableCatalogue_TableNo",
                table: "Reservations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TableCatalogue",
                table: "TableCatalogue");

            migrationBuilder.RenameTable(
                name: "TableCatalogue",
                newName: "Tables");

            migrationBuilder.AlterColumn<int>(
                name: "Location",
                table: "Tables",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tables",
                table: "Tables",
                column: "TableNo");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Tables_TableNo",
                table: "Reservations",
                column: "TableNo",
                principalTable: "Tables",
                principalColumn: "TableNo",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
