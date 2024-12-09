using System.Threading.Tasks;

namespace ReservationApp.Models
{
    public class Restaurant
    {
    public int RestaurantId { get; set; } 
    public string Category { get; set; } = ""; 
    public string Name { get; set; } = ""; 
    public string Address { get; set; } = ""; 
    public string PhoneNumber { get; set; } = ""; 
    public int AvgPrice { get; set; }
    public int Capacity { get; set; } 
    public string ImageFileName { get; set; } = "";
    public DateTime CreatedAt { get; set; } 

    public ICollection<Reservation>? Reservation { get; set; }

    }
}