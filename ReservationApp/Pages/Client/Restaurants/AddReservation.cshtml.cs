using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReservationApp.Models;
using ReservationApp.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;


namespace ReservationApp.Pages.Client.Restaurants
{
    [Authorize(Roles = "client")]
    public class AddReservationModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        // Dependency Injection Model
        public AddReservationModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        //Defining The Classes
        // Not [BindProperty]: this is only ever populated server-side (OnGet/OnPost),
        // never submitted by the form — binding it from POST body would run [Required]
        // validation against empty Restaurant fields the form never sends, breaking every submit.
        public Restaurant? Restaurant { get; set; } = new Restaurant();
        [BindProperty]
        public Reservation Reservation { get; set; } = new Reservation
        {
            ReservationDate = DateTime.Today,
            NumberOfPeople = 1,
        };
        public string? LoggedInUserName { get; set; } = "";

        // Error-Success Message
        public string errorMessage = "";
        [TempData]
        public string? SuccessMessage { get; set; }
        ////////////////////////

        public record DayAvailability(DateTime Date, bool HasAvailability);

        public List<DayAvailability> UpcomingAvailability { get; set; } = new();

        public record SlotAvailability(TimeSpan Time, bool Available);

        // Available times for the currently selected date + party size — the user
        // picks date/guests first, then clicks one of these to book (mirrors how
        // OpenTable/Resy show only bookable times instead of a blind time dropdown).
        public List<SlotAvailability> SlotsForSelectedDate { get; set; } = new();

        [BindProperty]
        public string SelectedTime { get; set; } = "";

        //Async OnGet Method
        public async Task<IActionResult> OnGetAsync(int id, DateTime? date, int? partySize)
        {
            Restaurant = await _context.Restaurants
                .Include(r => r.Category).Include(r => r.City).Include(r => r.Tables).Include(r => r.Images)
                .Include(r => r.Settings).Include(r => r.BusinessHours)
                .FirstOrDefaultAsync(r => r.RestaurantId == id);

            if (Restaurant == null || Restaurant.Settings == null)
            {
                return RedirectToPage("/Client/Restaurants/RestaurantList");
            }

            var settings = Restaurant.Settings;

            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                LoggedInUserName = user.UserName;
            }

            var effectiveDate = date?.Date ?? DateTime.Today;
            if (effectiveDate < DateTime.Today) effectiveDate = DateTime.Today;
            if (effectiveDate > DateTime.Today.AddDays(settings.MaxAdvanceBookingDays)) effectiveDate = DateTime.Today.AddDays(settings.MaxAdvanceBookingDays);

            var effectivePartySize = partySize ?? 2;
            if (effectivePartySize < 1) effectivePartySize = 1;

            Reservation.ReservationDate = effectiveDate;
            Reservation.NumberOfPeople = effectivePartySize;

            UpcomingAvailability = await BuildUpcomingAvailabilityAsync(id, Restaurant.Tables, settings, Restaurant.BusinessHours);
            SlotsForSelectedDate = await BuildSlotAvailabilityAsync(id, Restaurant.Tables, effectiveDate, effectivePartySize, settings, Restaurant.BusinessHours);

            return Page();
            ////////////////
        }

        private async Task<List<SlotAvailability>> BuildSlotAvailabilityAsync(int restaurantId, IEnumerable<RestaurantTable> tables, DateTime date, int partySize, RestaurantSettings settings, IEnumerable<RestaurantBusinessHour> businessHours)
        {
            var dayReservations = await _context.Reservations
                .Where(r => r.RestaurantId == restaurantId && r.ReservationDate.Date == date)
                .ToListAsync();

            var tableList = tables.ToList();
            var minTime = date == DateTime.Today ? DateTime.Now : (DateTime?)null;

            return ReservationAvailability.GenerateTimeSlots(date, settings, businessHours, minTime)
                .Select(slotStart => new SlotAvailability(
                    slotStart.TimeOfDay,
                    ReservationAvailability.FindBestTable(tableList, dayReservations, slotStart, partySize, settings) != null))
                .ToList();
        }

        private async Task<List<DayAvailability>> BuildUpcomingAvailabilityAsync(int restaurantId, IEnumerable<RestaurantTable> tables, RestaurantSettings settings, IEnumerable<RestaurantBusinessHour> businessHours)
        {
            var rangeStart = DateTime.Today;
            var rangeEnd = rangeStart.AddDays(settings.MaxAdvanceBookingDays);

            var reservations = await _context.Reservations
                .Where(r => r.RestaurantId == restaurantId && r.ReservationDate.Date >= rangeStart && r.ReservationDate.Date <= rangeEnd)
                .ToListAsync();

            var tableList = tables.ToList();
            var businessHoursList = businessHours.ToList();
            var result = new List<DayAvailability>();
            for (int offset = 0; offset <= settings.MaxAdvanceBookingDays; offset++)
            {
                var day = rangeStart.AddDays(offset);
                var dayReservations = reservations.Where(r => r.ReservationDate.Date == day).ToList();
                var minTime = day == DateTime.Today ? DateTime.Now : (DateTime?)null;
                // partySize 1 = "is there any table free at all", since the visitor
                // hasn't chosen a party size yet on this preview.
                var hasAvailability = ReservationAvailability.HasAvailableTable(tableList, dayReservations, day, 1, settings, businessHoursList, minTime);

                result.Add(new DayAvailability(day, hasAvailability));
            }

            return result;
        }


        //Async Post Method

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var restaurant = await _context.Restaurants
                .Include(r => r.Category).Include(r => r.City).Include(r => r.Tables).Include(r => r.Images)
                .Include(r => r.Settings).Include(r => r.BusinessHours)
                .FirstOrDefaultAsync(r => r.RestaurantId == id);

            if (restaurant == null || restaurant.Settings == null)
            {
                return NotFound();
            }

            var settings = restaurant.Settings;

            Restaurant = restaurant;
            var requestedDate = Reservation.ReservationDate.Date;
            var requestedPartySize = Reservation.NumberOfPeople;
            UpcomingAvailability = await BuildUpcomingAvailabilityAsync(id, restaurant.Tables, settings, restaurant.BusinessHours);
            SlotsForSelectedDate = await BuildSlotAvailabilityAsync(id, restaurant.Tables, requestedDate, requestedPartySize, settings, restaurant.BusinessHours);

            if (settings.MaxGuestsPerReservation.HasValue && requestedPartySize > settings.MaxGuestsPerReservation.Value)
            {
                errorMessage = $"This restaurant accepts at most {settings.MaxGuestsPerReservation.Value} guests per reservation.";
                return Page();
            }

            if (!settings.AcceptSameDayReservations && requestedDate == DateTime.Today)
            {
                errorMessage = "This restaurant does not accept same-day reservations. Please choose a later date.";
                return Page();
            }

            if (!TimeSpan.TryParse(SelectedTime, out var timeOfDay) ||
                !ReservationAvailability.GenerateTimeSlots(requestedDate, settings, restaurant.BusinessHours).Any(s => s.TimeOfDay == timeOfDay))
            {
                errorMessage = "Please select a valid reservation time.";
                return Page();
            }

            Reservation.ReservationDate = requestedDate + timeOfDay;

            if (Reservation.ReservationDate < DateTime.Now)
            {
                errorMessage = "Reservation date and time cannot be in the past.";
                return Page();
            }

            if (Reservation.ReservationDate.Date > DateTime.Today.AddDays(settings.MaxAdvanceBookingDays))
            {
                errorMessage = $"Reservations can only be made up to {settings.MaxAdvanceBookingDays + 1} days in advance.";
                return Page();
            }

            if (Reservation.ReservationDate < DateTime.Now.AddHours(settings.MinAdvanceBookingHours))
            {
                errorMessage = $"Reservations must be made at least {settings.MinAdvanceBookingHours} hour(s) in advance.";
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);

            if (user != null)
            {
                // "Active" = not yet past its reservation time. Computed at query time
                // rather than via a background job — no scheduled task needed to "expire" old reservations.
                // DateTime.Now is captured here rather than written inline in the Where —
                // EF Core translates an inline DateTime.Now/Today into SQL Server's own
                // GETDATE(), which uses the DB container's clock/timezone, not the app's.
                var now = DateTime.Now;
                var hasActiveReservation = await _context.Reservations.AnyAsync(r =>
                    r.RestaurantId == id &&
                    r.UserId == user.Id &&
                    r.ReservationDate >= now);

                if (hasActiveReservation)
                {
                    errorMessage = "You already have an active reservation at this restaurant.";
                    return Page();
                }

                Reservation.UserId = user.Id;
            }

            var dayReservations = await _context.Reservations
                .Where(r => r.RestaurantId == id && r.ReservationDate.Date == Reservation.ReservationDate.Date)
                .ToListAsync();

            var assignedTable = ReservationAvailability.FindBestTable(restaurant.Tables, dayReservations, Reservation.ReservationDate, Reservation.NumberOfPeople, settings);

            if (assignedTable == null)
            {
                errorMessage = "No table available for that time and party size. Try a different time, date, or fewer guests.";
                return Page();
            }

            //Arrange the reservation date
            Reservation.RestaurantId = id;
            Reservation.TableId = assignedTable.TableId;
            Reservation.CreatedAt = DateTime.Now;
            // Snapshot today's turn-time settings onto the reservation itself so a later
            // settings edit can't retroactively change how long this booking blocks its
            // table — see ReservationAvailability.Overlaps.
            Reservation.DurationMinutes = settings.ReservationDurationMinutes;
            Reservation.BufferMinutes = settings.BufferMinutes;
            // The Notes field is only ever shown to the guest when the restaurant opts
            // in — strip it server-side too, in case a request was crafted to include
            // one anyway.
            if (!settings.AllowReservationNotes)
            {
                Reservation.Notes = null;
            }
            Reservation.Status = settings.AutoConfirmReservations ? ReservationStatus.Confirmed : ReservationStatus.Pending;
            ////////////////////////////

            _context.Reservations.Add(Reservation);

            int changes = await _context.SaveChangesAsync();

            if (changes > 0)
            {
                SuccessMessage = "Reservation successfully created.";
                return RedirectToPage("/Client/Reservations/MyReservations"); // Redirect to a list of user reservations or another page
            }
            else
            {
                errorMessage = "Reservation could not be created.";
                return Page();
            }
        }
    }
}
