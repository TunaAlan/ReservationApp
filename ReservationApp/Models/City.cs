using System.ComponentModel.DataAnnotations;

namespace ReservationApp.Models
{
    public class City
    {
        public int CityId { get; set; }

        [Required, StringLength(50)]
        public string Name { get; set; } = "";

        public ICollection<Restaurant>? Restaurants { get; set; }
    }
}
