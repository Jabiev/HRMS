using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareerCompassAPI.Persistence.Migrations
{
    public partial class JobLocationChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Location",
                table: "Recruiters");

            migrationBuilder.RenameColumn(
                name: "JobLocationType",
                table: "JobLocations",
                newName: "Location");

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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.RenameColumn(
                name: "Location",
                table: "JobLocations",
                newName: "JobLocationType");

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Recruiters",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
