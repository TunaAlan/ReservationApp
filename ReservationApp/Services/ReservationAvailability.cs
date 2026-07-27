using ReservationApp.Models;

namespace ReservationApp.Services
{
    // Table-level, turn-time/overlap based capacity model shared by every page that
    // needs to know whether a restaurant has room at a given time (reservation form,
    // restaurant list badges, admin dashboard) — kept in one place so all three stay
    // consistent. No table-combining: a reservation is seated at exactly one table
    // that's big enough to hold it.
    //
    // Every method here takes the restaurant's own RestaurantSettings (and, for slot
    // generation, its RestaurantBusinessHours) as a parameter instead of relying on a
    // single global constant — each restaurant configures its own turn-time, cleanup
    // buffer, slot spacing, and open/close hours via /Owner/Settings or
    // /Admin/Restaurants/Settings.
    public static class ReservationAvailability
    {
        // A reservation occupies its table for Duration, then stays unavailable for an
        // extra Buffer minutes (cleanup) before it's bookable again. Two reservations
        // "overlap" if their [start, start + Duration + Buffer) windows intersect.
        //
        // Asymmetric on purpose: `existing`'s window comes from its own DurationMinutes/
        // BufferMinutes snapshot (fixed at the moment it was booked), while
        // `candidateStart`'s window comes from the restaurant's current live settings —
        // this is what's being asked "if a new reservation started here right now, would
        // it collide with this already-booked one?" A restaurant editing its turn-time
        // later must not retroactively shrink an existing guest's occupied window.
        public static bool Overlaps(Reservation existing, DateTime candidateStart, RestaurantSettings settings)
        {
            var existingEnd = existing.ReservationDate.AddMinutes(existing.DurationMinutes + existing.BufferMinutes);
            var candidateEnd = candidateStart.AddMinutes(settings.ReservationDurationMinutes + settings.BufferMinutes);
            return existing.ReservationDate < candidateEnd && existingEnd > candidateStart;
        }

        private static HashSet<int> OccupiedTableIds(IEnumerable<Reservation> reservations, DateTime slotStart, RestaurantSettings settings)
        {
            return reservations
                .Where(r => r.TableId.HasValue && Overlaps(r, slotStart, settings))
                .Select(r => r.TableId!.Value)
                .ToHashSet();
        }

        // Best-fit table for the requested party size at the requested time: the
        // smallest free table that still seats the whole party (minimizes wasted seats).
        // Ties (same seat count) break on TableId so the choice is deterministic and
        // repeatable rather than depending on whatever order the DB happens to return —
        // otherwise two identical requests could get seated at different tables.
        // Null if nothing fits.
        public static RestaurantTable? FindBestTable(IEnumerable<RestaurantTable> tables, IEnumerable<Reservation> dayReservations, DateTime requestedStart, int partySize, RestaurantSettings settings)
        {
            var occupied = OccupiedTableIds(dayReservations, requestedStart, settings);
            return tables
                .Where(t => t.SeatCount >= partySize && !occupied.Contains(t.TableId))
                .OrderBy(t => t.SeatCount)
                .ThenBy(t => t.TableId)
                .FirstOrDefault();
        }

        // Whether at least one bookable slot on the given day still has a table free
        // for a party of the given size, considering only slots at or after minTime.
        public static bool HasAvailableTable(IEnumerable<RestaurantTable> tables, IEnumerable<Reservation> dayReservations, DateTime day, int partySize, RestaurantSettings settings, IEnumerable<RestaurantBusinessHour> businessHours, DateTime? minTime = null)
        {
            var tableList = tables.Where(t => t.SeatCount >= partySize).ToList();
            if (tableList.Count == 0)
            {
                return false;
            }

            var reservations = dayReservations.ToList();
            return GenerateTimeSlots(day, settings, businessHours, minTime).Any(slotStart =>
            {
                var occupied = OccupiedTableIds(reservations, slotStart, settings);
                return tableList.Any(t => !occupied.Contains(t.TableId));
            });
        }

        // Highest number of simultaneously occupied tables on the given day (busiest
        // moment), considering only slots at or after minTime. Used for a "how busy is
        // today" admin summary.
        public static int PeakOccupiedTables(IEnumerable<Reservation> dayReservations, DateTime day, RestaurantSettings settings, IEnumerable<RestaurantBusinessHour> businessHours, DateTime? minTime = null)
        {
            var reservations = dayReservations.ToList();
            var peak = 0;
            foreach (var slotStart in GenerateTimeSlots(day, settings, businessHours, minTime))
            {
                var occupied = OccupiedTableIds(reservations, slotStart, settings).Count;
                if (occupied > peak)
                {
                    peak = occupied;
                }
            }
            return peak;
        }

        // Bookable start times for the given day, derived from that day-of-week's
        // RestaurantBusinessHour row (spaced by settings.SlotGranularityMinutes)
        // instead of a single global 11:00-22:00/30-min list. Empty if the restaurant
        // has no hours configured for that day, or is explicitly closed.
        //
        // The slot boundary is the day's CloseTime itself (matches the original
        // hardcoded behavior: 11:00-22:00 produced slots up to 21:30, i.e. up to but
        // not including close) rather than CloseTime minus Duration — so, same as
        // before, a reservation booked at the very last slot can still run past
        // closing time. Making the last seating account for the full Duration is a
        // reasonable follow-up, tracked in future_works.md rather than changed here,
        // so default-settings restaurants keep producing identical slots to today.
        public static IEnumerable<DateTime> GenerateTimeSlots(DateTime day, RestaurantSettings settings, IEnumerable<RestaurantBusinessHour> businessHours, DateTime? minTime = null)
        {
            var hours = businessHours.FirstOrDefault(h => h.DayOfWeek == day.DayOfWeek);
            if (hours == null || hours.IsClosed || hours.OpenTime is null || hours.CloseTime is null)
            {
                yield break;
            }

            var step = TimeSpan.FromMinutes(settings.SlotGranularityMinutes);
            var closeAt = day.Date.Add(hours.CloseTime.Value);
            var slotStart = day.Date.Add(hours.OpenTime.Value);

            while (slotStart <= closeAt)
            {
                if (minTime is null || slotStart >= minTime.Value)
                {
                    yield return slotStart;
                }
                slotStart = slotStart.Add(step);
            }
        }

        public record WeeklyOccupancy(DateTime Date, int OccupancyPercent);

        // Seat-based occupancy per day for the restaurant's advance-booking window
        // (today through today+MaxAdvanceBookingDays, inclusive — same "N+1 total
        // days" semantics as the old hardcoded MaxBookingDaysAhead=6 constant it
        // replaces): at each day's busiest moment, what % of the restaurant's total
        // seats are blocked by an occupied table. A table counts as fully blocked
        // even if the party seated at it is smaller than the table — the seats
        // aren't bookable by anyone else either way.
        public static List<WeeklyOccupancy> BuildWeeklyOccupancy(IEnumerable<RestaurantTable> tables, IEnumerable<Reservation> weekReservations, DateTime rangeStart, RestaurantSettings settings, IEnumerable<RestaurantBusinessHour> businessHours)
        {
            var tableList = tables.ToList();
            var totalSeats = tableList.Sum(t => t.SeatCount);
            var reservations = weekReservations.ToList();
            var businessHoursList = businessHours.ToList();
            var result = new List<WeeklyOccupancy>();

            for (int offset = 0; offset <= settings.MaxAdvanceBookingDays; offset++)
            {
                var day = rangeStart.AddDays(offset);
                var dayReservations = reservations.Where(r => r.ReservationDate.Date == day).ToList();
                var minTime = day == DateTime.Today ? DateTime.Now : (DateTime?)null;

                var peakSeats = 0;
                foreach (var slotStart in GenerateTimeSlots(day, settings, businessHoursList, minTime))
                {
                    var occupiedIds = OccupiedTableIds(dayReservations, slotStart, settings);
                    var occupiedSeats = tableList.Where(t => occupiedIds.Contains(t.TableId)).Sum(t => t.SeatCount);
                    if (occupiedSeats > peakSeats)
                    {
                        peakSeats = occupiedSeats;
                    }
                }

                var percent = totalSeats > 0 ? (int)Math.Round(100.0 * peakSeats / totalSeats) : 0;
                result.Add(new WeeklyOccupancy(day, percent));
            }

            return result;
        }

        public record TableStatus(bool IsOccupiedNow, DateTime? NextReservation, int ReservationsToday);

        // Per-table live status for a floor-management view (admin/owner only — this is
        // operational detail, not something a diner needs or should see). Built from a
        // single day's reservations, keyed by TableId.
        public static Dictionary<int, TableStatus> BuildTableStatuses(IEnumerable<RestaurantTable> tables, IEnumerable<Reservation> todayReservations, DateTime now)
        {
            var reservations = todayReservations.ToList();

            return tables.ToDictionary(t => t.TableId, t =>
            {
                var forTable = reservations.Where(r => r.TableId == t.TableId).OrderBy(r => r.ReservationDate).ToList();
                var isOccupiedNow = forTable.Any(r => r.ReservationDate <= now && r.ReservationDate.AddMinutes(r.DurationMinutes + r.BufferMinutes) > now);
                var next = forTable.FirstOrDefault(r => r.ReservationDate >= now)?.ReservationDate;
                return new TableStatus(isOccupiedNow, next, forTable.Count);
            });
        }
    }
}
