using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ReservationApp.Models;
using ReservationApp.Services;

namespace ReservationApp.Pages.Admin.Categories
{
    [Authorize(Roles = "admin")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public List<Category> Categories { get; set; } = new List<Category>();
        public Dictionary<int, int> RestaurantCountByCategory { get; set; } = new Dictionary<int, int>();

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task OnGetAsync()
        {
            Categories = await _context.Categories.OrderBy(c => c.Name).ToListAsync();

            RestaurantCountByCategory = await _context.Restaurants
                .GroupBy(r => r.CategoryId)
                .Select(g => new { CategoryId = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.CategoryId, x => x.Count);
        }
    }
}
