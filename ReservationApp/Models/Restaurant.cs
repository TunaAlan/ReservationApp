using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;

namespace ReservationApp.Models
{
    public class Restaurant
    {
    public int RestaurantId { get; set; }

    [Required]
    public int CategoryId { get; set; }
    public Category? Category { get; set; }

    [Required, StringLength(100)]
    public string Name { get; set; } = "";

    [Required]
    public int CityId { get; set; }
    public City? City { get; set; }

    [Required, StringLength(80)]
    public string District { get; set; } = "";

    [Required, StringLength(200)]
    public string Address { get; set; } = "";

    [Required, Phone, StringLength(20)]
    public string PhoneNumber { get; set; } = "";

    [Range(1, 100000, ErrorMessage = "Average price must be greater than 0.")]
    public int AvgPrice { get; set; }

    // Total seats, derived from the restaurant's own tables rather than entered
    // by hand — keeps the number consistent with what's actually bookable.
    // Requires Tables to be eager-loaded (Include) or this always evaluates to 0.
    [NotMapped]
    public int Capacity => Tables?.Sum(t => t.SeatCount) ?? 0;

    // The restaurant's "cover" photo is just the first image in its gallery (by
    // DisplayOrder) — no separate field to keep in sync. Requires Images to be
    // eager-loaded (Include) or this always evaluates to null.
    [NotMapped]
    public string? CoverImageFileName => Images?.OrderBy(i => i.DisplayOrder).FirstOrDefault()?.FileName;

    public DateTime CreatedAt { get; set; }

    // The restaurant-role user who manages this listing (tables, profile). Null
    // until an admin assigns one — set-null on delete so removing the user account
    // never cascades into deleting the restaurant itself.
    public string? OwnerUserId { get; set; }
    public ApplicationUser? OwnerUser { get; set; }

    public ICollection<Reservation>? Reservation { get; set; }
    public ICollection<RestaurantTable> Tables { get; set; } = new List<RestaurantTable>();
    public ICollection<RestaurantImage> Images { get; set; } = new List<RestaurantImage>();

    public RestaurantSettings? Settings { get; set; }
    public ICollection<RestaurantBusinessHour> BusinessHours { get; set; } = new List<RestaurantBusinessHour>();

    }
}