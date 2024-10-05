using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AssignaApi.Migrations
{
    public partial class RemovedExternalLoginUserTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<byte[]>(
                name: "password_salt",
                table: "users",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)");

            migrationBuilder.AlterColumn<byte[]>(
                name: "password_hash",
                table: "users",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)");

            migrationBuilder.AddColumn<string>(
                name: "email",
                table: "users",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "email_verified",
                table: "users",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "family_name",
                table: "users",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "given_name",
                table: "users",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "locale",
                table: "users",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "picture",
                table: "users",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "email",
                table: "users");

            migrationBuilder.DropColumn(
                name: "email_verified",
                table: "users");

            migrationBuilder.DropColumn(
                name: "family_name",
                table: "users");

            migrationBuilder.DropColumn(
                name: "given_name",
                table: "users");

            migrationBuilder.DropColumn(
                name: "locale",
                table: "users");

            migrationBuilder.DropColumn(
                name: "picture",
                table: "users");

            migrationBuilder.AlterColumn<byte[]>(
                name: "password_salt",
                table: "users",
                type: "varbinary(max)",
                nullable: false,
                oldClrType: typeof(byte[]),
                oldNullable: true);

            migrationBuilder.AlterColumn<byte[]>(
                name: "password_hash",
                table: "users",
                type: "varbinary(max)",
                nullable: false,
                oldClrType: typeof(byte[]),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExternalUsersuser_id",
                table: "task",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "external_users",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    email_verified = table.Column<bool>(type: "bit", nullable: false),
                    expires_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    family_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    given_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    insertdate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    is_admin = table.Column<bool>(type: "bit", nullable: false),
                    locale = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    picture = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    refresh_expires = table.Column<DateTime>(type: "datetime2", nullable: false),
                    refresh_token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    reset_expires = table.Column<DateTime>(type: "datetime2", nullable: true),
                    reset_token = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    verify_token = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
    }
}
