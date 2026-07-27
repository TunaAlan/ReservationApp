using System.ComponentModel.DataAnnotations;

namespace ReservationApp.Models
{
    // One row per restaurant. Everything the booking engine used to treat as a
    // single global constant (turn-time, buffer, slot spacing, advance-booking
    // window) lives here instead, so each restaurant can configure its own.
    public class RestaurantSettings
    {
        public int SettingsId { get; set; }

        [Required]
        public int RestaurantId { get; set; }
        public Restaurant? Restaurant { get; set; }

        // How long a table is occupied by a single reservation.
        [Range(15, 480)]
        public int ReservationDurationMinutes { get; set; } = 90;

        // Extra cleanup time after a reservation's Duration before the table is
        // considered free again — added on top of Duration, not part of it.
        [Range(0, 120)]
        public int BufferMinutes { get; set; } = 0;

        // How finely spaced the start times offered to the customer are.
        // Independent of Duration — e.g. a 90-minute reservation can still be
        // offered in 15-minute increments (18:00, 18:15, 18:30, ...).
        [Range(5, 120)]
        public int SlotGranularityMinutes { get; set; } = 30;

        // Null = no explicit cap (today's implicit behavior: bounded only by
        // the restaurant's largest table).
        [Range(1, 1000)]
        public int? MaxGuestsPerReservation { get; set; }

        [Range(0, 720)]
        public int MinAdvanceBookingHours { get; set; } = 0;

        [Range(1, 365)]
        public int MaxAdvanceBookingDays { get; set; } = 6;

        // Operating policies — defaults match today's implicit behavior exactly, so
        // enabling this feature doesn't change anything until an owner opts in.
        public bool AcceptSameDayReservations { get; set; } = true;
        public bool AllowReservationNotes { get; set; } = true;
        public bool AutoConfirmReservations { get; set; } = true;
        public bool AllowCancellation { get; set; } = true;

        // Hours before the reservation time after which a guest can no longer cancel.
        // 0 = no deadline (cancel any time up to the reservation itself).
        [Range(0, 720)]
        public int CancellationDeadlineHours { get; set; } = 0;
    }
}
