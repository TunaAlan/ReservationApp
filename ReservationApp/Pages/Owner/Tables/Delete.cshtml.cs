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
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public RestaurantTable? RestaurantTable { get; set; }
        public Restaurant? Restaurant { get; set; }
        public int ActiveReservationsUsingIt { get; set; }
        public List<Reservation> ActiveReservations { get; set; } = new();
        public string errorMessage = "";

        public DeleteModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        private async Task<Restaurant?> GetOwnRestaurantAsync()
        {
            var userId = _userManager.GetUserId(User);
            return await _context.Restaurants.FirstOrDefaultAsync(r => r.OwnerUserId == userId);
        }

        // GET only shows a confirmation screen — it must never mutate data.
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Restaurant = await GetOwnRestaurantAsync();
            if (Restaurant == null)
            {
                return RedirectToPage("/Owner/Index");
            }

            RestaurantTable = await _context.RestaurantTables.FirstOrDefaultAsync(t => t.TableId == id && t.RestaurantId == Restaurant.RestaurantId);
            if (RestaurantTable == null)
            {
                return NotFound();
            }

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

            Restaurant = await GetOwnRestaurantAsync();
            if (Restaurant == null)
            {
                return RedirectToPage("/Owner/Index");
            }

            var table = await _context.RestaurantTables.FirstOrDefaultAsync(t => t.TableId == id && t.RestaurantId == Restaurant.RestaurantId);
            if (table == null)
            {
                return NotFound();
            }

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

            return RedirectToPage("./Index");
        }
    }
}
