using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReservationApp.Models;
using ReservationApp.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;


namespace ReservationApp.Pages.Client.Reservations
{
    [Authorize(Roles = "client")]
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

        [TempData]
        public string? SuccessMessage { get; set; }

        //Defining Classes as a List
        public IList<Reservation>? MyReservations { get; set; }
        public string CustomerName { get; set; } = "";
        public string CustomerEmail { get; set; } = "";

        //Async OnGet Method
        public async Task<IActionResult> OnGetAsync()
        {
        var user = await _userManager.GetUserAsync(User);

        if (user == null)
        {
            return Redirect("/Identity/Account/Login");
        }
                                                           //Check for User Id
        MyReservations = await _context.Reservations
            .Include(r => r.Restaurant).ThenInclude(rest => rest!.Category)
            .Include(r => r.Table)
            .Where(r => r.UserId == user.Id)
            .OrderByDescending(r => r.ReservationDate)
            .ToListAsync();
        CustomerName = $"{user.FirstName} {user.LastName}".Trim();
        CustomerEmail = user.Email ?? "";

        return Page();
        }

    }
}
