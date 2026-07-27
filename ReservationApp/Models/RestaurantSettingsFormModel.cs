using System.ComponentModel.DataAnnotations;

namespace ReservationApp.Models
{
    // Form-binding shape for the Business Hours, Reservation Rules and Operating
    // Policies cards on /Owner/Settings and /Admin/Restaurants/Settings —
    // separate from the EF entities (RestaurantSettings, RestaurantBusinessHour)
    // the same way RestaurantDto is separate from Restaurant, so the POST body
    // only ever needs to contain what the form actually shows.
    public class RestaurantSettingsFormModel
    {
        [Range(15, 480)]
        public int ReservationDurationMinutes { get; set; } = 90;

        [Range(0, 120)]
        public int BufferMinutes { get; set; }

        [Range(5, 120)]
        public int SlotGranularityMinutes { get; set; } = 30;

        [Range(1, 1000)]
        public int? MaxGuestsPerReservation { get; set; }

        [Range(0, 720)]
        public int MinAdvanceBookingHours { get; set; }

        [Range(1, 365)]
        public int MaxAdvanceBookingDays { get; set; } = 6;

        public bool AcceptSameDayReservations { get; set; } = true;
        public bool AllowReservationNotes { get; set; } = true;
        public bool AutoConfirmReservations { get; set; } = true;
        public bool AllowCancellation { get; set; } = true;

        [Range(0, 720)]
        public int CancellationDeadlineHours { get; set; }

        // Always exactly 7 rows, Monday first — display order for the week,
        // independent of .NET's own Sunday-first DayOfWeek enum ordering.
        public List<BusinessHourRow> BusinessHours { get; set; } = new();
    }

    public class BusinessHourRow
    {
        public DayOfWeek DayOfWeek { get; set; }
        public TimeSpan? OpenTime { get; set; }
        public TimeSpan? CloseTime { get; set; }
        public bool IsClosed { get; set; }
    }
}
