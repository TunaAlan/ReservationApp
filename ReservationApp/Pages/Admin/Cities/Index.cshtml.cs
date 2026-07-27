using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ReservationApp.Models;
using ReservationApp.Services;

namespace ReservationApp.Pages.Admin.Cities
{
    [Authorize(Roles = "admin")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public List<City> Cities { get; set; } = new List<City>();
        public Dictionary<int, int> RestaurantCountByCity { get; set; } = new Dictionary<int, int>();

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task OnGetAsync()
        {
            Cities = await _context.Cities.OrderBy(c => c.Name).ToListAsync();

            RestaurantCountByCity = await _context.Restaurants
                .GroupBy(r => r.CityId)
                .Select(g => new { CityId = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.CityId, x => x.Count);
        }
    }
}
