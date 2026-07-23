using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ReservationApp.Pages;

public class IndexModel : PageModel
{
    public IActionResult OnGetAdmin()
    {
        if (User?.Identity == null || !User.Identity.IsAuthenticated) 
        {
            return Redirect("/Identity/Account/Login"); 
        }

        if (User.IsInRole("admin"))
        {
            return Redirect("/Admin/Restaurants/Index");
        }

        return Redirect("/");
    }

    public IActionResult OnGetClient()
    {
        if (User?.Identity == null || !User.Identity.IsAuthenticated)
        {
            return Redirect("/Identity/Account/Login");
        }

        if (User.IsInRole("client"))
        {
            return Redirect("/Client/Restaurants/RestaurantList");
        }

        return Redirect("/");
        
    }
}
