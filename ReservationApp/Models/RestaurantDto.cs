using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReservationApp.Models
{
    public class RestaurantDto
    {
    public string Category { get; set; } = ""; 
    public string Name { get; set; } = ""; 
    public string Address { get; set; } = ""; 
    public string PhoneNumber { get; set; } = ""; 
    public int AvgPrice { get; set; }
    public int Capacity { get; set; } 
    public IFormFile? ImageFile { get; set; }
    public int RestaurantId { get; set; } 
    
    }
}