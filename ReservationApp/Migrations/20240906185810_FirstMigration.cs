using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ReservationApp.Migrations
{
    /// <inheritdoc />
    public partial class FirstMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3fe3b7e2-fc86-4195-a91f-3ab10d2db18d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c6e4e7c1-91b2-4745-9f5f-3891b98cc130");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "12bbbcfb-1bbd-49b9-afbc-b6e6c27bba89", null, "admin", "admin" },
                    { "81a79a6d-f0e6-4e5b-8ee6-26066f49b20d", null, "client", "client" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "12bbbcfb-1bbd-49b9-afbc-b6e6c27bba89");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "81a79a6d-f0e6-4e5b-8ee6-26066f49b20d");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3fe3b7e2-fc86-4195-a91f-3ab10d2db18d", null, "admin", "admin" },
                    { "c6e4e7c1-91b2-4745-9f5f-3891b98cc130", null, "client", "client" }
                });
        }
    }
}
