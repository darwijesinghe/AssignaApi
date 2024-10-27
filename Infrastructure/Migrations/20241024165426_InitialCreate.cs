using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    CatId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CatName = table.Column<string>(maxLength: 50, nullable: false),
                    InsertDate = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.CatId);
                });

            migrationBuilder.CreateTable(
                name: "Priority",
                columns: table => new
                {
                    PriorityId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PriorityName = table.Column<string>(nullable: false),
                    InsertDate = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Priority", x => x.PriorityId);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(maxLength: 50, nullable: false),
                    FirstName = table.Column<string>(maxLength: 50, nullable: false),
                    UserMail = table.Column<string>(maxLength: 50, nullable: false),
                    PasswordHash = table.Column<byte[]>(nullable: true),
                    PasswordSalt = table.Column<byte[]>(nullable: true),
                    GivenName = table.Column<string>(maxLength: 50, nullable: true),
                    FamilyName = table.Column<string>(maxLength: 50, nullable: true),
                    Picture = table.Column<string>(nullable: true),
                    EmailVerified = table.Column<bool>(nullable: false),
                    Locale = table.Column<string>(nullable: true),
                    VerifyToken = table.Column<string>(nullable: false),
                    ExpiresAt = table.Column<DateTime>(nullable: false),
                    RefreshToken = table.Column<string>(nullable: false),
                    RefreshExpires = table.Column<DateTime>(nullable: false),
                    ResetToken = table.Column<string>(nullable: true),
                    ResetExpires = table.Column<DateTime>(nullable: true),
                    IsAdmin = table.Column<bool>(nullable: false),
                    InsertDate = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Task",
                columns: table => new
                {
                    TaskId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaskTitle = table.Column<string>(maxLength: 50, nullable: false),
                    Deadline = table.Column<DateTime>(nullable: false),
                    TaskNote = table.Column<string>(maxLength: 250, nullable: false),
                    Pending = table.Column<bool>(nullable: false),
                    Complete = table.Column<bool>(nullable: false),
                    HighPriority = table.Column<bool>(nullable: false),
                    MediumPriority = table.Column<bool>(nullable: false),
                    LowPriority = table.Column<bool>(nullable: false),
                    UserNote = table.Column<string>(maxLength: 250, nullable: true),
                    InsertDate = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    CatId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Task", x => x.TaskId);
                    table.ForeignKey(
                        name: "FK_Task_Category_CatId",
                        column: x => x.CatId,
                        principalTable: "Category",
                        principalColumn: "CatId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Task_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Category",
                columns: new[] { "CatId", "CatName" },
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
                table: "Priority",
                columns: new[] { "PriorityId", "PriorityName" },
                values: new object[,]
                {
                    { 1, "High" },
                    { 2, "Medium" },
                    { 3, "Low" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Task_CatId",
                table: "Task",
                column: "CatId");

            migrationBuilder.CreateIndex(
                name: "IX_Task_UserId",
                table: "Task",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Priority");

            migrationBuilder.DropTable(
                name: "Task");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
