using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReservationApp.Models
{
    public class RestaurantDto
    {
    [Range(1, int.MaxValue, ErrorMessage = "Please select a category.")]
    public int CategoryId { get; set; }

    [Required, StringLength(100)]
    public string Name { get; set; } = "";

    [Range(1, int.MaxValue, ErrorMessage = "Please select a city.")]
    public int CityId { get; set; }

    [Required, StringLength(80)]
    public string District { get; set; } = "";

    [Required, StringLength(200)]
    public string Address { get; set; } = "";

    [Required, Phone, StringLength(20)]
    public string PhoneNumber { get; set; } = "";

    [Range(1, 100000, ErrorMessage = "Average price must be greater than 0.")]
    public int AvgPrice { get; set; }

    public int RestaurantId { get; set; }

    // Admin-only field: which restaurant-role user manages this listing.
    // Never bound/shown on the owner-facing edit form.
    public string? OwnerUserId { get; set; }

    }
}