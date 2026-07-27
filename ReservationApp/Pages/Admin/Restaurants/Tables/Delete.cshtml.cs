using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ReservationApp.Models;
using ReservationApp.Services;

namespace ReservationApp.Pages.Admin.Restaurants.Tables
{
    [Authorize(Roles = "admin")]
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public RestaurantTable? RestaurantTable { get; set; }
        public Restaurant? Restaurant { get; set; }
        public int ActiveReservationsUsingIt { get; set; }
        public List<Reservation> ActiveReservations { get; set; } = new();
        public string errorMessage = "";

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET only shows a confirmation screen — it must never mutate data.
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            RestaurantTable = await _context.RestaurantTables.FirstOrDefaultAsync(t => t.TableId == id);
            if (RestaurantTable == null)
            {
                return NotFound();
            }

            Restaurant = await _context.Restaurants.FirstOrDefaultAsync(r => r.RestaurantId == RestaurantTable.RestaurantId);

            // Captured to a variable: an inline DateTime.Now in the query gets translated
            // by EF Core into SQL Server's GETDATE(), which runs on the DB container's
            // own clock/timezone rather than the app's.
            var now = DateTime.Now;
            ActiveReservations = await _context.Reservations
                .Include(r => r.User)
                .Where(r => r.TableId == id && r.ReservationDate >= now)
                .OrderBy(r => r.ReservationDate)
                .ToListAsync();
            ActiveReservationsUsingIt = ActiveReservations.Count;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var table = await _context.RestaurantTables.FirstOrDefaultAsync(t => t.TableId == id);
            if (table == null)
            {
                return NotFound();
            }

            Restaurant = await _context.Restaurants.FirstOrDefaultAsync(r => r.RestaurantId == table.RestaurantId);

            var now = DateTime.Now;
            var activeReservations = await _context.Reservations
                .Include(r => r.User)
                .Where(r => r.TableId == id && r.ReservationDate >= now)
                .OrderBy(r => r.ReservationDate)
                .ToListAsync();
            if (activeReservations.Count > 0)
            {
                RestaurantTable = table;
                ActiveReservations = activeReservations;
                ActiveReservationsUsingIt = activeReservations.Count;
                errorMessage = $"This table has {activeReservations.Count} upcoming reservation(s) and cannot be deleted.";
                return Page();
            }

            _context.RestaurantTables.Remove(table);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index", new { restaurantId = table.RestaurantId });
        }
    }
}
