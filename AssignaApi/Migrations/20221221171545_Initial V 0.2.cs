using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AssignaApi.Migrations
{
    public partial class InitialV02 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_task_priority_Prioritypri_id",
                table: "task");

            migrationBuilder.DropIndex(
                name: "IX_task_Prioritypri_id",
                table: "task");

            migrationBuilder.DropColumn(
                name: "Prioritypri_id",
                table: "task");

            migrationBuilder.AddColumn<DateTime>(
                name: "expires_at",
                table: "users",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "expires_at",
                table: "users");

            migrationBuilder.AddColumn<int>(
                name: "Prioritypri_id",
                table: "task",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_task_Prioritypri_id",
                table: "task",
                column: "Prioritypri_id");

            migrationBuilder.AddForeignKey(
                name: "FK_task_priority_Prioritypri_id",
                table: "task",
                column: "Prioritypri_id",
                principalTable: "priority",
                principalColumn: "pri_id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
