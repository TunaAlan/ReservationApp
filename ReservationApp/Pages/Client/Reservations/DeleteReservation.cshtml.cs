using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
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
        public string? errorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            var userId = _userManager.GetUserId(User);
            Reservation = await _context.Reservations
                .Include(r => r.Restaurant).ThenInclude(rest => rest!.Settings)
                .FirstOrDefaultAsync(r => r.ReservationId == id);

            if (Reservation == null || Reservation.UserId != userId)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var userId = _userManager.GetUserId(User);
            var reservation = await _context.Reservations
                .Include(r => r.Restaurant).ThenInclude(rest => rest!.Settings)
                .FirstOrDefaultAsync(r => r.ReservationId == id);

            if (reservation == null || reservation.UserId != userId)
            {
                return NotFound();
            }

            var settings = reservation.Restaurant?.Settings;
            if (settings != null)
            {
                if (!settings.AllowCancellation)
                {
                    Reservation = reservation;
                    errorMessage = "This restaurant does not allow reservations to be cancelled.";
                    return Page();
                }

                var deadline = reservation.ReservationDate.AddHours(-settings.CancellationDeadlineHours);
                if (settings.CancellationDeadlineHours > 0 && DateTime.Now > deadline)
                {
                    Reservation = reservation;
                    errorMessage = $"Cancellations must be made at least {settings.CancellationDeadlineHours} hour(s) before your reservation time.";
                    return Page();
                }
            }

            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Client/Reservations/MyReservations");
        }
    }
}
