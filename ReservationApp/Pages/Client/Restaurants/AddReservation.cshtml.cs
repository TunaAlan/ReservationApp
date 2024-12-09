using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReservationApp.Models;
using ReservationApp.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;


namespace ReservationApp.Pages.Client.Restaurants
{
    public class AddReservationModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        // Dependency Injection Model
        public AddReservationModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        //Defining The Classes
        [BindProperty]
        public Restaurant? Restaurant { get; set; } = new Restaurant();
        [BindProperty]
        public Reservation Reservation { get; set; } = new Reservation();
        public string? LoggedInUserName { get; set; } = "";
        public string? LoggedInUserId { get; set;}

        // Error-Success Message
        public string errorMessage = "";
        public string successMessage = "";
        ////////////////////////
        

        //Async OnGet Method
        public async Task<IActionResult> OnGetAsync(int id)
        {
            Restaurant = await _context.Restaurants.FirstOrDefaultAsync(r => r.RestaurantId == id);

            if (Restaurant == null)
            {
                return RedirectToPage("/Client/ReservationList");
            }

            //Fetch UserName and Id
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                LoggedInUserName = user.UserName;
                LoggedInUserId = user.Id;
            }

            return Page();
            ////////////////
        }

        
        //Async Post Method

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var restaurant = await _context.Restaurants.FindAsync(id);

            if (restaurant == null)
            {
                return RedirectToPage("/NotFound");
            }

            var user = await _userManager.GetUserAsync(User);

            if (user != null)
            {
                Reservation.UserId = user.Id; 
            }
            
            //Arrange the reservation date
            Reservation.RestaurantId = id;
            Reservation.CreatedAt = DateTime.Now;
            ////////////////////////////

            _context.Reservations.Add(Reservation);

            int changes = await _context.SaveChangesAsync();

            if (changes > 0)
            {
                successMessage = "Reservation successfully created.";
                return RedirectToPage("/Client/Reservations/MyReservations"); // Redirect to a list of user reservations or another page
            }
            else
            {
                errorMessage = "Reservation could not be created.";
                return Page();
            }
        }
    }
}
