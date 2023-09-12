using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareerCompassAPI.Persistence.Migrations
{
    public partial class userDeleteFollowersCascade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Followers_AspNetUsers_UserId",
                table: "Followers");

            migrationBuilder.AddForeignKey(
                name: "FK_Followers_AspNetUsers_UserId",
                table: "Followers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Followers_AspNetUsers_UserId",
                table: "Followers");

            migrationBuilder.AddForeignKey(
                name: "FK_Followers_AspNetUsers_UserId",
                table: "Followers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
