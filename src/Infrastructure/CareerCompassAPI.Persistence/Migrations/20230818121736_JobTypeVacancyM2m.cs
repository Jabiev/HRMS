using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareerCompassAPI.Persistence.Migrations
{
    public partial class JobTypeVacancyM2m : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vacancy_JobTypes_JobTypeId",
                table: "Vacancy");

            migrationBuilder.DropIndex(
                name: "IX_Vacancy_JobTypeId",
                table: "Vacancy");

            migrationBuilder.DropColumn(
                name: "JobTypeId",
                table: "Vacancy");

            migrationBuilder.CreateTable(
                name: "JobTypeVacancy",
                columns: table => new
                {
                    JobTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VacanciesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobTypeVacancy", x => new { x.JobTypeId, x.VacanciesId });
                    table.ForeignKey(
                        name: "FK_JobTypeVacancy_JobTypes_JobTypeId",
                        column: x => x.JobTypeId,
                        principalTable: "JobTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JobTypeVacancy_Vacancy_VacanciesId",
                        column: x => x.VacanciesId,
                        principalTable: "Vacancy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobTypeVacancy_VacanciesId",
                table: "JobTypeVacancy",
                column: "VacanciesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JobTypeVacancy");

            migrationBuilder.AddColumn<Guid>(
                name: "JobTypeId",
                table: "Vacancy",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Vacancy_JobTypeId",
                table: "Vacancy",
                column: "JobTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vacancy_JobTypes_JobTypeId",
                table: "Vacancy",
                column: "JobTypeId",
                principalTable: "JobTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
