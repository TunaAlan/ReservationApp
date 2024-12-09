
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReservationApp.Models;
using ReservationApp.Services;

namespace ReservationApp.Pages.Admin.Restaurants
{
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


        public void OnPost()
        {
            if (!ModelState.IsValid)
            {
                errorMessage = "Please provide all the required fields"; 
                return; 
            }

            if (RestaurantDto.ImageFile == null) 
            {
                ModelState.AddModelError("RestaurantDto.ImageFile", "The image file is required");
            }

            //save the image ??????
            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            newFileName += Path.GetExtension(RestaurantDto.ImageFile!.FileName);

            string imageFullPath = environment.WebRootPath + "/Restaurant_Img/" + newFileName;
            using (var stream = System.IO.File.Create(imageFullPath))
            {
                RestaurantDto.ImageFile.CopyTo(stream);
            }
            ///////////////////////
            
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