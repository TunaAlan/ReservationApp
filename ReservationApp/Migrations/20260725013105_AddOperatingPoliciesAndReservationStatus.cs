using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReservationApp.Migrations
{
    /// <inheritdoc />
    public partial class AddOperatingPoliciesAndReservationStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AcceptSameDayReservations",
                table: "RestaurantSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AllowCancellation",
                table: "RestaurantSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AllowReservationNotes",
                table: "RestaurantSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AutoConfirmReservations",
                table: "RestaurantSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "CancellationDeadlineHours",
                table: "RestaurantSettings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Reservations",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Reservations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "RestaurantSettings",
                keyColumn: "SettingsId",
                keyValue: 1,
                columns: new[] { "AcceptSameDayReservations", "AllowCancellation", "AllowReservationNotes", "AutoConfirmReservations", "CancellationDeadlineHours" },
                values: new object[] { true, true, true, true, 0 });

            migrationBuilder.UpdateData(
                table: "RestaurantSettings",
                keyColumn: "SettingsId",
                keyValue: 2,
                columns: new[] { "AcceptSameDayReservations", "AllowCancellation", "AllowReservationNotes", "AutoConfirmReservations", "CancellationDeadlineHours" },
                values: new object[] { true, true, true, true, 0 });

            migrationBuilder.UpdateData(
                table: "RestaurantSettings",
                keyColumn: "SettingsId",
                keyValue: 3,
                columns: new[] { "AcceptSameDayReservations", "AllowCancellation", "AllowReservationNotes", "AutoConfirmReservations", "CancellationDeadlineHours" },
                values: new object[] { true, true, true, true, 0 });

            migrationBuilder.UpdateData(
                table: "RestaurantSettings",
                keyColumn: "SettingsId",
                keyValue: 4,
                columns: new[] { "AcceptSameDayReservations", "AllowCancellation", "AllowReservationNotes", "AutoConfirmReservations", "CancellationDeadlineHours" },
                values: new object[] { true, true, true, true, 0 });

            migrationBuilder.UpdateData(
                table: "RestaurantSettings",
                keyColumn: "SettingsId",
                keyValue: 5,
                columns: new[] { "AcceptSameDayReservations", "AllowCancellation", "AllowReservationNotes", "AutoConfirmReservations", "CancellationDeadlineHours" },
                values: new object[] { true, true, true, true, 0 });

            migrationBuilder.UpdateData(
                table: "RestaurantSettings",
                keyColumn: "SettingsId",
                keyValue: 6,
                columns: new[] { "AcceptSameDayReservations", "AllowCancellation", "AllowReservationNotes", "AutoConfirmReservations", "CancellationDeadlineHours" },
                values: new object[] { true, true, true, true, 0 });

            migrationBuilder.UpdateData(
                table: "RestaurantSettings",
                keyColumn: "SettingsId",
                keyValue: 7,
                columns: new[] { "AcceptSameDayReservations", "AllowCancellation", "AllowReservationNotes", "AutoConfirmReservations", "CancellationDeadlineHours" },
                values: new object[] { true, true, true, true, 0 });

            migrationBuilder.UpdateData(
                table: "RestaurantSettings",
                keyColumn: "SettingsId",
                keyValue: 8,
                columns: new[] { "AcceptSameDayReservations", "AllowCancellation", "AllowReservationNotes", "AutoConfirmReservations", "CancellationDeadlineHours" },
                values: new object[] { true, true, true, true, 0 });

            migrationBuilder.UpdateData(
                table: "RestaurantSettings",
                keyColumn: "SettingsId",
                keyValue: 9,
                columns: new[] { "AcceptSameDayReservations", "AllowCancellation", "AllowReservationNotes", "AutoConfirmReservations", "CancellationDeadlineHours" },
                values: new object[] { true, true, true, true, 0 });

            migrationBuilder.UpdateData(
                table: "RestaurantSettings",
                keyColumn: "SettingsId",
                keyValue: 10,
                columns: new[] { "AcceptSameDayReservations", "AllowCancellation", "AllowReservationNotes", "AutoConfirmReservations", "CancellationDeadlineHours" },
                values: new object[] { true, true, true, true, 0 });

            migrationBuilder.UpdateData(
                table: "RestaurantSettings",
                keyColumn: "SettingsId",
                keyValue: 11,
                columns: new[] { "AcceptSameDayReservations", "AllowCancellation", "AllowReservationNotes", "AutoConfirmReservations", "CancellationDeadlineHours" },
                values: new object[] { true, true, true, true, 0 });

            migrationBuilder.UpdateData(
                table: "RestaurantSettings",
                keyColumn: "SettingsId",
                keyValue: 12,
                columns: new[] { "AcceptSameDayReservations", "AllowCancellation", "AllowReservationNotes", "AutoConfirmReservations", "CancellationDeadlineHours" },
                values: new object[] { true, true, true, true, 0 });

            migrationBuilder.UpdateData(
                table: "RestaurantSettings",
                keyColumn: "SettingsId",
                keyValue: 13,
                columns: new[] { "AcceptSameDayReservations", "AllowCancellation", "AllowReservationNotes", "AutoConfirmReservations", "CancellationDeadlineHours" },
                values: new object[] { true, true, true, true, 0 });

            migrationBuilder.UpdateData(
                table: "RestaurantSettings",
                keyColumn: "SettingsId",
                keyValue: 14,
                columns: new[] { "AcceptSameDayReservations", "AllowCancellation", "AllowReservationNotes", "AutoConfirmReservations", "CancellationDeadlineHours" },
                values: new object[] { true, true, true, true, 0 });

            migrationBuilder.UpdateData(
                table: "RestaurantSettings",
                keyColumn: "SettingsId",
                keyValue: 15,
                columns: new[] { "AcceptSameDayReservations", "AllowCancellation", "AllowReservationNotes", "AutoConfirmReservations", "CancellationDeadlineHours" },
                values: new object[] { true, true, true, true, 0 });

            migrationBuilder.UpdateData(
                table: "RestaurantSettings",
                keyColumn: "SettingsId",
                keyValue: 16,
                columns: new[] { "AcceptSameDayReservations", "AllowCancellation", "AllowReservationNotes", "AutoConfirmReservations", "CancellationDeadlineHours" },
                values: new object[] { true, true, true, true, 0 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AcceptSameDayReservations",
                table: "RestaurantSettings");

            migrationBuilder.DropColumn(
                name: "AllowCancellation",
                table: "RestaurantSettings");

            migrationBuilder.DropColumn(
                name: "AllowReservationNotes",
                table: "RestaurantSettings");

            migrationBuilder.DropColumn(
                name: "AutoConfirmReservations",
                table: "RestaurantSettings");

            migrationBuilder.DropColumn(
                name: "CancellationDeadlineHours",
                table: "RestaurantSettings");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Reservations");
        }
    }
}
