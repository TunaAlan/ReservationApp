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

        // Migration-time seed data: baked into the InitialCreate migration and applied
        // exactly once when the migration runs (tracked via __EFMigrationsHistory).
        // Only for non-sensitive, static demo data — never credentials (see Program.cs
        // for how the admin user is seeded instead, at runtime, from external config).
        var seedDate = new DateTime(2024, 1, 1);

        builder.Entity<Restaurant>().HasData(
            new Restaurant { RestaurantId = 1, Category = "Seafood", Name = "Ocean's Bounty", Address = "12 Shoreline Ave, Seaside", PhoneNumber = "555-9876", AvgPrice = 75, Capacity = 120, ImageFileName = "oceans_bounty.jpg", CreatedAt = seedDate },
            new Restaurant { RestaurantId = 2, Category = "Seafood", Name = "The Fisherman's Wharf", Address = "10 Ocean Drive, Shoreline City", PhoneNumber = "555-1111", AvgPrice = 85, Capacity = 150, ImageFileName = "fishermans_wharf.jpg", CreatedAt = seedDate },
            new Restaurant { RestaurantId = 3, Category = "Fine Dining", Name = "The Golden Fork", Address = "22 High Street, Uptown", PhoneNumber = "555-2345", AvgPrice = 200, Capacity = 80, ImageFileName = "golden_fork.jpg", CreatedAt = seedDate },
            new Restaurant { RestaurantId = 4, Category = "Fine Dining", Name = "Elegance Palace", Address = "33 Luxury Ave, High Hill", PhoneNumber = "555-2222", AvgPrice = 250, Capacity = 70, ImageFileName = "elegance_palace.jpg", CreatedAt = seedDate },
            new Restaurant { RestaurantId = 5, Category = "Fast Food", Name = "QuickBite", Address = "85 Fast Lane, Speedy City", PhoneNumber = "555-7654", AvgPrice = 15, Capacity = 60, ImageFileName = "quick_bite.jpg", CreatedAt = seedDate },
            new Restaurant { RestaurantId = 6, Category = "Fast Food", Name = "Speedy Bites", Address = "99 Quick Rd, Rushville", PhoneNumber = "555-3333", AvgPrice = 18, Capacity = 45, ImageFileName = "speedy_bites.jpg", CreatedAt = seedDate },
            new Restaurant { RestaurantId = 7, Category = "Japanese", Name = "Sakura Sushi", Address = "4 Blossom Rd, Little Tokyo", PhoneNumber = "555-3210", AvgPrice = 40, Capacity = 50, ImageFileName = "sakura_sushi.jpg", CreatedAt = seedDate },
            new Restaurant { RestaurantId = 8, Category = "Japanese", Name = "Tokyo Delight", Address = "25 Sakura St, New Kyoto", PhoneNumber = "555-4444", AvgPrice = 50, Capacity = 60, ImageFileName = "tokyo_delight.jpg", CreatedAt = seedDate },
            new Restaurant { RestaurantId = 9, Category = "Italian", Name = "Mamma Mia", Address = "15 Olive St, Old Town", PhoneNumber = "555-9087", AvgPrice = 60, Capacity = 70, ImageFileName = "mamma_mia.jpg", CreatedAt = seedDate },
            new Restaurant { RestaurantId = 10, Category = "Italian", Name = "Pasta House", Address = "18 Roman Way, Little Italy", PhoneNumber = "555-5555", AvgPrice = 65, Capacity = 80, ImageFileName = "pasta_house.jpg", CreatedAt = seedDate },
            new Restaurant { RestaurantId = 11, Category = "Cafe", Name = "Brewed Awakening", Address = "9 Coffee Blvd, Downtown", PhoneNumber = "555-4532", AvgPrice = 12, Capacity = 40, ImageFileName = "brewed_awakening.jpg", CreatedAt = seedDate },
            new Restaurant { RestaurantId = 12, Category = "Cafe", Name = "Morning Brew", Address = "44 Bean St, Coffeeville", PhoneNumber = "555-6666", AvgPrice = 15, Capacity = 35, ImageFileName = "morning_brew.jpg", CreatedAt = seedDate },
            new Restaurant { RestaurantId = 13, Category = "Steakhouse", Name = "Grill Master", Address = "34 Beef Rd, Meat District", PhoneNumber = "555-8723", AvgPrice = 90, Capacity = 90, ImageFileName = "grill_master.jpg", CreatedAt = seedDate },
            new Restaurant { RestaurantId = 14, Category = "Steakhouse", Name = "Prime Cut Grill", Address = "52 Steakhouse Rd, Beef City", PhoneNumber = "555-7777", AvgPrice = 100, Capacity = 85, ImageFileName = "prime_cut_grill.jpg", CreatedAt = seedDate },
            new Restaurant { RestaurantId = 15, Category = "Bistro", Name = "Le Petit Bistro", Address = "11 Cozy Corner, Riverside", PhoneNumber = "555-6789", AvgPrice = 45, Capacity = 30, ImageFileName = "le_petit_bistro.jpg", CreatedAt = seedDate },
            new Restaurant { RestaurantId = 16, Category = "Bistro", Name = "Bistro Bella", Address = "3 Cozy Ln, Riverside", PhoneNumber = "555-8888", AvgPrice = 40, Capacity = 25, ImageFileName = "bistro_bella.jpg", CreatedAt = seedDate }
        );

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