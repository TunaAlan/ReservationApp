//add-delete-update migrations

//dotnet ef migrations remove
//dotnet ef database update
//dotnet ef migrations add FirstMigration

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using ReservationApp.Models;
using Microsoft.AspNetCore.Identity;

namespace ReservationApp.Services;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {

    }
    
    public DbSet<Restaurant> Restaurants { get; set; }

    public DbSet<Reservation> Reservations { get; set; }

    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        var admin = new IdentityRole("admin");
        admin.NormalizedName = "admin";

        var client = new IdentityRole("client");
        client.NormalizedName = "client";

        builder.Entity<IdentityRole>().HasData(admin, client);

        //Fluent API ???????????
        builder.Entity<Reservation>()
                .HasOne(r => r.Restaurant)
                .WithMany(r => r.Reservation)  
                .HasForeignKey(r => r.RestaurantId)
                .OnDelete(DeleteBehavior.Cascade); 

            builder.Entity<Reservation>()
                .HasOne(r => r.User)
                .WithMany()  
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict); 
        //////////////////////////
    }

}