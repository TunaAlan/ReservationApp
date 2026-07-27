using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReservationApp.Models;
using ReservationApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ReservationApp.Pages.Admin.Restaurants
{
    [Authorize(Roles = "admin")]

    public class IndexModel: PageModel
    {
        private readonly ApplicationDbContext context ;
        public List<Restaurant> Restaurants { get; set;} = new List<Restaurant>();

        [BindProperty(SupportsGet = true)]
        public string? Search { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? CategoryId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? CityId { get; set; }

        public RestaurantFilter Filter { get; set; } = new();

        // Peak number of simultaneously occupied tables today per restaurant, computed
        // at page load (grouped query, no background job) — used to show a "booked
        // today" indicator per row.
        public Dictionary<int, int> BookedTodayByRestaurant { get; set; } = new Dictionary<int, int>();

        // Seat-based occupancy for the next 7 days per restaurant, shown in the details
        // modal.
        public Dictionary<int, List<ReservationAvailability.WeeklyOccupancy>> WeeklyOccupancyByRestaurant { get; set; } = new();

        public IndexModel(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task OnGetAsync()
        {
            var query = context.Restaurants.Include(r => r.Category).Include(r => r.City).Include(r => r.Tables).Include(r => r.OwnerUser).Include(r => r.Images)
                .Include(r => r.Settings).Include(r => r.BusinessHours).AsQueryable();

            if (!string.IsNullOrWhiteSpace(Search))
            {
                query = query.Where(r => r.Name.Contains(Search));
            }

            if (CategoryId.HasValue)
            {
                query = query.Where(r => r.CategoryId == CategoryId.Value);
            }

            if (CityId.HasValue)
            {
                query = query.Where(r => r.CityId == CityId.Value);
            }

            Restaurants = await query.OrderByDescending(p => p.RestaurantId).ToListAsync();

            var categories = await context.Categories.OrderBy(c => c.Name).ToListAsync();
            var cities = await context.Cities.OrderBy(c => c.Name).ToListAsync();
            Filter = new RestaurantFilter
            {
                ActionUrl = "/Admin/Restaurants/Index",
                Search = Search,
                CategoryId = CategoryId,
                CityId = CityId,
                CategoryOptions = new SelectList(categories, "CategoryId", "Name"),
                CityOptions = new SelectList(cities, "CityId", "Name"),
            };

            var rangeStart = DateTime.Today;
            var maxAdvanceDays = Restaurants.Count > 0 ? Restaurants.Max(r => r.Settings?.MaxAdvanceBookingDays ?? 6) : 6;
            var rangeEnd = rangeStart.AddDays(maxAdvanceDays);
            var weekReservations = await context.Reservations
                .Where(r => r.ReservationDate.Date >= rangeStart && r.ReservationDate.Date <= rangeEnd)
                .ToListAsync();

            // Peak occupied-table count today (busiest overlapping time slot), not a raw
            // headcount sum — under the turn-time model, reservations at different times
            // don't add up.
            BookedTodayByRestaurant = Restaurants
                .Where(r => r.Settings != null)
                .ToDictionary(
                    r => r.RestaurantId,
                    r => ReservationAvailability.PeakOccupiedTables(
                        weekReservations.Where(res => res.RestaurantId == r.RestaurantId && res.ReservationDate.Date == rangeStart), rangeStart, r.Settings!, r.BusinessHours));

            WeeklyOccupancyByRestaurant = Restaurants
                .Where(r => r.Settings != null)
                .ToDictionary(
                    r => r.RestaurantId,
                    r => ReservationAvailability.BuildWeeklyOccupancy(
                        r.Tables, weekReservations.Where(res => res.RestaurantId == r.RestaurantId), rangeStart, r.Settings!, r.BusinessHours));
        }
    }
}