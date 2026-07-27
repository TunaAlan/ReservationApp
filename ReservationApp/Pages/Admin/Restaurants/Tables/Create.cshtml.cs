using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ReservationApp.Models;
using ReservationApp.Services;

namespace ReservationApp.Pages.Admin.Restaurants.Tables
{
    [Authorize(Roles = "admin")]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        [BindProperty]
        public RestaurantTable RestaurantTable { get; set; } = new RestaurantTable();

        public Restaurant? Restaurant { get; set; }
        public string errorMessage = "";

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(int restaurantId)
        {
            Restaurant = await _context.Restaurants.FirstOrDefaultAsync(r => r.RestaurantId == restaurantId);
            if (Restaurant == null)
            {
                return NotFound();
            }

            RestaurantTable.RestaurantId = restaurantId;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Restaurant = await _context.Restaurants.FirstOrDefaultAsync(r => r.RestaurantId == RestaurantTable.RestaurantId);
            if (Restaurant == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var labelTaken = await _context.RestaurantTables.AnyAsync(t =>
                t.RestaurantId == RestaurantTable.RestaurantId && t.Label == RestaurantTable.Label);
            if (labelTaken)
            {
                errorMessage = "This restaurant already has a table with that label.";
                return Page();
            }

            _context.RestaurantTables.Add(RestaurantTable);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index", new { restaurantId = RestaurantTable.RestaurantId });
        }
    }
}
