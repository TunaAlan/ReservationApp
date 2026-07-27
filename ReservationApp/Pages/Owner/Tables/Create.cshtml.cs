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
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        [BindProperty]
        public RestaurantTable RestaurantTable { get; set; } = new RestaurantTable();

        public Restaurant? Restaurant { get; set; }
        public string errorMessage = "";

        public CreateModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        private async Task<Restaurant?> GetOwnRestaurantAsync()
        {
            var userId = _userManager.GetUserId(User);
            return await _context.Restaurants.FirstOrDefaultAsync(r => r.OwnerUserId == userId);
        }

        public async Task<IActionResult> OnGetAsync()
        {
            Restaurant = await GetOwnRestaurantAsync();
            if (Restaurant == null)
            {
                return RedirectToPage("/Owner/Index");
            }

            RestaurantTable.RestaurantId = Restaurant.RestaurantId;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Restaurant = await GetOwnRestaurantAsync();
            if (Restaurant == null)
            {
                return RedirectToPage("/Owner/Index");
            }

            // Always the owner's own restaurant — never trust a posted RestaurantId.
            // Also clear any stale binding-time validation error for it (e.g. if the
            // client sent it blank), since we're about to overwrite the value anyway.
            RestaurantTable.RestaurantId = Restaurant.RestaurantId;
            ModelState.Remove("RestaurantTable.RestaurantId");

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var labelTaken = await _context.RestaurantTables.AnyAsync(t =>
                t.RestaurantId == Restaurant.RestaurantId && t.Label == RestaurantTable.Label);
            if (labelTaken)
            {
                errorMessage = "You already have a table with that label.";
                return Page();
            }

            _context.RestaurantTables.Add(RestaurantTable);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
