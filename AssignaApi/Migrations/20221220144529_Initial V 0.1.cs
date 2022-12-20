using Microsoft.EntityFrameworkCore.Migrations;

namespace AssignaApi.Migrations
{
    public partial class InitialV01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
