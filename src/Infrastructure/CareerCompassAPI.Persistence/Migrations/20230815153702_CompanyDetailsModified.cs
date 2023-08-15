using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareerCompassAPI.Persistence.Migrations
{
    public partial class CompanyDetailsModified : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyDetails_JobLocations_JobLocationId",
                table: "CompanyDetails");

            migrationBuilder.RenameColumn(
                name: "JobLocationId",
                table: "CompanyDetails",
                newName: "LocationId");

            migrationBuilder.RenameIndex(
                name: "IX_CompanyDetails_JobLocationId",
                table: "CompanyDetails",
                newName: "IX_CompanyDetails_LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyDetails_JobLocations_LocationId",
                table: "CompanyDetails",
                column: "LocationId",
                principalTable: "JobLocations",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyDetails_JobLocations_LocationId",
                table: "CompanyDetails");

            migrationBuilder.RenameColumn(
                name: "LocationId",
                table: "CompanyDetails",
                newName: "JobLocationId");

            migrationBuilder.RenameIndex(
                name: "IX_CompanyDetails_LocationId",
                table: "CompanyDetails",
                newName: "IX_CompanyDetails_JobLocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyDetails_JobLocations_JobLocationId",
                table: "CompanyDetails",
                column: "JobLocationId",
                principalTable: "JobLocations",
                principalColumn: "Id");
        }
    }
}
