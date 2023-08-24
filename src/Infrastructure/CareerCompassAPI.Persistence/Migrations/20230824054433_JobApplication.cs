using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareerCompassAPI.Persistence.Migrations
{
    public partial class JobApplication : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobSeekers_Applications_JobApplicationsId",
                table: "JobSeekers");

            migrationBuilder.DropIndex(
                name: "IX_JobSeekers_JobApplicationsId",
                table: "JobSeekers");

            migrationBuilder.DropColumn(
                name: "JobApplicationsId",
                table: "JobSeekers");

            migrationBuilder.AlterColumn<int>(
                name: "Experience",
                table: "JobSeekerDetails",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<Guid>(
                name: "JobSeekerId",
                table: "Applications",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Applications_JobSeekerId",
                table: "Applications",
                column: "JobSeekerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_JobSeekers_JobSeekerId",
                table: "Applications",
                column: "JobSeekerId",
                principalTable: "JobSeekers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_JobSeekers_JobSeekerId",
                table: "Applications");

            migrationBuilder.DropIndex(
                name: "IX_Applications_JobSeekerId",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "JobSeekerId",
                table: "Applications");

            migrationBuilder.AddColumn<Guid>(
                name: "JobApplicationsId",
                table: "JobSeekers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Experience",
                table: "JobSeekerDetails",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_JobSeekers_JobApplicationsId",
                table: "JobSeekers",
                column: "JobApplicationsId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobSeekers_Applications_JobApplicationsId",
                table: "JobSeekers",
                column: "JobApplicationsId",
                principalTable: "Applications",
                principalColumn: "Id");
        }
    }
}
