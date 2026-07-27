using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ReservationApp.Models;
using ReservationApp.Services;

namespace ReservationApp.Pages.Owner.Settings
{
    [Authorize(Roles = "restaurant")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        [BindProperty]
        public RestaurantSettingsFormModel Form { get; set; } = new();

        public string RestaurantName { get; set; } = "";
        public string successMessage = "";

        public IndexModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Every handler resolves the restaurant from the logged-in owner's Id rather
        // than trusting a route/query value — an owner can never act on another
        // restaurant's data, regardless of what's sent in the request.
        private async Task<Restaurant?> GetOwnRestaurantAsync()
        {
            var userId = _userManager.GetUserId(User);
            return await _context.Restaurants
                .Include(r => r.Settings)
                .Include(r => r.BusinessHours)
                .FirstOrDefaultAsync(r => r.OwnerUserId == userId);
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var restaurant = await GetOwnRestaurantAsync();
            if (restaurant == null || restaurant.Settings == null)
            {
                return RedirectToPage("/Owner/Index");
            }

            RestaurantName = restaurant.Name;
            Form = RestaurantSettingsForm.Build(restaurant);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var restaurant = await GetOwnRestaurantAsync();
            if (restaurant == null || restaurant.Settings == null)
            {
                return RedirectToPage("/Owner/Index");
            }

            RestaurantName = restaurant.Name;

            if (!ModelState.IsValid)
            {
                return Page();
            }

            RestaurantSettingsForm.Apply(restaurant, Form);
            await _context.SaveChangesAsync();

            successMessage = "Settings updated.";
            return Page();
        }
    }
}
