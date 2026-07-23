using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReservationApp.Services;
using ReservationApp.Models;

namespace ReservationApp.Pages.Admin.Restaurants
{
    [Authorize(Roles = "admin")]
    public class EditModel : PageModel
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ApplicationDbContext _context;

        [BindProperty]
        public RestaurantDto RestaurantDto { get; set; } = new RestaurantDto();
        
        // Dependency Injection Model
        public EditModel(IWebHostEnvironment environment, ApplicationDbContext context)
        {
            _environment = environment;
            _context = context;
        }

        //Error-Success Message
        public string errorMessage = "";
        public string successMessage = "";
        ///////////////////////

        private static readonly string[] AllowedImageExtensions = { ".jpg", ".jpeg", ".png", ".webp" };
        private const long MaxImageSizeBytes = 5 * 1024 * 1024; // 5 MB

        //Fetch The Old Data
        public async Task<IActionResult> OnGetAsync(int id)
        {
            Restaurant? restaurant = await _context.Restaurants.FindAsync(id);

            if (restaurant == null)
            {
                return Redirect("/Admin/Restaurants/Index");
            }

            RestaurantDto = new RestaurantDto
            {
                Category = restaurant.Category,
                Name = restaurant.Name,
                Address = restaurant.Address,
                PhoneNumber = restaurant.PhoneNumber,
                AvgPrice = restaurant.AvgPrice,
                Capacity = restaurant.Capacity,
                
            };
            

            return Page(); 
        }



        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Restaurant? restaurant = await _context.Restaurants.FindAsync(id);

            if (restaurant == null)
            {
                return Redirect("/Admin/Restaurants/Index");
            }
            
            restaurant.Category = RestaurantDto.Category;
            restaurant.Name = RestaurantDto.Name;
            restaurant.Address = RestaurantDto.Address;
            restaurant.PhoneNumber = RestaurantDto.PhoneNumber;
            restaurant.AvgPrice = RestaurantDto.AvgPrice;
            restaurant.Capacity = RestaurantDto.Capacity;

            if (RestaurantDto.ImageFile != null)
            {
                var extension = Path.GetExtension(RestaurantDto.ImageFile.FileName).ToLowerInvariant();
                if (!AllowedImageExtensions.Contains(extension))
                {
                    errorMessage = "Only .jpg, .jpeg, .png, and .webp image files are allowed.";
                    return Page();
                }

                if (RestaurantDto.ImageFile.Length > MaxImageSizeBytes)
                {
                    errorMessage = "Image file must be smaller than 5 MB.";
                    return Page();
                }

                string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + extension;
                string imagePath = Path.Combine(_environment.WebRootPath, "Restaurant_Img", newFileName);

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await RestaurantDto.ImageFile.CopyToAsync(stream);
                }

                restaurant.ImageFileName = newFileName;
            }

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
