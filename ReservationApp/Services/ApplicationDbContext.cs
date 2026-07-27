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

    public DbSet<Category> Categories { get; set; }

    public DbSet<City> Cities { get; set; }

    public DbSet<RestaurantTable> RestaurantTables { get; set; }

    public DbSet<RestaurantImage> RestaurantImages { get; set; }

    public DbSet<RestaurantSettings> RestaurantSettings { get; set; }

    public DbSet<RestaurantBusinessHour> RestaurantBusinessHours { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        // Fixed Ids (not `new IdentityRole("admin")`, which mints a random Guid on every
        // build): otherwise every future migration diffs the whole role set and
        // delete+reinserts these rows, orphaning any AspNetUserRoles pointing at them.
        var admin = new IdentityRole("admin") { Id = "c0c0c23b-1ceb-4bb2-b0b3-e1212082eba9", NormalizedName = "admin" };
        var client = new IdentityRole("client") { Id = "f6d83ba9-e9dc-4202-9976-f4ea848a33fb", NormalizedName = "client" };
        var restaurant = new IdentityRole("restaurant") { Id = "a1b2c3d4-5e6f-4a7b-8c9d-0e1f2a3b4c5d", NormalizedName = "restaurant" };

        builder.Entity<IdentityRole>().HasData(admin, client, restaurant);

        // Migration-time seed data: baked into the InitialCreate migration and applied
        // exactly once when the migration runs (tracked via __EFMigrationsHistory).
        // Only for non-sensitive, static demo data — never credentials (see Program.cs
        // for how the admin user is seeded instead, at runtime, from external config).
        var seedDate = new DateTime(2024, 1, 1);

        builder.Entity<Category>().HasData(
            new Category { CategoryId = 1, Name = "Seafood" },
            new Category { CategoryId = 2, Name = "Fine Dining" },
            new Category { CategoryId = 3, Name = "Fast Food" },
            new Category { CategoryId = 4, Name = "Japanese" },
            new Category { CategoryId = 5, Name = "Italian" },
            new Category { CategoryId = 6, Name = "Cafe" },
            new Category { CategoryId = 7, Name = "Steakhouse" },
            new Category { CategoryId = 8, Name = "Bistro" }
        );

        // Turkey's 81 provinces (İl Trafik Kodu order, 1–81) — a real, fixed reference
        // list rather than fictional demo names, so city filtering means something.
        // Admin/Cities still has full CRUD, but the list is already complete.
        builder.Entity<City>().HasData(
            new City { CityId = 1, Name = "Adana" },
            new City { CityId = 2, Name = "Adıyaman" },
            new City { CityId = 3, Name = "Afyonkarahisar" },
            new City { CityId = 4, Name = "Ağrı" },
            new City { CityId = 5, Name = "Amasya" },
            new City { CityId = 6, Name = "Ankara" },
            new City { CityId = 7, Name = "Antalya" },
            new City { CityId = 8, Name = "Artvin" },
            new City { CityId = 9, Name = "Aydın" },
            new City { CityId = 10, Name = "Balıkesir" },
            new City { CityId = 11, Name = "Bilecik" },
            new City { CityId = 12, Name = "Bingöl" },
            new City { CityId = 13, Name = "Bitlis" },
            new City { CityId = 14, Name = "Bolu" },
            new City { CityId = 15, Name = "Burdur" },
            new City { CityId = 16, Name = "Bursa" },
            new City { CityId = 17, Name = "Çanakkale" },
            new City { CityId = 18, Name = "Çankırı" },
            new City { CityId = 19, Name = "Çorum" },
            new City { CityId = 20, Name = "Denizli" },
            new City { CityId = 21, Name = "Diyarbakır" },
            new City { CityId = 22, Name = "Edirne" },
            new City { CityId = 23, Name = "Elazığ" },
            new City { CityId = 24, Name = "Erzincan" },
            new City { CityId = 25, Name = "Erzurum" },
            new City { CityId = 26, Name = "Eskişehir" },
            new City { CityId = 27, Name = "Gaziantep" },
            new City { CityId = 28, Name = "Giresun" },
            new City { CityId = 29, Name = "Gümüşhane" },
            new City { CityId = 30, Name = "Hakkari" },
            new City { CityId = 31, Name = "Hatay" },
            new City { CityId = 32, Name = "Isparta" },
            new City { CityId = 33, Name = "Mersin" },
            new City { CityId = 34, Name = "İstanbul" },
            new City { CityId = 35, Name = "İzmir" },
            new City { CityId = 36, Name = "Kars" },
            new City { CityId = 37, Name = "Kastamonu" },
            new City { CityId = 38, Name = "Kayseri" },
            new City { CityId = 39, Name = "Kırklareli" },
            new City { CityId = 40, Name = "Kırşehir" },
            new City { CityId = 41, Name = "Kocaeli" },
            new City { CityId = 42, Name = "Konya" },
            new City { CityId = 43, Name = "Kütahya" },
            new City { CityId = 44, Name = "Malatya" },
            new City { CityId = 45, Name = "Manisa" },
            new City { CityId = 46, Name = "Kahramanmaraş" },
            new City { CityId = 47, Name = "Mardin" },
            new City { CityId = 48, Name = "Muğla" },
            new City { CityId = 49, Name = "Muş" },
            new City { CityId = 50, Name = "Nevşehir" },
            new City { CityId = 51, Name = "Niğde" },
            new City { CityId = 52, Name = "Ordu" },
            new City { CityId = 53, Name = "Rize" },
            new City { CityId = 54, Name = "Sakarya" },
            new City { CityId = 55, Name = "Samsun" },
            new City { CityId = 56, Name = "Siirt" },
            new City { CityId = 57, Name = "Sinop" },
            new City { CityId = 58, Name = "Sivas" },
            new City { CityId = 59, Name = "Tekirdağ" },
            new City { CityId = 60, Name = "Tokat" },
            new City { CityId = 61, Name = "Trabzon" },
            new City { CityId = 62, Name = "Tunceli" },
            new City { CityId = 63, Name = "Şanlıurfa" },
            new City { CityId = 64, Name = "Uşak" },
            new City { CityId = 65, Name = "Van" },
            new City { CityId = 66, Name = "Yozgat" },
            new City { CityId = 67, Name = "Zonguldak" },
            new City { CityId = 68, Name = "Aksaray" },
            new City { CityId = 69, Name = "Bayburt" },
            new City { CityId = 70, Name = "Karaman" },
            new City { CityId = 71, Name = "Kırıkkale" },
            new City { CityId = 72, Name = "Batman" },
            new City { CityId = 73, Name = "Şırnak" },
            new City { CityId = 74, Name = "Bartın" },
            new City { CityId = 75, Name = "Ardahan" },
            new City { CityId = 76, Name = "Iğdır" },
            new City { CityId = 77, Name = "Yalova" },
            new City { CityId = 78, Name = "Karabük" },
            new City { CityId = 79, Name = "Kilis" },
            new City { CityId = 80, Name = "Osmaniye" },
            new City { CityId = 81, Name = "Düzce" }
        );

        builder.Entity<Restaurant>().HasData(
            new Restaurant { RestaurantId = 1, CategoryId = 1, CityId = 48, District = "Bodrum", Name = "Ocean's Bounty", Address = "Neyzen Tevfik Cd. No:12", PhoneNumber = "555-9876", AvgPrice = 75, CreatedAt = seedDate },
            new Restaurant { RestaurantId = 2, CategoryId = 1, CityId = 35, District = "Karşıyaka", Name = "The Fisherman's Wharf", Address = "Girne Blv. No:10", PhoneNumber = "555-1111", AvgPrice = 85, CreatedAt = seedDate },
            new Restaurant { RestaurantId = 3, CategoryId = 2, CityId = 34, District = "Beşiktaş", Name = "The Golden Fork", Address = "Barbaros Blv. No:22", PhoneNumber = "555-2345", AvgPrice = 200, CreatedAt = seedDate },
            new Restaurant { RestaurantId = 4, CategoryId = 2, CityId = 6, District = "Çankaya", Name = "Elegance Palace", Address = "Tunalı Hilmi Cd. No:33", PhoneNumber = "555-2222", AvgPrice = 250, CreatedAt = seedDate },
            new Restaurant { RestaurantId = 5, CategoryId = 3, CityId = 16, District = "Nilüfer", Name = "QuickBite", Address = "FSM Blv. No:85", PhoneNumber = "555-7654", AvgPrice = 15, CreatedAt = seedDate },
            new Restaurant { RestaurantId = 6, CategoryId = 3, CityId = 41, District = "İzmit", Name = "Speedy Bites", Address = "Cumhuriyet Cd. No:99", PhoneNumber = "555-3333", AvgPrice = 18, CreatedAt = seedDate },
            new Restaurant { RestaurantId = 7, CategoryId = 4, CityId = 7, District = "Konyaaltı", Name = "Sakura Sushi", Address = "Akdeniz Blv. No:4", PhoneNumber = "555-3210", AvgPrice = 40, CreatedAt = seedDate },
            new Restaurant { RestaurantId = 8, CategoryId = 4, CityId = 26, District = "Tepebaşı", Name = "Tokyo Delight", Address = "İki Eylül Cd. No:25", PhoneNumber = "555-4444", AvgPrice = 50, CreatedAt = seedDate },
            new Restaurant { RestaurantId = 9, CategoryId = 5, CityId = 27, District = "Şahinbey", Name = "Mamma Mia", Address = "İncilipınar Cd. No:15", PhoneNumber = "555-9087", AvgPrice = 60, CreatedAt = seedDate },
            new Restaurant { RestaurantId = 10, CategoryId = 5, CityId = 42, District = "Selçuklu", Name = "Pasta House", Address = "Mevlana Cd. No:18", PhoneNumber = "555-5555", AvgPrice = 65, CreatedAt = seedDate },
            new Restaurant { RestaurantId = 11, CategoryId = 6, CityId = 1, District = "Seyhan", Name = "Brewed Awakening", Address = "Turhan Cemal Beriker Blv. No:9", PhoneNumber = "555-4532", AvgPrice = 12, CreatedAt = seedDate },
            new Restaurant { RestaurantId = 12, CategoryId = 6, CityId = 61, District = "Ortahisar", Name = "Morning Brew", Address = "Kahramanmaraş Cd. No:44", PhoneNumber = "555-6666", AvgPrice = 15, CreatedAt = seedDate },
            new Restaurant { RestaurantId = 13, CategoryId = 7, CityId = 38, District = "Melikgazi", Name = "Grill Master", Address = "Sivas Cd. No:34", PhoneNumber = "555-8723", AvgPrice = 90, CreatedAt = seedDate },
            new Restaurant { RestaurantId = 14, CategoryId = 7, CityId = 33, District = "Yenişehir", Name = "Prime Cut Grill", Address = "Atatürk Cd. No:52", PhoneNumber = "555-7777", AvgPrice = 100, CreatedAt = seedDate },
            new Restaurant { RestaurantId = 15, CategoryId = 8, CityId = 34, District = "Üsküdar", Name = "Le Petit Bistro", Address = "Çamlıca Cd. No:11", PhoneNumber = "555-6789", AvgPrice = 45, CreatedAt = seedDate },
            new Restaurant { RestaurantId = 16, CategoryId = 8, CityId = 34, District = "Bakırköy", Name = "Bistro Bella", Address = "İstasyon Cd. No:3", PhoneNumber = "555-8888", AvgPrice = 40, CreatedAt = seedDate }
        );

        // Seed table layouts that sum to the same headcounts the restaurants used to
        // have via the old flat Capacity column, so seeded data looks consistent.
        var tableSeeds = new List<RestaurantTable>();
        var targetSeats = new (int RestaurantId, int TotalSeats)[]
        {
            (1, 120), (2, 150), (3, 80), (4, 70), (5, 60), (6, 45), (7, 50), (8, 60),
            (9, 70), (10, 80), (11, 40), (12, 35), (13, 90), (14, 85), (15, 30), (16, 25),
        };
        var nextTableId = 1;
        foreach (var (restaurantId, totalSeats) in targetSeats)
        {
            tableSeeds.AddRange(GenerateTableSeeds(restaurantId, totalSeats, ref nextTableId));
        }
        builder.Entity<RestaurantTable>().HasData(tableSeeds);

        // Each restaurant's original cover photo, seeded as the first (and for now
        // only) entry in its gallery — the cover shown everywhere is just whichever
        // gallery image sorts first, not a separate field.
        var coverFileNames = new (int RestaurantId, string FileName)[]
        {
            (1, "oceans_bounty.jpg"), (2, "fishermans_wharf.jpg"), (3, "golden_fork.jpg"), (4, "elegance_palace.jpg"),
            (5, "quick_bite.jpg"), (6, "speedy_bites.jpg"), (7, "sakura_sushi.jpg"), (8, "tokyo_delight.jpg"),
            (9, "mamma_mia.jpg"), (10, "pasta_house.jpg"), (11, "brewed_awakening.jpg"), (12, "morning_brew.jpg"),
            (13, "grill_master.jpg"), (14, "prime_cut_grill.jpg"), (15, "le_petit_bistro.jpg"), (16, "bistro_bella.jpg"),
        };
        builder.Entity<RestaurantImage>().HasData(
            coverFileNames.Select((c, index) => new RestaurantImage
            {
                ImageId = index + 1,
                RestaurantId = c.RestaurantId,
                FileName = c.FileName,
                DisplayOrder = 0,
            }));

        // One settings row per restaurant, defaulted to exactly what used to be
        // hardcoded in ReservationAvailability/AddReservation (90-min duration, no
        // buffer, 30-min slot spacing, 6-day advance window) — so no existing
        // restaurant's booking behavior changes until an owner/admin edits it.
        builder.Entity<RestaurantSettings>().HasData(
            targetSeats.Select((t, index) => new RestaurantSettings
            {
                SettingsId = index + 1,
                RestaurantId = t.RestaurantId,
                ReservationDurationMinutes = 90,
                BufferMinutes = 0,
                SlotGranularityMinutes = 30,
                MaxGuestsPerReservation = null,
                MinAdvanceBookingHours = 0,
                MaxAdvanceBookingDays = 6,
            }));

        // Seven business-hour rows per restaurant (one per day of week), all
        // defaulted to the same 11:00-22:00, never-closed window the app used
        // to apply globally to every restaurant.
        var businessHourSeeds = new List<RestaurantBusinessHour>();
        var nextBusinessHourId = 1;
        foreach (var (restaurantId, _) in targetSeats)
        {
            foreach (DayOfWeek day in Enum.GetValues(typeof(DayOfWeek)))
            {
                businessHourSeeds.Add(new RestaurantBusinessHour
                {
                    BusinessHourId = nextBusinessHourId++,
                    RestaurantId = restaurantId,
                    DayOfWeek = day,
                    OpenTime = new TimeSpan(11, 0, 0),
                    CloseTime = new TimeSpan(22, 0, 0),
                    IsClosed = false,
                });
            }
        }
        builder.Entity<RestaurantBusinessHour>().HasData(businessHourSeeds);

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

            builder.Entity<Reservation>()
                .HasOne(r => r.Table)
                .WithMany()
                .HasForeignKey(r => r.TableId)
                .OnDelete(DeleteBehavior.Restrict); // a table with reservations can't be deleted

            builder.Entity<Restaurant>()
                .HasOne(r => r.Category)
                .WithMany(c => c.Restaurants)
                .HasForeignKey(r => r.CategoryId)
                .OnDelete(DeleteBehavior.Restrict); // a category in use by a restaurant can't be deleted

            builder.Entity<Restaurant>()
                .HasOne(r => r.City)
                .WithMany(c => c.Restaurants)
                .HasForeignKey(r => r.CityId)
                .OnDelete(DeleteBehavior.Restrict); // a city in use by a restaurant can't be deleted

            builder.Entity<Restaurant>()
                .HasOne(r => r.OwnerUser)
                .WithMany()
                .HasForeignKey(r => r.OwnerUserId)
                .OnDelete(DeleteBehavior.SetNull); // deleting the owner account unassigns it, doesn't delete the restaurant

            // One owner manages exactly one restaurant. SQL Server unique indexes treat
            // each NULL as distinct, so any number of unassigned restaurants is still fine.
            builder.Entity<Restaurant>()
                .HasIndex(r => r.OwnerUserId)
                .IsUnique();

            builder.Entity<RestaurantTable>().HasKey(t => t.TableId);

            builder.Entity<RestaurantTable>()
                .HasOne(t => t.Restaurant)
                .WithMany(r => r.Tables)
                .HasForeignKey(t => t.RestaurantId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<RestaurantImage>().HasKey(i => i.ImageId);

            builder.Entity<RestaurantImage>()
                .HasOne(i => i.Restaurant)
                .WithMany(r => r.Images)
                .HasForeignKey(i => i.RestaurantId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<RestaurantSettings>().HasKey(s => s.SettingsId);

            builder.Entity<RestaurantSettings>()
                .HasOne(s => s.Restaurant)
                .WithOne(r => r.Settings)
                .HasForeignKey<RestaurantSettings>(s => s.RestaurantId)
                .OnDelete(DeleteBehavior.Cascade);

            // One settings row per restaurant, same "unique index on the FK" trick
            // used above for Restaurant.OwnerUserId.
            builder.Entity<RestaurantSettings>()
                .HasIndex(s => s.RestaurantId)
                .IsUnique();

            builder.Entity<RestaurantBusinessHour>().HasKey(h => h.BusinessHourId);

            builder.Entity<RestaurantBusinessHour>()
                .HasOne(h => h.Restaurant)
                .WithMany(r => r.BusinessHours)
                .HasForeignKey(h => h.RestaurantId)
                .OnDelete(DeleteBehavior.Cascade);
        //////////////////////////
    }

    // Deterministic table breakdown (mix of 2/4/6/8-tops) that sums to exactly
    // totalSeats — used only for seeding realistic-looking demo data.
    private static List<RestaurantTable> GenerateTableSeeds(int restaurantId, int totalSeats, ref int nextTableId)
    {
        var sizes = new[] { 6, 4, 4, 2, 2, 8 };
        var tables = new List<RestaurantTable>();
        var remaining = totalSeats;
        var tableNumber = 1;
        var i = 0;

        while (remaining > 0)
        {
            var size = Math.Min(sizes[i % sizes.Length], remaining);
            tables.Add(new RestaurantTable
            {
                TableId = nextTableId++,
                RestaurantId = restaurantId,
                Label = $"T{tableNumber}",
                SeatCount = size,
            });
            remaining -= size;
            tableNumber++;
            i++;
        }

        return tables;
    }

}