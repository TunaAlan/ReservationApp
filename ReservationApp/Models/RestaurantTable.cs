using System.ComponentModel.DataAnnotations;

namespace ReservationApp.Models
{
    public class RestaurantTable
    {
        public int TableId { get; set; }

        [Required]
        public int RestaurantId { get; set; }
        public Restaurant? Restaurant { get; set; }

        [Required, StringLength(20)]
        public string Label { get; set; } = "";

        [Range(1, 20, ErrorMessage = "Seat count must be between 1 and 20.")]
        public int SeatCount { get; set; }
    }
}
