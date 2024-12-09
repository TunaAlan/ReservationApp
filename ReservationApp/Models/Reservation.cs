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

        public DateTime ReservationDate { get; set; } 
        public int NumberOfPeople { get; set; }  
        public DateTime CreatedAt { get; set; } = DateTime.Now; 
        
    }
}