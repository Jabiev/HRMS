using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareerCompassAPI.Persistence.Migrations
{
    public partial class VacancyRelatedEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Subscriptions_SubscriptionsId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_SubscriptionsId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SubscriptionsId",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "ExperienceLevels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LevelName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExperienceLevels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JobLocations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    JobLocationType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobLocations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JobTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TypeName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShiftAndSchedules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ShiftName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShiftAndSchedules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vacancy",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    JobTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExperienceLevelId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RecruiterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Salary = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    JobTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    JobLocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vacancy", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vacancy_ExperienceLevels_ExperienceLevelId",
                        column: x => x.ExperienceLevelId,
                        principalTable: "ExperienceLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Vacancy_JobLocations_JobLocationId",
                        column: x => x.JobLocationId,
                        principalTable: "JobLocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Vacancy_JobTypes_JobTypeId",
                        column: x => x.JobTypeId,
                        principalTable: "JobTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Vacancy_Recruiters_RecruiterId",
                        column: x => x.RecruiterId,
                        principalTable: "Recruiters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShiftAndScheduleVacancy",
                columns: table => new
                {
                    ShiftAndSchedulesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VacanciesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShiftAndScheduleVacancy", x => new { x.ShiftAndSchedulesId, x.VacanciesId });
                    table.ForeignKey(
                        name: "FK_ShiftAndScheduleVacancy_ShiftAndSchedules_ShiftAndSchedulesId",
                        column: x => x.ShiftAndSchedulesId,
                        principalTable: "ShiftAndSchedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShiftAndScheduleVacancy_Vacancy_VacanciesId",
                        column: x => x.VacanciesId,
                        principalTable: "Vacancy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShiftAndScheduleVacancy_VacanciesId",
                table: "ShiftAndScheduleVacancy",
                column: "VacanciesId");

            migrationBuilder.CreateIndex(
                name: "IX_Vacancy_ExperienceLevelId",
                table: "Vacancy",
                column: "ExperienceLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_Vacancy_JobLocationId",
                table: "Vacancy",
                column: "JobLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Vacancy_JobTypeId",
                table: "Vacancy",
                column: "JobTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Vacancy_RecruiterId",
                table: "Vacancy",
                column: "RecruiterId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShiftAndScheduleVacancy");

            migrationBuilder.DropTable(
                name: "ShiftAndSchedules");

            migrationBuilder.DropTable(
                name: "Vacancy");

            migrationBuilder.DropTable(
                name: "ExperienceLevels");

            migrationBuilder.DropTable(
                name: "JobLocations");

            migrationBuilder.DropTable(
                name: "JobTypes");

            migrationBuilder.AddColumn<Guid>(
                name: "SubscriptionsId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_SubscriptionsId",
                table: "AspNetUsers",
                column: "SubscriptionsId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Subscriptions_SubscriptionsId",
                table: "AspNetUsers",
                column: "SubscriptionsId",
                principalTable: "Subscriptions",
                principalColumn: "Id");
        }
    }
}
