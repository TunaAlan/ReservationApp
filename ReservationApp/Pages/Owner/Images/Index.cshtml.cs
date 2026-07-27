using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ReservationApp.Models;
using ReservationApp.Services;

namespace ReservationApp.Pages.Owner.Images
{
    [Authorize(Roles = "restaurant")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly UserManager<ApplicationUser> _userManager;

        public Restaurant? Restaurant { get; set; }
        public List<RestaurantImage> RestaurantImages { get; set; } = new();
        public string errorMessage = "";

        [BindProperty]
        public List<IFormFile> Files { get; set; } = new();

        public IndexModel(ApplicationDbContext context, IWebHostEnvironment environment, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _environment = environment;
            _userManager = userManager;
        }

        private async Task<Restaurant?> GetOwnRestaurantAsync()
        {
            var userId = _userManager.GetUserId(User);
            return await _context.Restaurants.FirstOrDefaultAsync(r => r.OwnerUserId == userId);
        }

        public async Task<IActionResult> OnGetAsync()
        {
            Restaurant = await GetOwnRestaurantAsync();
            if (Restaurant == null)
            {
                return RedirectToPage("/Owner/Index");
            }

            RestaurantImages = await _context.RestaurantImages
                .Where(i => i.RestaurantId == Restaurant.RestaurantId)
                .OrderBy(i => i.DisplayOrder)
                .ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostUploadAsync()
        {
            Restaurant = await GetOwnRestaurantAsync();
            if (Restaurant == null)
            {
                return RedirectToPage("/Owner/Index");
            }

            if (Files.Count == 0)
            {
                errorMessage = "Please choose at least one photo to upload.";
                RestaurantImages = await _context.RestaurantImages.Where(i => i.RestaurantId == Restaurant.RestaurantId).OrderBy(i => i.DisplayOrder).ToListAsync();
                return Page();
            }

            foreach (var file in Files)
            {
                var validationError = ImageUploadHelper.Validate(file);
                if (validationError != null)
                {
                    errorMessage = validationError;
                    RestaurantImages = await _context.RestaurantImages.Where(i => i.RestaurantId == Restaurant.RestaurantId).OrderBy(i => i.DisplayOrder).ToListAsync();
                    return Page();
                }
            }

            var nextOrder = await _context.RestaurantImages
                .Where(i => i.RestaurantId == Restaurant.RestaurantId)
                .Select(i => (int?)i.DisplayOrder)
                .MaxAsync() ?? -1;
            nextOrder++;

            foreach (var file in Files)
            {
                var fileName = await ImageUploadHelper.SaveAsync(file, _environment.WebRootPath);
                _context.RestaurantImages.Add(new RestaurantImage
                {
                    RestaurantId = Restaurant.RestaurantId,
                    FileName = fileName,
                    DisplayOrder = nextOrder++,
                });
            }

            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }

        public async Task<IActionResult> OnPostDeleteAsync(int imageId)
        {
            Restaurant = await GetOwnRestaurantAsync();
            if (Restaurant == null)
            {
                return RedirectToPage("/Owner/Index");
            }

            var image = await _context.RestaurantImages.FirstOrDefaultAsync(i => i.ImageId == imageId && i.RestaurantId == Restaurant.RestaurantId);
            if (image == null)
            {
                return NotFound();
            }

            var fullPath = Path.Combine(_environment.WebRootPath, "Restaurant_Img", image.FileName);
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }

            _context.RestaurantImages.Remove(image);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        public async Task<IActionResult> OnPostMoveAsync(int imageId, string direction)
        {
            Restaurant = await GetOwnRestaurantAsync();
            if (Restaurant == null)
            {
                return RedirectToPage("/Owner/Index");
            }

            var images = await _context.RestaurantImages
                .Where(i => i.RestaurantId == Restaurant.RestaurantId)
                .OrderBy(i => i.DisplayOrder)
                .ToListAsync();

            var index = images.FindIndex(i => i.ImageId == imageId);
            if (index == -1)
            {
                return NotFound();
            }

            var swapWith = direction == "up" ? index - 1 : index + 1;
            if (swapWith >= 0 && swapWith < images.Count)
            {
                (images[index].DisplayOrder, images[swapWith].DisplayOrder) = (images[swapWith].DisplayOrder, images[index].DisplayOrder);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
