using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReservationApp.Models;
using ReservationApp.Services;

namespace ReservationApp.Pages.Owner
{
    [Authorize(Roles = "restaurant")]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        [BindProperty]
        public RestaurantDto RestaurantDto { get; set; } = new RestaurantDto();

        public int CurrentPhotoCount { get; set; }
        public SelectList CategoryOptions { get; set; } = new SelectList(Enumerable.Empty<Category>(), "CategoryId", "Name");
        public SelectList CityOptions { get; set; } = new SelectList(Enumerable.Empty<City>(), "CityId", "Name");

        public string errorMessage = "";
        public string successMessage = "";

        public EditModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        private async Task LoadCategoryOptionsAsync()
        {
            var categories = await _context.Categories.OrderBy(c => c.Name).ToListAsync();
            CategoryOptions = new SelectList(categories, "CategoryId", "Name");
        }

        private async Task LoadCityOptionsAsync()
        {
            var cities = await _context.Cities.OrderBy(c => c.Name).ToListAsync();
            CityOptions = new SelectList(cities, "CityId", "Name");
        }

        // Every handler resolves the restaurant from the logged-in owner's Id rather
        // than trusting a route/query value — an owner can never act on another
        // restaurant's data, regardless of what's sent in the request.
        private async Task<Restaurant?> GetOwnRestaurantAsync()
        {
            var userId = _userManager.GetUserId(User);
            return await _context.Restaurants.Include(r => r.Images).FirstOrDefaultAsync(r => r.OwnerUserId == userId);
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var restaurant = await GetOwnRestaurantAsync();
            if (restaurant == null)
            {
                return RedirectToPage("./Index");
            }

            await LoadCategoryOptionsAsync();
            await LoadCityOptionsAsync();

            RestaurantDto = new RestaurantDto
            {
                RestaurantId = restaurant.RestaurantId,
                CategoryId = restaurant.CategoryId,
                Name = restaurant.Name,
                CityId = restaurant.CityId,
                District = restaurant.District,
                Address = restaurant.Address,
                PhoneNumber = restaurant.PhoneNumber,
                AvgPrice = restaurant.AvgPrice,
            };
            CurrentPhotoCount = restaurant.Images.Count;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await LoadCategoryOptionsAsync();
            await LoadCityOptionsAsync();

            var restaurant = await GetOwnRestaurantAsync();
            if (restaurant == null)
            {
                return RedirectToPage("./Index");
            }

            CurrentPhotoCount = restaurant.Images.Count;
            RestaurantDto.RestaurantId = restaurant.RestaurantId;

            if (!ModelState.IsValid)
            {
                return Page();
            }

            restaurant.CategoryId = RestaurantDto.CategoryId;
            restaurant.Name = RestaurantDto.Name;
            restaurant.CityId = RestaurantDto.CityId;
            restaurant.District = RestaurantDto.District;
            restaurant.Address = RestaurantDto.Address;
            restaurant.PhoneNumber = RestaurantDto.PhoneNumber;
            restaurant.AvgPrice = RestaurantDto.AvgPrice;

            await _context.SaveChangesAsync();
            successMessage = "Restaurant profile updated.";
            return Page();
        }
    }
}
