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

        //User Id
        public string? UserId { get; set; }

        //User Object(Fetch)
        public ApplicationUser? User { get; set; }

        [Required]
        public DateTime ReservationDate { get; set; }

        [Range(1, 20, ErrorMessage = "Number of people must be between 1 and 20.")]
        public int NumberOfPeople { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

    }
}