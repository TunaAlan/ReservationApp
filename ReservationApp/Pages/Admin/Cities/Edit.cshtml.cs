using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ReservationApp.Models;
using ReservationApp.Services;

namespace ReservationApp.Pages.Admin.Cities
{
    [Authorize(Roles = "admin")]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        [BindProperty]
        public City City { get; set; } = new City();

        public string errorMessage = "";
        public string successMessage = "";

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var city = await _context.Cities.FindAsync(id);

            if (city == null)
            {
                return Redirect("/Admin/Cities/Index");
            }

            City = city;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var city = await _context.Cities.FindAsync(id);
            if (city == null)
            {
                return Redirect("/Admin/Cities/Index");
            }

            var nameTaken = await _context.Cities.AnyAsync(c => c.CityId != id && c.Name == City.Name);
            if (nameTaken)
            {
                errorMessage = "A city with this name already exists.";
                return Page();
            }

            city.Name = City.Name;
            await _context.SaveChangesAsync();

            successMessage = "City updated successfully.";
            return Page();
        }
    }
}
