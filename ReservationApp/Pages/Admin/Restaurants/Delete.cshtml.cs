using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReservationApp.Services;
using ReservationApp.Models;

namespace ReservationApp.Pages.Admin.Restaurants
{
    public class DeleteModel : PageModel
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ApplicationDbContext _context;

        [BindProperty]
        public RestaurantDto RestaurantDto { get; set; } = new RestaurantDto();
        
        // Dependency Injection Model
        public DeleteModel(IWebHostEnvironment environment, ApplicationDbContext context)
        {
            _environment = environment;
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var restaurant = await _context.Restaurants.FindAsync(id);

            if (restaurant == null) 
            {
                return NotFound();
            }

            // Build the full path to the image file
            var imageFullPath = Path.Combine(_environment.WebRootPath, "restaurants", restaurant.ImageFileName);

            // Check if the file exists before attempting to delete
            if (System.IO.File.Exists(imageFullPath))
            {
                try
                {
                    System.IO.File.Delete(imageFullPath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred while deleting the file: {ex.Message}");
                }
            }
            else
            {
                    Console.WriteLine("File not found: " + imageFullPath);
            }

            // Remove 
            _context.Restaurants.Remove(restaurant);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

    }
}