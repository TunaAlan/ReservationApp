using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReservationApp.Services;
using ReservationApp.Models;

namespace ReservationApp.Pages.Admin.Restaurants
{
    [Authorize(Roles = "admin")]
    public class DeleteModel : PageModel
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ApplicationDbContext _context;

        public Restaurant? Restaurant { get; set; }

        // Dependency Injection Model
        public DeleteModel(IWebHostEnvironment environment, ApplicationDbContext context)
        {
            _environment = environment;
            _context = context;
        }

        // GET only shows a confirmation screen — it must never mutate data.
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Restaurant = await _context.Restaurants.FindAsync(id);

            if (Restaurant == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
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

            var imageFullPath = Path.Combine(_environment.WebRootPath, "Restaurant_Img", restaurant.ImageFileName);

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

            _context.Restaurants.Remove(restaurant);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}