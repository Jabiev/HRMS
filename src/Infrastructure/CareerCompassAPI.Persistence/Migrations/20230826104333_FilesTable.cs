using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareerCompassAPI.Persistence.Migrations
{
    public partial class FilesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FileSize",
                table: "Files",
                newName: "Size");

            migrationBuilder.RenameColumn(
                name: "FileName",
                table: "Files",
                newName: "Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Size",
                table: "Files",
                newName: "FileSize");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Files",
                newName: "FileName");
        }
    }
}
