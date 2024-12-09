using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReservationApp.Models;
using ReservationApp.Services;
namespace ReservationApp.Pages.Client.Reservations
{
    public class DeleteReservationModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteReservationModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Reservation? Reservation { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            Reservation = await _context.Reservations.FindAsync(id);

            if (Reservation == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);

            if (reservation == null)
            {
                return NotFound();
            }

            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Client/Reservations/MyReservations");
        }
    }
}
