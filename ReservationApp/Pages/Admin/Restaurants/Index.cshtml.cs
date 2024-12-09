using Microsoft.AspNetCore.Mvc.RazorPages;
using ReservationApp.Models;
using ReservationApp.Services;
using Microsoft.AspNetCore.Authorization;

namespace ReservationApp.Pages.Admin.Restaurants
{
    [Authorize(Roles = "admin")]

    public class IndexModel: PageModel
    {
        private readonly ApplicationDbContext context ;
        public List<Restaurant> Restaurants { get; set;} = new List<Restaurant>();

        public IndexModel(ApplicationDbContext context)
        {
            this.context = context;
        }

        public void OnGet()
        {
            Restaurants = context.Restaurants.OrderByDescending(p => p.RestaurantId).ToList();
        }
    } 
}