using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ReservationApp.Models;
using ReservationApp.Services;

namespace ReservationApp.Pages.Owner
{
    [Authorize(Roles = "restaurant")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public Restaurant? Restaurant { get; set; }
        public int BookedToday { get; set; }
        public List<ReservationAvailability.WeeklyOccupancy> WeeklyOccupancy { get; set; } = new();

        public IndexModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task OnGetAsync()
        {
            var userId = _userManager.GetUserId(User);

            Restaurant = await _context.Restaurants
                .Include(r => r.Category)
                .Include(r => r.City)
                .Include(r => r.Tables)
                .Include(r => r.Images)
                .Include(r => r.Settings)
                .Include(r => r.BusinessHours)
                .FirstOrDefaultAsync(r => r.OwnerUserId == userId);

            if (Restaurant == null || Restaurant.Settings == null)
            {
                return;
            }

            var rangeStart = DateTime.Today;
            var rangeEnd = rangeStart.AddDays(Restaurant.Settings.MaxAdvanceBookingDays);
            var weekReservations = await _context.Reservations
                .Where(r => r.RestaurantId == Restaurant.RestaurantId && r.ReservationDate.Date >= rangeStart && r.ReservationDate.Date <= rangeEnd)
                .ToListAsync();

            BookedToday = ReservationAvailability.PeakOccupiedTables(
                weekReservations.Where(r => r.ReservationDate.Date == rangeStart), rangeStart, Restaurant.Settings, Restaurant.BusinessHours);

            WeeklyOccupancy = ReservationAvailability.BuildWeeklyOccupancy(Restaurant.Tables, weekReservations, rangeStart, Restaurant.Settings, Restaurant.BusinessHours);
        }
    }
}
