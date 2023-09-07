using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareerCompassAPI.Persistence.Migrations
{
    public partial class ReviewPropertiesChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Reviews");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Rating",
                table: "Reviews",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
