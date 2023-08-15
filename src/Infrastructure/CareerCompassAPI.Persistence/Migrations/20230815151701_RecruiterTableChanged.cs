using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareerCompassAPI.Persistence.Migrations
{
    public partial class RecruiterTableChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recruiters_JobLocations_JobLocationId",
                table: "Recruiters");

            migrationBuilder.DropIndex(
                name: "IX_Recruiters_JobLocationId",
                table: "Recruiters");

            migrationBuilder.DropColumn(
                name: "JobLocationId",
                table: "Recruiters");

            migrationBuilder.AddColumn<Guid>(
                name: "JobLocationId",
                table: "CompanyDetails",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CompanyDetails_JobLocationId",
                table: "CompanyDetails",
                column: "JobLocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyDetails_JobLocations_JobLocationId",
                table: "CompanyDetails",
                column: "JobLocationId",
                principalTable: "JobLocations",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyDetails_JobLocations_JobLocationId",
                table: "CompanyDetails");

            migrationBuilder.DropIndex(
                name: "IX_CompanyDetails_JobLocationId",
                table: "CompanyDetails");

            migrationBuilder.DropColumn(
                name: "JobLocationId",
                table: "CompanyDetails");

            migrationBuilder.AddColumn<Guid>(
                name: "JobLocationId",
                table: "Recruiters",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Recruiters_JobLocationId",
                table: "Recruiters",
                column: "JobLocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Recruiters_JobLocations_JobLocationId",
                table: "Recruiters",
                column: "JobLocationId",
                principalTable: "JobLocations",
                principalColumn: "Id");
        }
    }
}
