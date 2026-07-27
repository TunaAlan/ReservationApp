using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReservationApp.Services;
using ReservationApp.Models;

namespace ReservationApp.Pages.Admin.Restaurants
{
    [Authorize(Roles = "admin")]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        [BindProperty]
        public RestaurantDto RestaurantDto { get; set; } = new RestaurantDto();

        public int CurrentCapacity { get; set; }
        public int CurrentTableCount { get; set; }
        public int CurrentPhotoCount { get; set; }
        public SelectList CategoryOptions { get; set; } = new SelectList(Enumerable.Empty<Category>(), "CategoryId", "Name");
        public SelectList CityOptions { get; set; } = new SelectList(Enumerable.Empty<City>(), "CityId", "Name");
        public SelectList OwnerOptions { get; set; } = new SelectList(Enumerable.Empty<ApplicationUser>(), "Id", "Email");

        // Dependency Injection Model
        public EditModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        //Error-Success Message
        public string errorMessage = "";
        public string successMessage = "";
        ///////////////////////

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

        private async Task LoadOwnerOptionsAsync()
        {
            var owners = await _userManager.GetUsersInRoleAsync("restaurant");
            OwnerOptions = new SelectList(owners.OrderBy(u => u.Email), "Id", "Email");
        }

        //Fetch The Old Data
        public async Task<IActionResult> OnGetAsync(int id)
        {
            Restaurant? restaurant = await _context.Restaurants.Include(r => r.Tables).Include(r => r.Images).FirstOrDefaultAsync(r => r.RestaurantId == id);

            if (restaurant == null)
            {
                return Redirect("/Admin/Restaurants/Index");
            }

            await LoadCategoryOptionsAsync();
            await LoadCityOptionsAsync();
            await LoadOwnerOptionsAsync();

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
                OwnerUserId = restaurant.OwnerUserId,

            };
            CurrentCapacity = restaurant.Capacity;
            CurrentTableCount = restaurant.Tables.Count;
            CurrentPhotoCount = restaurant.Images.Count;

            return Page();
        }



        public async Task<IActionResult> OnPostAsync(int id)
        {
            await LoadCategoryOptionsAsync();
            await LoadCityOptionsAsync();
            await LoadOwnerOptionsAsync();

            if (!ModelState.IsValid)
            {
                return Page();
            }

            Restaurant? restaurant = await _context.Restaurants.Include(r => r.Tables).Include(r => r.Images).FirstOrDefaultAsync(r => r.RestaurantId == id);

            if (restaurant == null)
            {
                return Redirect("/Admin/Restaurants/Index");
            }

            CurrentCapacity = restaurant.Capacity;
            CurrentTableCount = restaurant.Tables.Count;
            CurrentPhotoCount = restaurant.Images.Count;
            RestaurantDto.RestaurantId = restaurant.RestaurantId;

            var newOwnerUserId = string.IsNullOrEmpty(RestaurantDto.OwnerUserId) ? null : RestaurantDto.OwnerUserId;
            if (newOwnerUserId != null && newOwnerUserId != restaurant.OwnerUserId)
            {
                var ownerTaken = await _context.Restaurants.AnyAsync(r => r.RestaurantId != id && r.OwnerUserId == newOwnerUserId);
                if (ownerTaken)
                {
                    errorMessage = "This owner already manages another restaurant.";
                    return Page();
                }
            }

            restaurant.CategoryId = RestaurantDto.CategoryId;
            restaurant.Name = RestaurantDto.Name;
            restaurant.CityId = RestaurantDto.CityId;
            restaurant.District = RestaurantDto.District;
            restaurant.Address = RestaurantDto.Address;
            restaurant.PhoneNumber = RestaurantDto.PhoneNumber;
            restaurant.AvgPrice = RestaurantDto.AvgPrice;
            restaurant.OwnerUserId = newOwnerUserId;

            int changes = await _context.SaveChangesAsync();

                if (changes > 0)
                {
                    successMessage = "Restaurant successfully updated.";
                }
                else
                {
                    errorMessage = "No changes were made.";
                }

                return Page();
        }
    }
}
