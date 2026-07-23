using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReservationApp.Models;
using ReservationApp.Services;
namespace ReservationApp.Pages.Client.Reservations
{
    [Authorize]
    public class DeleteReservationModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DeleteReservationModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public Reservation? Reservation { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            var userId = _userManager.GetUserId(User);
            Reservation = await _context.Reservations.FindAsync(id);

            if (Reservation == null || Reservation.UserId != userId)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var userId = _userManager.GetUserId(User);
            var reservation = await _context.Reservations.FindAsync(id);

            if (reservation == null || reservation.UserId != userId)
            {
                return NotFound();
            }

            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Client/Reservations/MyReservations");
        }
    }
}
