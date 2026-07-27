using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ReservationApp.Migrations
{
    /// <inheritdoc />
    public partial class ReplaceImageFileNameWithGallery : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageFileName",
                table: "Restaurants");

            migrationBuilder.InsertData(
                table: "RestaurantImages",
                columns: new[] { "ImageId", "DisplayOrder", "FileName", "RestaurantId" },
                values: new object[,]
                {
                    { 1, 0, "oceans_bounty.jpg", 1 },
                    { 2, 0, "fishermans_wharf.jpg", 2 },
                    { 3, 0, "golden_fork.jpg", 3 },
                    { 4, 0, "elegance_palace.jpg", 4 },
                    { 5, 0, "quick_bite.jpg", 5 },
                    { 6, 0, "speedy_bites.jpg", 6 },
                    { 7, 0, "sakura_sushi.jpg", 7 },
                    { 8, 0, "tokyo_delight.jpg", 8 },
                    { 9, 0, "mamma_mia.jpg", 9 },
                    { 10, 0, "pasta_house.jpg", 10 },
                    { 11, 0, "brewed_awakening.jpg", 11 },
                    { 12, 0, "morning_brew.jpg", 12 },
                    { 13, 0, "grill_master.jpg", 13 },
                    { 14, 0, "prime_cut_grill.jpg", 14 },
                    { 15, 0, "le_petit_bistro.jpg", 15 },
                    { 16, 0, "bistro_bella.jpg", 16 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RestaurantImages",
                keyColumn: "ImageId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "RestaurantImages",
                keyColumn: "ImageId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "RestaurantImages",
                keyColumn: "ImageId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "RestaurantImages",
                keyColumn: "ImageId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "RestaurantImages",
                keyColumn: "ImageId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "RestaurantImages",
                keyColumn: "ImageId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "RestaurantImages",
                keyColumn: "ImageId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "RestaurantImages",
                keyColumn: "ImageId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "RestaurantImages",
                keyColumn: "ImageId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "RestaurantImages",
                keyColumn: "ImageId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "RestaurantImages",
                keyColumn: "ImageId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "RestaurantImages",
                keyColumn: "ImageId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "RestaurantImages",
                keyColumn: "ImageId",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "RestaurantImages",
                keyColumn: "ImageId",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "RestaurantImages",
                keyColumn: "ImageId",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "RestaurantImages",
                keyColumn: "ImageId",
                keyValue: 16);

            migrationBuilder.AddColumn<string>(
                name: "ImageFileName",
                table: "Restaurants",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "RestaurantId",
                keyValue: 1,
                column: "ImageFileName",
                value: "oceans_bounty.jpg");

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "RestaurantId",
                keyValue: 2,
                column: "ImageFileName",
                value: "fishermans_wharf.jpg");

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "RestaurantId",
                keyValue: 3,
                column: "ImageFileName",
                value: "golden_fork.jpg");

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "RestaurantId",
                keyValue: 4,
                column: "ImageFileName",
                value: "elegance_palace.jpg");

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "RestaurantId",
                keyValue: 5,
                column: "ImageFileName",
                value: "quick_bite.jpg");

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "RestaurantId",
                keyValue: 6,
                column: "ImageFileName",
                value: "speedy_bites.jpg");

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "RestaurantId",
                keyValue: 7,
                column: "ImageFileName",
                value: "sakura_sushi.jpg");

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "RestaurantId",
                keyValue: 8,
                column: "ImageFileName",
                value: "tokyo_delight.jpg");

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "RestaurantId",
                keyValue: 9,
                column: "ImageFileName",
                value: "mamma_mia.jpg");

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "RestaurantId",
                keyValue: 10,
                column: "ImageFileName",
                value: "pasta_house.jpg");

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "RestaurantId",
                keyValue: 11,
                column: "ImageFileName",
                value: "brewed_awakening.jpg");

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "RestaurantId",
                keyValue: 12,
                column: "ImageFileName",
                value: "morning_brew.jpg");

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "RestaurantId",
                keyValue: 13,
                column: "ImageFileName",
                value: "grill_master.jpg");

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "RestaurantId",
                keyValue: 14,
                column: "ImageFileName",
                value: "prime_cut_grill.jpg");

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "RestaurantId",
                keyValue: 15,
                column: "ImageFileName",
                value: "le_petit_bistro.jpg");

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "RestaurantId",
                keyValue: 16,
                column: "ImageFileName",
                value: "bistro_bella.jpg");
        }
    }
}
