using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ReservationApp.Models;
using ReservationApp.Services;

namespace ReservationApp.Pages.Admin.Categories
{
    [Authorize(Roles = "admin")]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        [BindProperty]
        public Category Category { get; set; } = new Category();

        public string errorMessage = "";
        public string successMessage = "";

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return Redirect("/Admin/Categories/Index");
            }

            Category = category;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return Redirect("/Admin/Categories/Index");
            }

            var nameTaken = await _context.Categories.AnyAsync(c => c.CategoryId != id && c.Name == Category.Name);
            if (nameTaken)
            {
                errorMessage = "A category with this name already exists.";
                return Page();
            }

            category.Name = Category.Name;
            await _context.SaveChangesAsync();

            successMessage = "Category updated successfully.";
            return Page();
        }
    }
}
