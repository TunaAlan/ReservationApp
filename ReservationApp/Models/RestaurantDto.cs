using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReservationApp.Models
{
    public class RestaurantDto
    {
    [Required, StringLength(50)]
    public string Category { get; set; } = "";

    [Required, StringLength(100)]
    public string Name { get; set; } = "";

    [Required, StringLength(200)]
    public string Address { get; set; } = "";

    [Required, Phone, StringLength(20)]
    public string PhoneNumber { get; set; } = "";

    [Range(1, 100000, ErrorMessage = "Average price must be greater than 0.")]
    public int AvgPrice { get; set; }

    [Range(1, 1000, ErrorMessage = "Capacity must be greater than 0.")]
    public int Capacity { get; set; }
    public IFormFile? ImageFile { get; set; }
    public int RestaurantId { get; set; }

    }
}