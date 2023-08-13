using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareerCompassAPI.Persistence.Migrations
{
    public partial class RecruiterSubscriptionId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Subscriptions_SubscriptionId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_SubscriptionId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SubscriptionId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<Guid>(
                name: "SubscriptionId",
                table: "Recruiters",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SubscriptionsId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Recruiters_SubscriptionId",
                table: "Recruiters",
                column: "SubscriptionId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Recruiters_Subscriptions_SubscriptionId",
                table: "Recruiters",
                column: "SubscriptionId",
                principalTable: "Subscriptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Subscriptions_SubscriptionsId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Recruiters_Subscriptions_SubscriptionId",
                table: "Recruiters");

            migrationBuilder.DropIndex(
                name: "IX_Recruiters_SubscriptionId",
                table: "Recruiters");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_SubscriptionsId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SubscriptionId",
                table: "Recruiters");

            migrationBuilder.DropColumn(
                name: "SubscriptionsId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<Guid>(
                name: "SubscriptionId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_SubscriptionId",
                table: "AspNetUsers",
                column: "SubscriptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Subscriptions_SubscriptionId",
                table: "AspNetUsers",
                column: "SubscriptionId",
                principalTable: "Subscriptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
