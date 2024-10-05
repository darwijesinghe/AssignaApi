using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AssignaApi.Migrations
{
    public partial class AddedExternalLoginUserTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "category",
                keyColumn: "cat_id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "category",
                keyColumn: "cat_id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "category",
                keyColumn: "cat_id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "category",
                keyColumn: "cat_id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "category",
                keyColumn: "cat_id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "category",
                keyColumn: "cat_id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "priority",
                keyColumn: "pri_id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "priority",
                keyColumn: "pri_id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "priority",
                keyColumn: "pri_id",
                keyValue: 3);

            migrationBuilder.AddColumn<int>(
                name: "ExternalUsersuser_id",
                table: "task",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "external_users",
                columns: table => new
                {
                    user_id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(maxLength: 50, nullable: false),
                    given_name = table.Column<string>(maxLength: 50, nullable: false),
                    family_name = table.Column<string>(maxLength: 50, nullable: false),
                    picture = table.Column<string>(nullable: false),
                    email = table.Column<string>(nullable: false),
                    email_verified = table.Column<bool>(nullable: false),
                    locale = table.Column<string>(nullable: false),
                    verify_token = table.Column<string>(nullable: false),
                    expires_at = table.Column<DateTime>(nullable: false),
                    refresh_token = table.Column<string>(nullable: false),
                    refresh_expires = table.Column<DateTime>(nullable: false),
                    reset_token = table.Column<string>(nullable: true),
                    reset_expires = table.Column<DateTime>(nullable: true),
                    is_admin = table.Column<bool>(nullable: false),
                    insertdate = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_external_users", x => x.user_id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_task_ExternalUsersuser_id",
                table: "task",
                column: "ExternalUsersuser_id");

            migrationBuilder.AddForeignKey(
                name: "FK_task_external_users_ExternalUsersuser_id",
                table: "task",
                column: "ExternalUsersuser_id",
                principalTable: "external_users",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_task_external_users_ExternalUsersuser_id",
                table: "task");

            migrationBuilder.DropTable(
                name: "external_users");

            migrationBuilder.DropIndex(
                name: "IX_task_ExternalUsersuser_id",
                table: "task");

            migrationBuilder.DropColumn(
                name: "ExternalUsersuser_id",
                table: "task");

            migrationBuilder.InsertData(
                table: "category",
                columns: new[] { "cat_id", "cat_name" },
                values: new object[,]
                {
                    { 1, "Team Task" },
                    { 2, "Individual Task" },
                    { 3, "Home Task" },
                    { 4, "Finance Task" },
                    { 5, "Client Task" },
                    { 6, "Reasearch Task" }
                });

            migrationBuilder.InsertData(
                table: "priority",
                columns: new[] { "pri_id", "pri_name" },
                values: new object[,]
                {
                    { 1, "High" },
                    { 2, "Medium" },
                    { 3, "Low" }
                });
        }
    }
}
