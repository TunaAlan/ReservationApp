
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReservationApp.Models;
using ReservationApp.Services;

namespace ReservationApp.Pages.Admin.Restaurants
{
    [Authorize(Roles = "admin")]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> userManager;

        [BindProperty]
        public RestaurantDto RestaurantDto { get; set; } = new RestaurantDto();

        public SelectList CategoryOptions { get; set; } = new SelectList(Enumerable.Empty<Category>(), "CategoryId", "Name");
        public SelectList CityOptions { get; set; } = new SelectList(Enumerable.Empty<City>(), "CityId", "Name");
        public SelectList OwnerOptions { get; set; } = new SelectList(Enumerable.Empty<ApplicationUser>(), "Id", "Email");

        public CreateModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public async Task OnGetAsync()
        {
            await LoadCategoryOptionsAsync();
            await LoadCityOptionsAsync();
            await LoadOwnerOptionsAsync();
        }

        public string errorMessage = "";

        private async Task LoadCategoryOptionsAsync()
        {
            var categories = await context.Categories.OrderBy(c => c.Name).ToListAsync();
            CategoryOptions = new SelectList(categories, "CategoryId", "Name");
        }

        private async Task LoadCityOptionsAsync()
        {
            var cities = await context.Cities.OrderBy(c => c.Name).ToListAsync();
            CityOptions = new SelectList(cities, "CityId", "Name");
        }

        private async Task LoadOwnerOptionsAsync()
        {
            var owners = await userManager.GetUsersInRoleAsync("restaurant");
            OwnerOptions = new SelectList(owners.OrderBy(u => u.Email), "Id", "Email");
        }

        public async Task OnPostAsync()
        {
            await LoadCategoryOptionsAsync();
            await LoadCityOptionsAsync();
            await LoadOwnerOptionsAsync();

            if (!ModelState.IsValid)
            {
                errorMessage = "Please provide all the required fields";
                return;
            }

            if (!string.IsNullOrEmpty(RestaurantDto.OwnerUserId))
            {
                var ownerTaken = await context.Restaurants.AnyAsync(r => r.OwnerUserId == RestaurantDto.OwnerUserId);
                if (ownerTaken)
                {
                    errorMessage = "This owner already manages another restaurant.";
                    return;
                }
            }

            //Saving the Restaurant On the List
            Restaurant restaurant = new Restaurant()
            {
                CategoryId = RestaurantDto.CategoryId,
                Name = RestaurantDto.Name,
                CityId = RestaurantDto.CityId,
                District = RestaurantDto.District,
                Address = RestaurantDto.Address,
                PhoneNumber = RestaurantDto.PhoneNumber,
                AvgPrice = RestaurantDto.AvgPrice,
                OwnerUserId = string.IsNullOrEmpty(RestaurantDto.OwnerUserId) ? null : RestaurantDto.OwnerUserId,
                CreatedAt = DateTime.Now,
                // Every restaurant needs a Settings row and 7 BusinessHours rows for the
                // booking engine to work at all — default to the same values that used
                // to be hardcoded app-wide (90-min turn-time, no buffer, 30-min slots,
                // 11:00-22:00 every day, 6-day advance window). Set as navigation
                // properties rather than a second SaveChanges — EF resolves the
                // RestaurantId FK for both once this graph is saved.
                Settings = new RestaurantSettings(),
                BusinessHours = Enum.GetValues(typeof(DayOfWeek))
                    .Cast<DayOfWeek>()
                    .Select(day => new RestaurantBusinessHour
                    {
                        DayOfWeek = day,
                        OpenTime = new TimeSpan(11, 0, 0),
                        CloseTime = new TimeSpan(22, 0, 0),
                        IsClosed = false,
                    })
                    .ToList(),
            };

            context.Restaurants.Add(restaurant);
            await context.SaveChangesAsync();

            ModelState.Clear();

            // New restaurant has zero tables and zero photos until the admin adds some
            // — send them straight to Tables first (capacity comes before photos).
            Response.Redirect($"/Admin/Restaurants/Tables/Index?restaurantId={restaurant.RestaurantId}");
        }
    }
}
