using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ReservationApp.Services;
using ReservationApp.Models;

namespace ReservationApp.Pages.Admin.Restaurants
{
    [Authorize(Roles = "admin")]
    public class DeleteModel : PageModel
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ApplicationDbContext _context;

        public Restaurant? Restaurant { get; set; }
        public int ActiveReservationsUsingIt { get; set; }
        public List<Reservation> ActiveReservations { get; set; } = new();
        public string errorMessage = "";

        // Dependency Injection Model
        public DeleteModel(IWebHostEnvironment environment, ApplicationDbContext context)
        {
            _environment = environment;
            _context = context;
        }

        // GET only shows a confirmation screen — it must never mutate data.
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Restaurant = await _context.Restaurants.Include(r => r.Category).Include(r => r.Tables).FirstOrDefaultAsync(r => r.RestaurantId == id);

            if (Restaurant == null)
            {
                return NotFound();
            }

            // Captured to a variable: an inline DateTime.Now in the query gets translated
            // by EF Core into SQL Server's GETDATE(), which runs on the DB container's
            // own clock/timezone rather than the app's.
            var now = DateTime.Now;
            ActiveReservations = await _context.Reservations
                .Include(r => r.User)
                .Include(r => r.Table)
                .Where(r => r.RestaurantId == id && r.ReservationDate >= now)
                .OrderBy(r => r.ReservationDate)
                .ToListAsync();
            ActiveReservationsUsingIt = ActiveReservations.Count;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var restaurant = await _context.Restaurants.Include(r => r.Category).Include(r => r.Tables).Include(r => r.Images).FirstOrDefaultAsync(r => r.RestaurantId == id);

            if (restaurant == null)
            {
                return NotFound();
            }

            // Deleting a restaurant cascades to its tables and reservations — block it
            // while a customer still has an upcoming booking, same protection Category
            // delete already has for restaurants using it.
            var now = DateTime.Now;
            var activeReservations = await _context.Reservations
                .Include(r => r.User)
                .Include(r => r.Table)
                .Where(r => r.RestaurantId == id && r.ReservationDate >= now)
                .OrderBy(r => r.ReservationDate)
                .ToListAsync();
            if (activeReservations.Count > 0)
            {
                Restaurant = restaurant;
                ActiveReservations = activeReservations;
                ActiveReservationsUsingIt = activeReservations.Count;
                errorMessage = $"This restaurant has {activeReservations.Count} upcoming reservation(s) and cannot be deleted.";
                return Page();
            }

            foreach (var image in restaurant.Images)
            {
                var imageFullPath = Path.Combine(_environment.WebRootPath, "Restaurant_Img", image.FileName);
                if (System.IO.File.Exists(imageFullPath))
                {
                    try
                    {
                        System.IO.File.Delete(imageFullPath);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"An error occurred while deleting the file: {ex.Message}");
                    }
                }
            }

            _context.Restaurants.Remove(restaurant);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}