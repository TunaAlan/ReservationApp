using Microsoft.AspNetCore.Mvc.Rendering;

namespace ReservationApp.Models
{
    // Shared by the Admin and Client restaurant list pages' search/category/city
    // filter bar (_RestaurantFilterBar partial) — both pages bind these same
    // query-string values on their own PageModel and pass this down just
    // to render the bar with the right action URL and pre-selected values.
    public class RestaurantFilter
    {
        public string ActionUrl { get; set; } = "";
        public string? Search { get; set; }
        public int? CategoryId { get; set; }
        public int? CityId { get; set; }
        public SelectList CategoryOptions { get; set; } = new SelectList(Enumerable.Empty<Category>(), "CategoryId", "Name");
        public SelectList CityOptions { get; set; } = new SelectList(Enumerable.Empty<City>(), "CityId", "Name");
    }
}
