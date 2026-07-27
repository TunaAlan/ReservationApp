using Microsoft.EntityFrameworkCore;
using ReservationApp.Services;
using ReservationApp.Models;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlServer(connectionString);
});


//ApplicationUser and Role-Based Authorization

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();



var app = builder.Build();




// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication(); 
app.UseAuthorization();

app.MapRazorPages();

// Dev-only account seeding. Credentials are never hardcoded here — they come from
// user-secrets (local `dotnet run`) or environment variables via .env (Docker),
// so this behaves identically regardless of where the values originate.
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    await SeedUserAsync(userManager, app.Configuration, "SeedAdmin", "admin");
    await SeedUserAsync(userManager, app.Configuration, "SeedClient", "client");
    await SeedUserAsync(userManager, app.Configuration, "SeedRestaurantOwner", "restaurant");
}

app.Run();

// Runs on every startup, but the guard below makes it idempotent:
// the actual INSERT only happens the first time (empty DB).
static async Task SeedUserAsync(UserManager<ApplicationUser> userManager, IConfiguration configuration, string configSection, string role)
{
    var email = configuration[$"{configSection}:Email"];
    var password = configuration[$"{configSection}:Password"];

    if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
    {
        return;
    }

    var user = await userManager.FindByEmailAsync(email);
    if (user == null)
    {
        user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            EmailConfirmed = true,
            CreatedAt = DateTime.Now,
        };

        await userManager.CreateAsync(user, password);
    }

    if (!await userManager.IsInRoleAsync(user, role))
    {
        await userManager.AddToRoleAsync(user, role);
    }
}
