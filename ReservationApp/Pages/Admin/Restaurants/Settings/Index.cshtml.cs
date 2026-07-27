using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ReservationApp.Models;
using ReservationApp.Services;

namespace ReservationApp.Pages.Admin.Restaurants.Settings
{
    [Authorize(Roles = "admin")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        [BindProperty]
        public RestaurantSettingsFormModel Form { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public int RestaurantId { get; set; }

        public string RestaurantName { get; set; } = "";
        public string successMessage = "";

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        private async Task<Restaurant?> GetRestaurantAsync()
        {
            return await _context.Restaurants
                .Include(r => r.Settings)
                .Include(r => r.BusinessHours)
                .FirstOrDefaultAsync(r => r.RestaurantId == RestaurantId);
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var restaurant = await GetRestaurantAsync();
            if (restaurant == null || restaurant.Settings == null)
            {
                return RedirectToPage("/Admin/Restaurants/Index");
            }

            RestaurantName = restaurant.Name;
            Form = RestaurantSettingsForm.Build(restaurant);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var restaurant = await GetRestaurantAsync();
            if (restaurant == null || restaurant.Settings == null)
            {
                return RedirectToPage("/Admin/Restaurants/Index");
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
