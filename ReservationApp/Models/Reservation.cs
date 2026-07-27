using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace ReservationApp.Models
{
    public class Reservation
    {
        public int ReservationId { get; set; }
        public int RestaurantId { get; set; }

        //Restaurant Object(Fetch)
        public Restaurant? Restaurant { get; set; }

        // The specific table assigned at booking time (best-fit match for party size,
        // free for the whole turn-time window). Null only for legacy rows predating
        // table-based capacity.
        public int? TableId { get; set; }
        public RestaurantTable? Table { get; set; }

        //User Id
        public string? UserId { get; set; }

        //User Object(Fetch)
        public ApplicationUser? User { get; set; }

        [Required]
        public DateTime ReservationDate { get; set; }

        // Upper bound is a sanity check only (matches Restaurant.Capacity's own max) —
        // the real per-restaurant limit is enforced by the capacity check in AddReservationModel.
        [Range(1, 1000, ErrorMessage = "Number of people must be greater than 0.")]
        public int NumberOfPeople { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [MaxLength(500)]
        public string? Notes { get; set; }

        public ReservationStatus Status { get; set; } = ReservationStatus.Confirmed;

    }
}