using System.ComponentModel.DataAnnotations;

namespace ReservationApp.Models
{
    // Extra gallery photos (interior, exterior, seating, etc.) shown alongside the
    // restaurant's main cover image (Restaurant.ImageFileName) in detail views.
    public class RestaurantImage
    {
        public int ImageId { get; set; }

        [Required]
        public int RestaurantId { get; set; }
        public Restaurant? Restaurant { get; set; }

        [Required, StringLength(255)]
        public string FileName { get; set; } = "";

        // Lower sorts first — lets an admin/owner reorder the gallery.
        public int DisplayOrder { get; set; }
    }
}
