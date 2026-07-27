using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ReservationApp.Models;
using ReservationApp.Services;

namespace ReservationApp.Pages.Admin.Categories
{
    [Authorize(Roles = "admin")]
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public Category? Category { get; set; }
        public int RestaurantsUsingIt { get; set; }
        public List<Restaurant> RestaurantsUsing { get; set; } = new();
        public string errorMessage = "";

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET only shows a confirmation screen — it must never mutate data.
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Category = await _context.Categories.FindAsync(id);
            if (Category == null)
            {
                return NotFound();
            }

            RestaurantsUsing = await _context.Restaurants.Where(r => r.CategoryId == id).OrderBy(r => r.Name).ToListAsync();
            RestaurantsUsingIt = RestaurantsUsing.Count;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            var restaurantsUsing = await _context.Restaurants.Where(r => r.CategoryId == id).OrderBy(r => r.Name).ToListAsync();
            if (restaurantsUsing.Count > 0)
            {
                Category = category;
                RestaurantsUsing = restaurantsUsing;
                RestaurantsUsingIt = restaurantsUsing.Count;
                errorMessage = $"This category is used by {restaurantsUsing.Count} restaurant(s) and cannot be deleted.";
                return Page();
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
