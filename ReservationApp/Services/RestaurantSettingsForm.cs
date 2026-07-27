using ReservationApp.Models;

namespace ReservationApp.Services
{
    // Maps between the RestaurantSettings/RestaurantBusinessHour EF entities and
    // the RestaurantSettingsFormModel the Business Hours + Reservation Rules cards
    // bind to — shared by /Owner/Settings and /Admin/Restaurants/Settings so the
    // two pages can't drift apart on how a restaurant's settings are read/written.
    public static class RestaurantSettingsForm
    {
        // Monday-first display order for the week, independent of .NET's own
        // Sunday-first DayOfWeek enum ordering.
        private static readonly DayOfWeek[] WeekOrder =
        {
            DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday,
            DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday,
        };

        public static RestaurantSettingsFormModel Build(Restaurant restaurant)
        {
            var settings = restaurant.Settings!;
            var hoursByDay = restaurant.BusinessHours.ToDictionary(h => h.DayOfWeek);

            var form = new RestaurantSettingsFormModel
            {
                ReservationDurationMinutes = settings.ReservationDurationMinutes,
                BufferMinutes = settings.BufferMinutes,
                SlotGranularityMinutes = settings.SlotGranularityMinutes,
                MaxGuestsPerReservation = settings.MaxGuestsPerReservation,
                MinAdvanceBookingHours = settings.MinAdvanceBookingHours,
                MaxAdvanceBookingDays = settings.MaxAdvanceBookingDays,
                AcceptSameDayReservations = settings.AcceptSameDayReservations,
                AllowReservationNotes = settings.AllowReservationNotes,
                AutoConfirmReservations = settings.AutoConfirmReservations,
                AllowCancellation = settings.AllowCancellation,
                CancellationDeadlineHours = settings.CancellationDeadlineHours,
            };

            foreach (var day in WeekOrder)
            {
                hoursByDay.TryGetValue(day, out var hour);
                form.BusinessHours.Add(new BusinessHourRow
                {
                    DayOfWeek = day,
                    OpenTime = hour?.OpenTime,
                    CloseTime = hour?.CloseTime,
                    IsClosed = hour?.IsClosed ?? false,
                });
            }

            return form;
        }

        public static void Apply(Restaurant restaurant, RestaurantSettingsFormModel form)
        {
            var settings = restaurant.Settings!;
            settings.ReservationDurationMinutes = form.ReservationDurationMinutes;
            settings.BufferMinutes = form.BufferMinutes;
            settings.SlotGranularityMinutes = form.SlotGranularityMinutes;
            settings.MaxGuestsPerReservation = form.MaxGuestsPerReservation;
            settings.MinAdvanceBookingHours = form.MinAdvanceBookingHours;
            settings.MaxAdvanceBookingDays = form.MaxAdvanceBookingDays;
            settings.AcceptSameDayReservations = form.AcceptSameDayReservations;
            settings.AllowReservationNotes = form.AllowReservationNotes;
            settings.AutoConfirmReservations = form.AutoConfirmReservations;
            settings.AllowCancellation = form.AllowCancellation;
            settings.CancellationDeadlineHours = form.CancellationDeadlineHours;

            var hoursByDay = restaurant.BusinessHours.ToDictionary(h => h.DayOfWeek);
            foreach (var row in form.BusinessHours)
            {
                if (!hoursByDay.TryGetValue(row.DayOfWeek, out var hour))
                {
                    continue; // every restaurant always has all 7 rows — defensive only
                }

                hour.IsClosed = row.IsClosed;
                hour.OpenTime = row.IsClosed ? null : row.OpenTime;
                hour.CloseTime = row.IsClosed ? null : row.CloseTime;
            }
        }
    }
}
