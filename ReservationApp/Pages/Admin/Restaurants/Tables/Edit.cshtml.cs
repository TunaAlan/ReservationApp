using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ReservationApp.Models;
using ReservationApp.Services;

namespace ReservationApp.Pages.Admin.Restaurants.Tables
{
    [Authorize(Roles = "admin")]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        [BindProperty]
        public RestaurantTable RestaurantTable { get; set; } = new RestaurantTable();

        public Restaurant? Restaurant { get; set; }
        public string errorMessage = "";
        public string successMessage = "";

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var table = await _context.RestaurantTables.FirstOrDefaultAsync(t => t.TableId == id);
            if (table == null)
            {
                return NotFound();
            }

            RestaurantTable = table;
            Restaurant = await _context.Restaurants.FirstOrDefaultAsync(r => r.RestaurantId == table.RestaurantId);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var table = await _context.RestaurantTables.FirstOrDefaultAsync(t => t.TableId == id);
            if (table == null)
            {
                return NotFound();
            }

            Restaurant = await _context.Restaurants.FirstOrDefaultAsync(r => r.RestaurantId == table.RestaurantId);

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var labelTaken = await _context.RestaurantTables.AnyAsync(t =>
                t.TableId != id && t.RestaurantId == table.RestaurantId && t.Label == RestaurantTable.Label);
            if (labelTaken)
            {
                errorMessage = "This restaurant already has a table with that label.";
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
