using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReservationApp.Models;
using ReservationApp.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;


namespace ReservationApp.Pages.Client.Reservations
{
    public class MyReservationsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        // Dependency Injection Model
        public MyReservationsModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        //Defining Classes as a List
        public IList<Reservation>? MyReservations { get; set; }
        public IList<Restaurant>? Restaurants { get; set; } = new List<Restaurant>(); 

        //Async OnGet Method
        public async Task<IActionResult> OnGetAsync()
        {
        var user = await _userManager.GetUserAsync(User);

        if (user == null)
        {
            return RedirectToPage("/Account/Login"); 
        }
                                                           //Check for User Id
        MyReservations = await _context.Reservations.Where(r => r.UserId == user.Id).ToListAsync();
        Restaurants = await _context.Restaurants.ToListAsync();

        return Page();
        }

    }
}
