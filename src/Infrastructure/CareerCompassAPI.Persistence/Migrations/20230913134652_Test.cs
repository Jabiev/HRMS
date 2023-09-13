using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareerCompassAPI.Persistence.Migrations
{
    public partial class Test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Files_AspNetUsers_UserId",
                table: "Files");

            migrationBuilder.DropForeignKey(
                name: "FK_Followers_AspNetUsers_AppUserId",
                table: "Followers");

            migrationBuilder.DropForeignKey(
                name: "FK_Followers_AspNetUsers_UserId",
                table: "Followers");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_AspNetUsers_AppUserId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Followers_AppUserId",
                table: "Followers");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Followers");

            migrationBuilder.AlterColumn<string>(
                name: "AppUserId",
                table: "Payments",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "SenderId",
                table: "Messages",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "ReceiverId",
                table: "Messages",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Followers",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Files",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_Files_AspNetUsers_UserId",
                table: "Files",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Followers_AspNetUsers_UserId",
                table: "Followers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_AspNetUsers_AppUserId",
                table: "Payments",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Files_AspNetUsers_UserId",
                table: "Files");

            migrationBuilder.DropForeignKey(
                name: "FK_Followers_AspNetUsers_UserId",
                table: "Followers");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_AspNetUsers_AppUserId",
                table: "Payments");

            migrationBuilder.AlterColumn<string>(
                name: "AppUserId",
                table: "Payments",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SenderId",
                table: "Messages",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ReceiverId",
                table: "Messages",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Followers",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "Followers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Files",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Followers_AppUserId",
                table: "Followers",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Files_AspNetUsers_UserId",
                table: "Files",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Followers_AspNetUsers_AppUserId",
                table: "Followers",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Followers_AspNetUsers_UserId",
                table: "Followers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_AspNetUsers_AppUserId",
                table: "Payments",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
