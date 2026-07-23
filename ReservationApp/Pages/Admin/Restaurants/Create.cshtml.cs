
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReservationApp.Models;
using ReservationApp.Services;

namespace ReservationApp.Pages.Admin.Restaurants
{
    [Authorize(Roles = "admin")]
    public class CreateModel : PageModel
    {
        private readonly IWebHostEnvironment environment;
        private readonly ApplicationDbContext context;

        [BindProperty]
        public RestaurantDto RestaurantDto { get; set; } = new RestaurantDto();

        public CreateModel(IWebHostEnvironment environment, ApplicationDbContext context)
        {
            this.environment = environment;
            this.context = context;
        }
        public void OnGet()
        {
        }

        public string errorMessage = "";
        public string successMessage = "";

        private static readonly string[] AllowedImageExtensions = { ".jpg", ".jpeg", ".png", ".webp" };
        private const long MaxImageSizeBytes = 5 * 1024 * 1024; // 5 MB

        public void OnPost()
        {
            if (!ModelState.IsValid)
            {
                errorMessage = "Please provide all the required fields";
                return;
            }

            if (RestaurantDto.ImageFile == null)
            {
                errorMessage = "The image file is required.";
                return;
            }

            var extension = Path.GetExtension(RestaurantDto.ImageFile.FileName).ToLowerInvariant();
            if (!AllowedImageExtensions.Contains(extension))
            {
                errorMessage = "Only .jpg, .jpeg, .png, and .webp image files are allowed.";
                return;
            }

            if (RestaurantDto.ImageFile.Length > MaxImageSizeBytes)
            {
                errorMessage = "Image file must be smaller than 5 MB.";
                return;
            }

            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + extension;

            string imageFullPath = environment.WebRootPath + "/Restaurant_Img/" + newFileName;
            using (var stream = System.IO.File.Create(imageFullPath))
            {
                RestaurantDto.ImageFile.CopyTo(stream);
            }
            
            //Saving the Restaurant On the List
            Restaurant restaurant = new Restaurant()
            {
                Category = RestaurantDto.Category,
                Name = RestaurantDto.Name,
                Address = RestaurantDto.Address,
                PhoneNumber = RestaurantDto.PhoneNumber,
                AvgPrice = RestaurantDto.AvgPrice,
                Capacity =  RestaurantDto.Capacity,
                ImageFileName = newFileName,
                CreatedAt = DateTime.Now,
            };

            context.Restaurants.Add(restaurant);
            context.SaveChanges();

            ModelState.Clear();
            
            successMessage = "Restaurant Has Been Saved On the List Successfully !";
            Response.Redirect("/Admin/Restaurants/Index");
        }
    }
}