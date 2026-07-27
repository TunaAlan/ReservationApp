using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ReservationApp.Migrations
{
    /// <inheritdoc />
    public partial class AddRestaurantTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "002c41ff-0c38-4633-8163-8289bf28c7a5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7393823f-caef-4430-9ff6-343a80ca96c5");

            migrationBuilder.DropColumn(
                name: "Capacity",
                table: "Restaurants");

            migrationBuilder.AddColumn<int>(
                name: "TableId",
                table: "Reservations",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RestaurantTables",
                columns: table => new
                {
                    TableId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RestaurantId = table.Column<int>(type: "int", nullable: false),
                    Label = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SeatCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RestaurantTables", x => x.TableId);
                    table.ForeignKey(
                        name: "FK_RestaurantTables_Restaurants_RestaurantId",
                        column: x => x.RestaurantId,
                        principalTable: "Restaurants",
                        principalColumn: "RestaurantId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "c0c0c23b-1ceb-4bb2-b0b3-e1212082eba9", null, "admin", "admin" },
                    { "f6d83ba9-e9dc-4202-9976-f4ea848a33fb", null, "client", "client" }
                });

            migrationBuilder.InsertData(
                table: "RestaurantTables",
                columns: new[] { "TableId", "Label", "RestaurantId", "SeatCount" },
                values: new object[,]
                {
                    { 1, "T1", 1, 6 },
                    { 2, "T2", 1, 4 },
                    { 3, "T3", 1, 4 },
                    { 4, "T4", 1, 2 },
                    { 5, "T5", 1, 2 },
                    { 6, "T6", 1, 8 },
                    { 7, "T7", 1, 6 },
                    { 8, "T8", 1, 4 },
                    { 9, "T9", 1, 4 },
                    { 10, "T10", 1, 2 },
                    { 11, "T11", 1, 2 },
                    { 12, "T12", 1, 8 },
                    { 13, "T13", 1, 6 },
                    { 14, "T14", 1, 4 },
                    { 15, "T15", 1, 4 },
                    { 16, "T16", 1, 2 },
                    { 17, "T17", 1, 2 },
                    { 18, "T18", 1, 8 },
                    { 19, "T19", 1, 6 },
                    { 20, "T20", 1, 4 },
                    { 21, "T21", 1, 4 },
                    { 22, "T22", 1, 2 },
                    { 23, "T23", 1, 2 },
                    { 24, "T24", 1, 8 },
                    { 25, "T25", 1, 6 },
                    { 26, "T26", 1, 4 },
                    { 27, "T27", 1, 4 },
                    { 28, "T28", 1, 2 },
                    { 29, "T1", 2, 6 },
                    { 30, "T2", 2, 4 },
                    { 31, "T3", 2, 4 },
                    { 32, "T4", 2, 2 },
                    { 33, "T5", 2, 2 },
                    { 34, "T6", 2, 8 },
                    { 35, "T7", 2, 6 },
                    { 36, "T8", 2, 4 },
                    { 37, "T9", 2, 4 },
                    { 38, "T10", 2, 2 },
                    { 39, "T11", 2, 2 },
                    { 40, "T12", 2, 8 },
                    { 41, "T13", 2, 6 },
                    { 42, "T14", 2, 4 },
                    { 43, "T15", 2, 4 },
                    { 44, "T16", 2, 2 },
                    { 45, "T17", 2, 2 },
                    { 46, "T18", 2, 8 },
                    { 47, "T19", 2, 6 },
                    { 48, "T20", 2, 4 },
                    { 49, "T21", 2, 4 },
                    { 50, "T22", 2, 2 },
                    { 51, "T23", 2, 2 },
                    { 52, "T24", 2, 8 },
                    { 53, "T25", 2, 6 },
                    { 54, "T26", 2, 4 },
                    { 55, "T27", 2, 4 },
                    { 56, "T28", 2, 2 },
                    { 57, "T29", 2, 2 },
                    { 58, "T30", 2, 8 },
                    { 59, "T31", 2, 6 },
                    { 60, "T32", 2, 4 },
                    { 61, "T33", 2, 4 },
                    { 62, "T34", 2, 2 },
                    { 63, "T35", 2, 2 },
                    { 64, "T36", 2, 2 },
                    { 65, "T1", 3, 6 },
                    { 66, "T2", 3, 4 },
                    { 67, "T3", 3, 4 },
                    { 68, "T4", 3, 2 },
                    { 69, "T5", 3, 2 },
                    { 70, "T6", 3, 8 },
                    { 71, "T7", 3, 6 },
                    { 72, "T8", 3, 4 },
                    { 73, "T9", 3, 4 },
                    { 74, "T10", 3, 2 },
                    { 75, "T11", 3, 2 },
                    { 76, "T12", 3, 8 },
                    { 77, "T13", 3, 6 },
                    { 78, "T14", 3, 4 },
                    { 79, "T15", 3, 4 },
                    { 80, "T16", 3, 2 },
                    { 81, "T17", 3, 2 },
                    { 82, "T18", 3, 8 },
                    { 83, "T19", 3, 2 },
                    { 84, "T1", 4, 6 },
                    { 85, "T2", 4, 4 },
                    { 86, "T3", 4, 4 },
                    { 87, "T4", 4, 2 },
                    { 88, "T5", 4, 2 },
                    { 89, "T6", 4, 8 },
                    { 90, "T7", 4, 6 },
                    { 91, "T8", 4, 4 },
                    { 92, "T9", 4, 4 },
                    { 93, "T10", 4, 2 },
                    { 94, "T11", 4, 2 },
                    { 95, "T12", 4, 8 },
                    { 96, "T13", 4, 6 },
                    { 97, "T14", 4, 4 },
                    { 98, "T15", 4, 4 },
                    { 99, "T16", 4, 2 },
                    { 100, "T17", 4, 2 },
                    { 101, "T1", 5, 6 },
                    { 102, "T2", 5, 4 },
                    { 103, "T3", 5, 4 },
                    { 104, "T4", 5, 2 },
                    { 105, "T5", 5, 2 },
                    { 106, "T6", 5, 8 },
                    { 107, "T7", 5, 6 },
                    { 108, "T8", 5, 4 },
                    { 109, "T9", 5, 4 },
                    { 110, "T10", 5, 2 },
                    { 111, "T11", 5, 2 },
                    { 112, "T12", 5, 8 },
                    { 113, "T13", 5, 6 },
                    { 114, "T14", 5, 2 },
                    { 115, "T1", 6, 6 },
                    { 116, "T2", 6, 4 },
                    { 117, "T3", 6, 4 },
                    { 118, "T4", 6, 2 },
                    { 119, "T5", 6, 2 },
                    { 120, "T6", 6, 8 },
                    { 121, "T7", 6, 6 },
                    { 122, "T8", 6, 4 },
                    { 123, "T9", 6, 4 },
                    { 124, "T10", 6, 2 },
                    { 125, "T11", 6, 2 },
                    { 126, "T12", 6, 1 },
                    { 127, "T1", 7, 6 },
                    { 128, "T2", 7, 4 },
                    { 129, "T3", 7, 4 },
                    { 130, "T4", 7, 2 },
                    { 131, "T5", 7, 2 },
                    { 132, "T6", 7, 8 },
                    { 133, "T7", 7, 6 },
                    { 134, "T8", 7, 4 },
                    { 135, "T9", 7, 4 },
                    { 136, "T10", 7, 2 },
                    { 137, "T11", 7, 2 },
                    { 138, "T12", 7, 6 },
                    { 139, "T1", 8, 6 },
                    { 140, "T2", 8, 4 },
                    { 141, "T3", 8, 4 },
                    { 142, "T4", 8, 2 },
                    { 143, "T5", 8, 2 },
                    { 144, "T6", 8, 8 },
                    { 145, "T7", 8, 6 },
                    { 146, "T8", 8, 4 },
                    { 147, "T9", 8, 4 },
                    { 148, "T10", 8, 2 },
                    { 149, "T11", 8, 2 },
                    { 150, "T12", 8, 8 },
                    { 151, "T13", 8, 6 },
                    { 152, "T14", 8, 2 },
                    { 153, "T1", 9, 6 },
                    { 154, "T2", 9, 4 },
                    { 155, "T3", 9, 4 },
                    { 156, "T4", 9, 2 },
                    { 157, "T5", 9, 2 },
                    { 158, "T6", 9, 8 },
                    { 159, "T7", 9, 6 },
                    { 160, "T8", 9, 4 },
                    { 161, "T9", 9, 4 },
                    { 162, "T10", 9, 2 },
                    { 163, "T11", 9, 2 },
                    { 164, "T12", 9, 8 },
                    { 165, "T13", 9, 6 },
                    { 166, "T14", 9, 4 },
                    { 167, "T15", 9, 4 },
                    { 168, "T16", 9, 2 },
                    { 169, "T17", 9, 2 },
                    { 170, "T1", 10, 6 },
                    { 171, "T2", 10, 4 },
                    { 172, "T3", 10, 4 },
                    { 173, "T4", 10, 2 },
                    { 174, "T5", 10, 2 },
                    { 175, "T6", 10, 8 },
                    { 176, "T7", 10, 6 },
                    { 177, "T8", 10, 4 },
                    { 178, "T9", 10, 4 },
                    { 179, "T10", 10, 2 },
                    { 180, "T11", 10, 2 },
                    { 181, "T12", 10, 8 },
                    { 182, "T13", 10, 6 },
                    { 183, "T14", 10, 4 },
                    { 184, "T15", 10, 4 },
                    { 185, "T16", 10, 2 },
                    { 186, "T17", 10, 2 },
                    { 187, "T18", 10, 8 },
                    { 188, "T19", 10, 2 },
                    { 189, "T1", 11, 6 },
                    { 190, "T2", 11, 4 },
                    { 191, "T3", 11, 4 },
                    { 192, "T4", 11, 2 },
                    { 193, "T5", 11, 2 },
                    { 194, "T6", 11, 8 },
                    { 195, "T7", 11, 6 },
                    { 196, "T8", 11, 4 },
                    { 197, "T9", 11, 4 },
                    { 198, "T1", 12, 6 },
                    { 199, "T2", 12, 4 },
                    { 200, "T3", 12, 4 },
                    { 201, "T4", 12, 2 },
                    { 202, "T5", 12, 2 },
                    { 203, "T6", 12, 8 },
                    { 204, "T7", 12, 6 },
                    { 205, "T8", 12, 3 },
                    { 206, "T1", 13, 6 },
                    { 207, "T2", 13, 4 },
                    { 208, "T3", 13, 4 },
                    { 209, "T4", 13, 2 },
                    { 210, "T5", 13, 2 },
                    { 211, "T6", 13, 8 },
                    { 212, "T7", 13, 6 },
                    { 213, "T8", 13, 4 },
                    { 214, "T9", 13, 4 },
                    { 215, "T10", 13, 2 },
                    { 216, "T11", 13, 2 },
                    { 217, "T12", 13, 8 },
                    { 218, "T13", 13, 6 },
                    { 219, "T14", 13, 4 },
                    { 220, "T15", 13, 4 },
                    { 221, "T16", 13, 2 },
                    { 222, "T17", 13, 2 },
                    { 223, "T18", 13, 8 },
                    { 224, "T19", 13, 6 },
                    { 225, "T20", 13, 4 },
                    { 226, "T21", 13, 2 },
                    { 227, "T1", 14, 6 },
                    { 228, "T2", 14, 4 },
                    { 229, "T3", 14, 4 },
                    { 230, "T4", 14, 2 },
                    { 231, "T5", 14, 2 },
                    { 232, "T6", 14, 8 },
                    { 233, "T7", 14, 6 },
                    { 234, "T8", 14, 4 },
                    { 235, "T9", 14, 4 },
                    { 236, "T10", 14, 2 },
                    { 237, "T11", 14, 2 },
                    { 238, "T12", 14, 8 },
                    { 239, "T13", 14, 6 },
                    { 240, "T14", 14, 4 },
                    { 241, "T15", 14, 4 },
                    { 242, "T16", 14, 2 },
                    { 243, "T17", 14, 2 },
                    { 244, "T18", 14, 8 },
                    { 245, "T19", 14, 6 },
                    { 246, "T20", 14, 1 },
                    { 247, "T1", 15, 6 },
                    { 248, "T2", 15, 4 },
                    { 249, "T3", 15, 4 },
                    { 250, "T4", 15, 2 },
                    { 251, "T5", 15, 2 },
                    { 252, "T6", 15, 8 },
                    { 253, "T7", 15, 4 },
                    { 254, "T1", 16, 6 },
                    { 255, "T2", 16, 4 },
                    { 256, "T3", 16, 4 },
                    { 257, "T4", 16, 2 },
                    { 258, "T5", 16, 2 },
                    { 259, "T6", 16, 7 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_TableId",
                table: "Reservations",
                column: "TableId");

            migrationBuilder.CreateIndex(
                name: "IX_RestaurantTables_RestaurantId",
                table: "RestaurantTables",
                column: "RestaurantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_RestaurantTables_TableId",
                table: "Reservations",
                column: "TableId",
                principalTable: "RestaurantTables",
                principalColumn: "TableId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_RestaurantTables_TableId",
                table: "Reservations");

            migrationBuilder.DropTable(
                name: "RestaurantTables");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_TableId",
                table: "Reservations");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c0c0c23b-1ceb-4bb2-b0b3-e1212082eba9");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f6d83ba9-e9dc-4202-9976-f4ea848a33fb");

            migrationBuilder.DropColumn(
                name: "TableId",
                table: "Reservations");

            migrationBuilder.AddColumn<int>(
                name: "Capacity",
                table: "Restaurants",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "002c41ff-0c38-4633-8163-8289bf28c7a5", null, "admin", "admin" },
                    { "7393823f-caef-4430-9ff6-343a80ca96c5", null, "client", "client" }
                });

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "RestaurantId",
                keyValue: 1,
                column: "Capacity",
                value: 120);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "RestaurantId",
                keyValue: 2,
                column: "Capacity",
                value: 150);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "RestaurantId",
                keyValue: 3,
                column: "Capacity",
                value: 80);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "RestaurantId",
                keyValue: 4,
                column: "Capacity",
                value: 70);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "RestaurantId",
                keyValue: 5,
                column: "Capacity",
                value: 60);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "RestaurantId",
                keyValue: 6,
                column: "Capacity",
                value: 45);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "RestaurantId",
                keyValue: 7,
                column: "Capacity",
                value: 50);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "RestaurantId",
                keyValue: 8,
                column: "Capacity",
                value: 60);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "RestaurantId",
                keyValue: 9,
                column: "Capacity",
                value: 70);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "RestaurantId",
                keyValue: 10,
                column: "Capacity",
                value: 80);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "RestaurantId",
                keyValue: 11,
                column: "Capacity",
                value: 40);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "RestaurantId",
                keyValue: 12,
                column: "Capacity",
                value: 35);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "RestaurantId",
                keyValue: 13,
                column: "Capacity",
                value: 90);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "RestaurantId",
                keyValue: 14,
                column: "Capacity",
                value: 85);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "RestaurantId",
                keyValue: 15,
                column: "Capacity",
                value: 30);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "RestaurantId",
                keyValue: 16,
                column: "Capacity",
                value: 25);
        }
    }
}
