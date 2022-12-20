using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AssignaApi.Migrations
{
    public partial class InitialV00 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "category",
                columns: table => new
                {
                    cat_id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    cat_name = table.Column<string>(maxLength: 50, nullable: false),
                    insertdate = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_category", x => x.cat_id);
                });

            migrationBuilder.CreateTable(
                name: "priority",
                columns: table => new
                {
                    pri_id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    pri_name = table.Column<string>(nullable: false),
                    insertdate = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_priority", x => x.pri_id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    user_id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_name = table.Column<string>(maxLength: 50, nullable: false),
                    first_name = table.Column<string>(maxLength: 50, nullable: false),
                    user_mail = table.Column<string>(maxLength: 50, nullable: false),
                    password_hash = table.Column<byte[]>(nullable: false),
                    password_salt = table.Column<byte[]>(nullable: false),
                    verify_token = table.Column<string>(nullable: false),
                    refresh_token = table.Column<string>(nullable: false),
                    refresh_expires = table.Column<DateTime>(nullable: false),
                    reset_token = table.Column<string>(nullable: true),
                    reset_expires = table.Column<DateTime>(nullable: true),
                    is_admin = table.Column<bool>(nullable: false),
                    insertdate = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "task",
                columns: table => new
                {
                    tsk_id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tsk_title = table.Column<string>(maxLength: 50, nullable: false),
                    deadline = table.Column<DateTime>(nullable: false),
                    tsk_note = table.Column<string>(maxLength: 250, nullable: false),
                    pending = table.Column<bool>(nullable: false),
                    complete = table.Column<bool>(nullable: false),
                    pri_high = table.Column<bool>(nullable: false),
                    pri_medium = table.Column<bool>(nullable: false),
                    pri_low = table.Column<bool>(nullable: false),
                    user_note = table.Column<string>(maxLength: 250, nullable: false),
                    insertdate = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    cat_id = table.Column<int>(nullable: false),
                    user_id = table.Column<int>(nullable: false),
                    Prioritypri_id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_task", x => x.tsk_id);
                    table.ForeignKey(
                        name: "FK_task_priority_Prioritypri_id",
                        column: x => x.Prioritypri_id,
                        principalTable: "priority",
                        principalColumn: "pri_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_task_category_cat_id",
                        column: x => x.cat_id,
                        principalTable: "category",
                        principalColumn: "cat_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_task_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_task_Prioritypri_id",
                table: "task",
                column: "Prioritypri_id");

            migrationBuilder.CreateIndex(
                name: "IX_task_cat_id",
                table: "task",
                column: "cat_id");

            migrationBuilder.CreateIndex(
                name: "IX_task_user_id",
                table: "task",
                column: "user_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "task");

            migrationBuilder.DropTable(
                name: "priority");

            migrationBuilder.DropTable(
                name: "category");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
