namespace ReservationApp.Models
{
    // One row per day of the week per restaurant (7 rows total). OpenTime/
    // CloseTime are null when IsClosed is true — the day contributes zero
    // bookable slots.
    public class RestaurantBusinessHour
    {
        public int BusinessHourId { get; set; }

        public int RestaurantId { get; set; }
        public Restaurant? Restaurant { get; set; }

        public DayOfWeek DayOfWeek { get; set; }

        public TimeSpan? OpenTime { get; set; }
        public TimeSpan? CloseTime { get; set; }

        public bool IsClosed { get; set; }
    }
}
