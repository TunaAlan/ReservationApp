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
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        [BindProperty]
        public RestaurantTable RestaurantTable { get; set; } = new RestaurantTable();

        public Restaurant? Restaurant { get; set; }
        public string errorMessage = "";
        public string successMessage = "";

        public EditModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        private async Task<Restaurant?> GetOwnRestaurantAsync()
        {
            var userId = _userManager.GetUserId(User);
            return await _context.Restaurants.FirstOrDefaultAsync(r => r.OwnerUserId == userId);
        }

        // A table only loads if it belongs to the logged-in owner's own restaurant —
        // guessing another restaurant's table id returns NotFound, not their data.
        public async Task<IActionResult> OnGetAsync(int id)
        {
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

            RestaurantTable = table;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
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

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var labelTaken = await _context.RestaurantTables.AnyAsync(t =>
                t.TableId != id && t.RestaurantId == Restaurant.RestaurantId && t.Label == RestaurantTable.Label);
            if (labelTaken)
            {
                errorMessage = "You already have a table with that label.";
                return Page();
            }

            table.Label = RestaurantTable.Label;
            table.SeatCount = RestaurantTable.SeatCount;
            await _context.SaveChangesAsync();

            successMessage = "Table updated successfully.";
            RestaurantTable = table;
            return Page();
        }
    }
}
