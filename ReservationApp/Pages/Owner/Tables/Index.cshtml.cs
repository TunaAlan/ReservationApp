using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ReservationApp.Models;
using ReservationApp.Services;

namespace ReservationApp.Pages.Owner.Tables
{
    [Authorize(Roles = "restaurant")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public Restaurant? Restaurant { get; set; }
        public List<RestaurantTable> RestaurantTables { get; set; } = new();
        public Dictionary<int, ReservationAvailability.TableStatus> TableStatuses { get; set; } = new();

        // Next 7 days' reservations per table (today through today+6), shown in each
        // table's detail modal as a schedule/ledger.
        public Dictionary<int, List<Reservation>> WeeklyReservationsByTable { get; set; } = new();

        public IndexModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = _userManager.GetUserId(User);
            Restaurant = await _context.Restaurants.Include(r => r.Settings).FirstOrDefaultAsync(r => r.OwnerUserId == userId);

            if (Restaurant == null || Restaurant.Settings == null)
            {
                return RedirectToPage("/Owner/Index");
            }

            RestaurantTables = await _context.RestaurantTables
                .Where(t => t.RestaurantId == Restaurant.RestaurantId)
                .OrderBy(t => t.Label)
                .ToListAsync();

            // Captured to variables: an inline DateTime.Now/Today in the query gets
            // translated by EF Core into SQL Server's GETDATE(), which runs on the DB
            // container's own clock/timezone rather than the app's.
            var now = DateTime.Now;
            var today = now.Date;
            var rangeEnd = today.AddDays(6);

            var weekReservations = await _context.Reservations
                .Include(r => r.User)
                .Where(r => r.RestaurantId == Restaurant.RestaurantId && r.ReservationDate.Date >= today && r.ReservationDate.Date <= rangeEnd)
                .OrderBy(r => r.ReservationDate)
                .ToListAsync();

            var todayReservations = weekReservations.Where(r => r.ReservationDate.Date == today).ToList();
            TableStatuses = ReservationAvailability.BuildTableStatuses(RestaurantTables, todayReservations, now);

            WeeklyReservationsByTable = RestaurantTables.ToDictionary(
                t => t.TableId,
                t => weekReservations.Where(r => r.TableId == t.TableId).ToList());

            return Page();
        }
    }
}
