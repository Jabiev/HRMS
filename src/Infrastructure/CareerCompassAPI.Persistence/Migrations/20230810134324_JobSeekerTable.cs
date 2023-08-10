using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareerCompassAPI.Persistence.Migrations
{
    public partial class JobSeekerTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobSeeker_AspNetUsers_AppUserId",
                table: "JobSeeker");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JobSeeker",
                table: "JobSeeker");

            migrationBuilder.RenameTable(
                name: "JobSeeker",
                newName: "JobSeekers");

            migrationBuilder.RenameIndex(
                name: "IX_JobSeeker_AppUserId",
                table: "JobSeekers",
                newName: "IX_JobSeekers_AppUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobSeekers",
                table: "JobSeekers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_JobSeekers_AspNetUsers_AppUserId",
                table: "JobSeekers",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobSeekers_AspNetUsers_AppUserId",
                table: "JobSeekers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JobSeekers",
                table: "JobSeekers");

            migrationBuilder.RenameTable(
                name: "JobSeekers",
                newName: "JobSeeker");

            migrationBuilder.RenameIndex(
                name: "IX_JobSeekers_AppUserId",
                table: "JobSeeker",
                newName: "IX_JobSeeker_AppUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobSeeker",
                table: "JobSeeker",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_JobSeeker_AspNetUsers_AppUserId",
                table: "JobSeeker",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
