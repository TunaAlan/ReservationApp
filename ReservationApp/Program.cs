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

// Dev-only admin seeding. Credentials are never hardcoded here — they come from
// user-secrets (local `dotnet run`) or environment variables via .env (Docker),
// so this block behaves identically regardless of where the values originate.
if (app.Environment.IsDevelopment())
{
    var adminEmail = app.Configuration["SeedAdmin:Email"];
    var adminPassword = app.Configuration["SeedAdmin:Password"];

    if (!string.IsNullOrEmpty(adminEmail) && !string.IsNullOrEmpty(adminPassword))
    {
        using var scope = app.Services.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        // Runs on every startup, but the guard below makes it idempotent:
        // the actual INSERT only happens the first time (empty DB).
        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true,
                CreatedAt = DateTime.Now,
            };

            await userManager.CreateAsync(adminUser, adminPassword);
        }

        if (!await userManager.IsInRoleAsync(adminUser, "admin"))
        {
            await userManager.AddToRoleAsync(adminUser, "admin");
        }
    }
}

app.Run();
