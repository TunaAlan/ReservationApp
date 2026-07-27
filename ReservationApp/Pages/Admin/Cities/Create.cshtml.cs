using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ReservationApp.Models;
using ReservationApp.Services;

namespace ReservationApp.Pages.Admin.Cities
{
    [Authorize(Roles = "admin")]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        [BindProperty]
        public City City { get; set; } = new City();

        public string errorMessage = "";

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var exists = await _context.Cities.AnyAsync(c => c.Name == City.Name);
            if (exists)
            {
                errorMessage = "A city with this name already exists.";
                return Page();
            }

            _context.Cities.Add(City);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
