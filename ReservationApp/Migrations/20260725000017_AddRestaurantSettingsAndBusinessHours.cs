using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ReservationApp.Migrations
{
    /// <inheritdoc />
    public partial class AddRestaurantSettingsAndBusinessHours : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RestaurantBusinessHours",
                columns: table => new
                {
                    BusinessHourId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RestaurantId = table.Column<int>(type: "int", nullable: false),
                    DayOfWeek = table.Column<int>(type: "int", nullable: false),
                    OpenTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    CloseTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    IsClosed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RestaurantBusinessHours", x => x.BusinessHourId);
                    table.ForeignKey(
                        name: "FK_RestaurantBusinessHours_Restaurants_RestaurantId",
                        column: x => x.RestaurantId,
                        principalTable: "Restaurants",
                        principalColumn: "RestaurantId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RestaurantSettings",
                columns: table => new
                {
                    SettingsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RestaurantId = table.Column<int>(type: "int", nullable: false),
                    ReservationDurationMinutes = table.Column<int>(type: "int", nullable: false),
                    BufferMinutes = table.Column<int>(type: "int", nullable: false),
                    SlotGranularityMinutes = table.Column<int>(type: "int", nullable: false),
                    MaxGuestsPerReservation = table.Column<int>(type: "int", nullable: true),
                    MinAdvanceBookingHours = table.Column<int>(type: "int", nullable: false),
                    MaxAdvanceBookingDays = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RestaurantSettings", x => x.SettingsId);
                    table.ForeignKey(
                        name: "FK_RestaurantSettings_Restaurants_RestaurantId",
                        column: x => x.RestaurantId,
                        principalTable: "Restaurants",
                        principalColumn: "RestaurantId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "RestaurantBusinessHours",
                columns: new[] { "BusinessHourId", "CloseTime", "DayOfWeek", "IsClosed", "OpenTime", "RestaurantId" },
                values: new object[,]
                {
                    { 1, new TimeSpan(0, 22, 0, 0, 0), 0, false, new TimeSpan(0, 11, 0, 0, 0), 1 },
                    { 2, new TimeSpan(0, 22, 0, 0, 0), 1, false, new TimeSpan(0, 11, 0, 0, 0), 1 },
                    { 3, new TimeSpan(0, 22, 0, 0, 0), 2, false, new TimeSpan(0, 11, 0, 0, 0), 1 },
                    { 4, new TimeSpan(0, 22, 0, 0, 0), 3, false, new TimeSpan(0, 11, 0, 0, 0), 1 },
                    { 5, new TimeSpan(0, 22, 0, 0, 0), 4, false, new TimeSpan(0, 11, 0, 0, 0), 1 },
                    { 6, new TimeSpan(0, 22, 0, 0, 0), 5, false, new TimeSpan(0, 11, 0, 0, 0), 1 },
                    { 7, new TimeSpan(0, 22, 0, 0, 0), 6, false, new TimeSpan(0, 11, 0, 0, 0), 1 },
                    { 8, new TimeSpan(0, 22, 0, 0, 0), 0, false, new TimeSpan(0, 11, 0, 0, 0), 2 },
                    { 9, new TimeSpan(0, 22, 0, 0, 0), 1, false, new TimeSpan(0, 11, 0, 0, 0), 2 },
                    { 10, new TimeSpan(0, 22, 0, 0, 0), 2, false, new TimeSpan(0, 11, 0, 0, 0), 2 },
                    { 11, new TimeSpan(0, 22, 0, 0, 0), 3, false, new TimeSpan(0, 11, 0, 0, 0), 2 },
                    { 12, new TimeSpan(0, 22, 0, 0, 0), 4, false, new TimeSpan(0, 11, 0, 0, 0), 2 },
                    { 13, new TimeSpan(0, 22, 0, 0, 0), 5, false, new TimeSpan(0, 11, 0, 0, 0), 2 },
                    { 14, new TimeSpan(0, 22, 0, 0, 0), 6, false, new TimeSpan(0, 11, 0, 0, 0), 2 },
                    { 15, new TimeSpan(0, 22, 0, 0, 0), 0, false, new TimeSpan(0, 11, 0, 0, 0), 3 },
                    { 16, new TimeSpan(0, 22, 0, 0, 0), 1, false, new TimeSpan(0, 11, 0, 0, 0), 3 },
                    { 17, new TimeSpan(0, 22, 0, 0, 0), 2, false, new TimeSpan(0, 11, 0, 0, 0), 3 },
                    { 18, new TimeSpan(0, 22, 0, 0, 0), 3, false, new TimeSpan(0, 11, 0, 0, 0), 3 },
                    { 19, new TimeSpan(0, 22, 0, 0, 0), 4, false, new TimeSpan(0, 11, 0, 0, 0), 3 },
                    { 20, new TimeSpan(0, 22, 0, 0, 0), 5, false, new TimeSpan(0, 11, 0, 0, 0), 3 },
                    { 21, new TimeSpan(0, 22, 0, 0, 0), 6, false, new TimeSpan(0, 11, 0, 0, 0), 3 },
                    { 22, new TimeSpan(0, 22, 0, 0, 0), 0, false, new TimeSpan(0, 11, 0, 0, 0), 4 },
                    { 23, new TimeSpan(0, 22, 0, 0, 0), 1, false, new TimeSpan(0, 11, 0, 0, 0), 4 },
                    { 24, new TimeSpan(0, 22, 0, 0, 0), 2, false, new TimeSpan(0, 11, 0, 0, 0), 4 },
                    { 25, new TimeSpan(0, 22, 0, 0, 0), 3, false, new TimeSpan(0, 11, 0, 0, 0), 4 },
                    { 26, new TimeSpan(0, 22, 0, 0, 0), 4, false, new TimeSpan(0, 11, 0, 0, 0), 4 },
                    { 27, new TimeSpan(0, 22, 0, 0, 0), 5, false, new TimeSpan(0, 11, 0, 0, 0), 4 },
                    { 28, new TimeSpan(0, 22, 0, 0, 0), 6, false, new TimeSpan(0, 11, 0, 0, 0), 4 },
                    { 29, new TimeSpan(0, 22, 0, 0, 0), 0, false, new TimeSpan(0, 11, 0, 0, 0), 5 },
                    { 30, new TimeSpan(0, 22, 0, 0, 0), 1, false, new TimeSpan(0, 11, 0, 0, 0), 5 },
                    { 31, new TimeSpan(0, 22, 0, 0, 0), 2, false, new TimeSpan(0, 11, 0, 0, 0), 5 },
                    { 32, new TimeSpan(0, 22, 0, 0, 0), 3, false, new TimeSpan(0, 11, 0, 0, 0), 5 },
                    { 33, new TimeSpan(0, 22, 0, 0, 0), 4, false, new TimeSpan(0, 11, 0, 0, 0), 5 },
                    { 34, new TimeSpan(0, 22, 0, 0, 0), 5, false, new TimeSpan(0, 11, 0, 0, 0), 5 },
                    { 35, new TimeSpan(0, 22, 0, 0, 0), 6, false, new TimeSpan(0, 11, 0, 0, 0), 5 },
                    { 36, new TimeSpan(0, 22, 0, 0, 0), 0, false, new TimeSpan(0, 11, 0, 0, 0), 6 },
                    { 37, new TimeSpan(0, 22, 0, 0, 0), 1, false, new TimeSpan(0, 11, 0, 0, 0), 6 },
                    { 38, new TimeSpan(0, 22, 0, 0, 0), 2, false, new TimeSpan(0, 11, 0, 0, 0), 6 },
                    { 39, new TimeSpan(0, 22, 0, 0, 0), 3, false, new TimeSpan(0, 11, 0, 0, 0), 6 },
                    { 40, new TimeSpan(0, 22, 0, 0, 0), 4, false, new TimeSpan(0, 11, 0, 0, 0), 6 },
                    { 41, new TimeSpan(0, 22, 0, 0, 0), 5, false, new TimeSpan(0, 11, 0, 0, 0), 6 },
                    { 42, new TimeSpan(0, 22, 0, 0, 0), 6, false, new TimeSpan(0, 11, 0, 0, 0), 6 },
                    { 43, new TimeSpan(0, 22, 0, 0, 0), 0, false, new TimeSpan(0, 11, 0, 0, 0), 7 },
                    { 44, new TimeSpan(0, 22, 0, 0, 0), 1, false, new TimeSpan(0, 11, 0, 0, 0), 7 },
                    { 45, new TimeSpan(0, 22, 0, 0, 0), 2, false, new TimeSpan(0, 11, 0, 0, 0), 7 },
                    { 46, new TimeSpan(0, 22, 0, 0, 0), 3, false, new TimeSpan(0, 11, 0, 0, 0), 7 },
                    { 47, new TimeSpan(0, 22, 0, 0, 0), 4, false, new TimeSpan(0, 11, 0, 0, 0), 7 },
                    { 48, new TimeSpan(0, 22, 0, 0, 0), 5, false, new TimeSpan(0, 11, 0, 0, 0), 7 },
                    { 49, new TimeSpan(0, 22, 0, 0, 0), 6, false, new TimeSpan(0, 11, 0, 0, 0), 7 },
                    { 50, new TimeSpan(0, 22, 0, 0, 0), 0, false, new TimeSpan(0, 11, 0, 0, 0), 8 },
                    { 51, new TimeSpan(0, 22, 0, 0, 0), 1, false, new TimeSpan(0, 11, 0, 0, 0), 8 },
                    { 52, new TimeSpan(0, 22, 0, 0, 0), 2, false, new TimeSpan(0, 11, 0, 0, 0), 8 },
                    { 53, new TimeSpan(0, 22, 0, 0, 0), 3, false, new TimeSpan(0, 11, 0, 0, 0), 8 },
                    { 54, new TimeSpan(0, 22, 0, 0, 0), 4, false, new TimeSpan(0, 11, 0, 0, 0), 8 },
                    { 55, new TimeSpan(0, 22, 0, 0, 0), 5, false, new TimeSpan(0, 11, 0, 0, 0), 8 },
                    { 56, new TimeSpan(0, 22, 0, 0, 0), 6, false, new TimeSpan(0, 11, 0, 0, 0), 8 },
                    { 57, new TimeSpan(0, 22, 0, 0, 0), 0, false, new TimeSpan(0, 11, 0, 0, 0), 9 },
                    { 58, new TimeSpan(0, 22, 0, 0, 0), 1, false, new TimeSpan(0, 11, 0, 0, 0), 9 },
                    { 59, new TimeSpan(0, 22, 0, 0, 0), 2, false, new TimeSpan(0, 11, 0, 0, 0), 9 },
                    { 60, new TimeSpan(0, 22, 0, 0, 0), 3, false, new TimeSpan(0, 11, 0, 0, 0), 9 },
                    { 61, new TimeSpan(0, 22, 0, 0, 0), 4, false, new TimeSpan(0, 11, 0, 0, 0), 9 },
                    { 62, new TimeSpan(0, 22, 0, 0, 0), 5, false, new TimeSpan(0, 11, 0, 0, 0), 9 },
                    { 63, new TimeSpan(0, 22, 0, 0, 0), 6, false, new TimeSpan(0, 11, 0, 0, 0), 9 },
                    { 64, new TimeSpan(0, 22, 0, 0, 0), 0, false, new TimeSpan(0, 11, 0, 0, 0), 10 },
                    { 65, new TimeSpan(0, 22, 0, 0, 0), 1, false, new TimeSpan(0, 11, 0, 0, 0), 10 },
                    { 66, new TimeSpan(0, 22, 0, 0, 0), 2, false, new TimeSpan(0, 11, 0, 0, 0), 10 },
                    { 67, new TimeSpan(0, 22, 0, 0, 0), 3, false, new TimeSpan(0, 11, 0, 0, 0), 10 },
                    { 68, new TimeSpan(0, 22, 0, 0, 0), 4, false, new TimeSpan(0, 11, 0, 0, 0), 10 },
                    { 69, new TimeSpan(0, 22, 0, 0, 0), 5, false, new TimeSpan(0, 11, 0, 0, 0), 10 },
                    { 70, new TimeSpan(0, 22, 0, 0, 0), 6, false, new TimeSpan(0, 11, 0, 0, 0), 10 },
                    { 71, new TimeSpan(0, 22, 0, 0, 0), 0, false, new TimeSpan(0, 11, 0, 0, 0), 11 },
                    { 72, new TimeSpan(0, 22, 0, 0, 0), 1, false, new TimeSpan(0, 11, 0, 0, 0), 11 },
                    { 73, new TimeSpan(0, 22, 0, 0, 0), 2, false, new TimeSpan(0, 11, 0, 0, 0), 11 },
                    { 74, new TimeSpan(0, 22, 0, 0, 0), 3, false, new TimeSpan(0, 11, 0, 0, 0), 11 },
                    { 75, new TimeSpan(0, 22, 0, 0, 0), 4, false, new TimeSpan(0, 11, 0, 0, 0), 11 },
                    { 76, new TimeSpan(0, 22, 0, 0, 0), 5, false, new TimeSpan(0, 11, 0, 0, 0), 11 },
                    { 77, new TimeSpan(0, 22, 0, 0, 0), 6, false, new TimeSpan(0, 11, 0, 0, 0), 11 },
                    { 78, new TimeSpan(0, 22, 0, 0, 0), 0, false, new TimeSpan(0, 11, 0, 0, 0), 12 },
                    { 79, new TimeSpan(0, 22, 0, 0, 0), 1, false, new TimeSpan(0, 11, 0, 0, 0), 12 },
                    { 80, new TimeSpan(0, 22, 0, 0, 0), 2, false, new TimeSpan(0, 11, 0, 0, 0), 12 },
                    { 81, new TimeSpan(0, 22, 0, 0, 0), 3, false, new TimeSpan(0, 11, 0, 0, 0), 12 },
                    { 82, new TimeSpan(0, 22, 0, 0, 0), 4, false, new TimeSpan(0, 11, 0, 0, 0), 12 },
                    { 83, new TimeSpan(0, 22, 0, 0, 0), 5, false, new TimeSpan(0, 11, 0, 0, 0), 12 },
                    { 84, new TimeSpan(0, 22, 0, 0, 0), 6, false, new TimeSpan(0, 11, 0, 0, 0), 12 },
                    { 85, new TimeSpan(0, 22, 0, 0, 0), 0, false, new TimeSpan(0, 11, 0, 0, 0), 13 },
                    { 86, new TimeSpan(0, 22, 0, 0, 0), 1, false, new TimeSpan(0, 11, 0, 0, 0), 13 },
                    { 87, new TimeSpan(0, 22, 0, 0, 0), 2, false, new TimeSpan(0, 11, 0, 0, 0), 13 },
                    { 88, new TimeSpan(0, 22, 0, 0, 0), 3, false, new TimeSpan(0, 11, 0, 0, 0), 13 },
                    { 89, new TimeSpan(0, 22, 0, 0, 0), 4, false, new TimeSpan(0, 11, 0, 0, 0), 13 },
                    { 90, new TimeSpan(0, 22, 0, 0, 0), 5, false, new TimeSpan(0, 11, 0, 0, 0), 13 },
                    { 91, new TimeSpan(0, 22, 0, 0, 0), 6, false, new TimeSpan(0, 11, 0, 0, 0), 13 },
                    { 92, new TimeSpan(0, 22, 0, 0, 0), 0, false, new TimeSpan(0, 11, 0, 0, 0), 14 },
                    { 93, new TimeSpan(0, 22, 0, 0, 0), 1, false, new TimeSpan(0, 11, 0, 0, 0), 14 },
                    { 94, new TimeSpan(0, 22, 0, 0, 0), 2, false, new TimeSpan(0, 11, 0, 0, 0), 14 },
                    { 95, new TimeSpan(0, 22, 0, 0, 0), 3, false, new TimeSpan(0, 11, 0, 0, 0), 14 },
                    { 96, new TimeSpan(0, 22, 0, 0, 0), 4, false, new TimeSpan(0, 11, 0, 0, 0), 14 },
                    { 97, new TimeSpan(0, 22, 0, 0, 0), 5, false, new TimeSpan(0, 11, 0, 0, 0), 14 },
                    { 98, new TimeSpan(0, 22, 0, 0, 0), 6, false, new TimeSpan(0, 11, 0, 0, 0), 14 },
                    { 99, new TimeSpan(0, 22, 0, 0, 0), 0, false, new TimeSpan(0, 11, 0, 0, 0), 15 },
                    { 100, new TimeSpan(0, 22, 0, 0, 0), 1, false, new TimeSpan(0, 11, 0, 0, 0), 15 },
                    { 101, new TimeSpan(0, 22, 0, 0, 0), 2, false, new TimeSpan(0, 11, 0, 0, 0), 15 },
                    { 102, new TimeSpan(0, 22, 0, 0, 0), 3, false, new TimeSpan(0, 11, 0, 0, 0), 15 },
                    { 103, new TimeSpan(0, 22, 0, 0, 0), 4, false, new TimeSpan(0, 11, 0, 0, 0), 15 },
                    { 104, new TimeSpan(0, 22, 0, 0, 0), 5, false, new TimeSpan(0, 11, 0, 0, 0), 15 },
                    { 105, new TimeSpan(0, 22, 0, 0, 0), 6, false, new TimeSpan(0, 11, 0, 0, 0), 15 },
                    { 106, new TimeSpan(0, 22, 0, 0, 0), 0, false, new TimeSpan(0, 11, 0, 0, 0), 16 },
                    { 107, new TimeSpan(0, 22, 0, 0, 0), 1, false, new TimeSpan(0, 11, 0, 0, 0), 16 },
                    { 108, new TimeSpan(0, 22, 0, 0, 0), 2, false, new TimeSpan(0, 11, 0, 0, 0), 16 },
                    { 109, new TimeSpan(0, 22, 0, 0, 0), 3, false, new TimeSpan(0, 11, 0, 0, 0), 16 },
                    { 110, new TimeSpan(0, 22, 0, 0, 0), 4, false, new TimeSpan(0, 11, 0, 0, 0), 16 },
                    { 111, new TimeSpan(0, 22, 0, 0, 0), 5, false, new TimeSpan(0, 11, 0, 0, 0), 16 },
                    { 112, new TimeSpan(0, 22, 0, 0, 0), 6, false, new TimeSpan(0, 11, 0, 0, 0), 16 }
                });

            migrationBuilder.InsertData(
                table: "RestaurantSettings",
                columns: new[] { "SettingsId", "BufferMinutes", "MaxAdvanceBookingDays", "MaxGuestsPerReservation", "MinAdvanceBookingHours", "ReservationDurationMinutes", "RestaurantId", "SlotGranularityMinutes" },
                values: new object[,]
                {
                    { 1, 0, 6, null, 0, 90, 1, 30 },
                    { 2, 0, 6, null, 0, 90, 2, 30 },
                    { 3, 0, 6, null, 0, 90, 3, 30 },
                    { 4, 0, 6, null, 0, 90, 4, 30 },
                    { 5, 0, 6, null, 0, 90, 5, 30 },
                    { 6, 0, 6, null, 0, 90, 6, 30 },
                    { 7, 0, 6, null, 0, 90, 7, 30 },
                    { 8, 0, 6, null, 0, 90, 8, 30 },
                    { 9, 0, 6, null, 0, 90, 9, 30 },
                    { 10, 0, 6, null, 0, 90, 10, 30 },
                    { 11, 0, 6, null, 0, 90, 11, 30 },
                    { 12, 0, 6, null, 0, 90, 12, 30 },
                    { 13, 0, 6, null, 0, 90, 13, 30 },
                    { 14, 0, 6, null, 0, 90, 14, 30 },
                    { 15, 0, 6, null, 0, 90, 15, 30 },
                    { 16, 0, 6, null, 0, 90, 16, 30 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_RestaurantBusinessHours_RestaurantId",
                table: "RestaurantBusinessHours",
                column: "RestaurantId");

            migrationBuilder.CreateIndex(
                name: "IX_RestaurantSettings_RestaurantId",
                table: "RestaurantSettings",
                column: "RestaurantId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RestaurantBusinessHours");

            migrationBuilder.DropTable(
                name: "RestaurantSettings");
        }
    }
}
