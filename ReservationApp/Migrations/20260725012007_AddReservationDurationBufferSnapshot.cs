using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReservationApp.Migrations
{
    /// <inheritdoc />
    public partial class AddReservationDurationBufferSnapshot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BufferMinutes",
                table: "Reservations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DurationMinutes",
                table: "Reservations",
                type: "int",
                nullable: false,
                defaultValue: 90);

            // Backfill existing reservations from their restaurant's current settings —
            // the closest available approximation of "what was in effect when this was
            // booked" for rows that predate this column existing.
            migrationBuilder.Sql(@"
                UPDATE res
                SET res.DurationMinutes = s.ReservationDurationMinutes,
                    res.BufferMinutes = s.BufferMinutes
                FROM Reservations res
                JOIN RestaurantSettings s ON s.RestaurantId = res.RestaurantId;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BufferMinutes",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "DurationMinutes",
                table: "Reservations");
        }
    }
}
