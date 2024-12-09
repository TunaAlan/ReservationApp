using Microsoft.AspNetCore.Mvc.RazorPages;
using ReservationApp.Models;
using ReservationApp.Services;
using Microsoft.AspNetCore.Authorization;

namespace ReservationApp.Pages.Client.Restaurants
{
    [Authorize(Roles = "client")]

    public class RestaurantListModel: PageModel
    {
        private readonly ApplicationDbContext context ;
        public List<Restaurant> Restaurants { get; set;} = new List<Restaurant>();

        public RestaurantListModel(ApplicationDbContext context)
        {
            this.context = context;
        }

        public void OnGet()
        {
            Restaurants = context.Restaurants.OrderByDescending(p => p.RestaurantId).ToList();
        }
    } 
}