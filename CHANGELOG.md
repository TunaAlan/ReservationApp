# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/), and this project adheres to [Semantic Versioning](https://semver.org/).

> **Status:** Pre-production demo / learning project — no formal release has shipped yet.

## [0.2.0] - 2026-07-23

**Security — Missing Authorization on Mutating Pages**
- `Admin/Restaurants/Create`, `Edit`, and `Delete` had no `[Authorize]` attribute at all — only the `Index` listing page checked for the `admin` role, so anyone (including unauthenticated visitors) could reach these pages directly by URL and create, edit, or delete restaurants.
- `Client/Restaurants/AddReservation` had the same gap: no `[Authorize]`, so an unauthenticated visitor could submit a reservation, which would be saved with a `null` `UserId`.
- Added `[Authorize(Roles = "admin")]` to the three restaurant pages and `[Authorize(Roles = "client")]` to `AddReservation`.

**Security — IDOR on Reservation Deletion**
- `DeleteReservationModel` looked up a reservation by `id` alone, with no check that it belonged to the requesting user — any authenticated client could delete another client's reservation by guessing/iterating IDs.
- Added `[Authorize]` and an ownership check (`reservation.UserId == current user`) to both `OnGetAsync` and `OnPostDeleteAsync`.

**Security — Unrestricted File Uploads**
- Restaurant image upload (`Create`, `Edit`) accepted any file type and any size — a `.pdf` (or any other file) would be silently saved as the "restaurant image."
- Added an extension whitelist (`.jpg`, `.jpeg`, `.png`, `.webp`) and a 5 MB size limit, enforced server-side before the file is written to disk.

**Security — GET-Based Deletion (CSRF)**
- `Admin/Restaurants/Delete` performed the actual deletion inside `OnGetAsync` — a plain link click (or an `<img>`/prefetch) would delete a restaurant, with no anti-forgery protection possible on GET.
- Restructured into the standard pattern: `OnGetAsync` now only renders a confirmation page; `OnPostAsync` performs the deletion, protected by Razor Pages' automatic anti-forgery token. Route changed from `?id=` query string to `{id:int}` path segment to match.

**Validation — No Server-Side Validation on Any Model**
- `Restaurant`, `RestaurantDto`, and `Reservation` had no `DataAnnotations` at all, so `ModelState.IsValid` was always `true` regardless of input — empty names, negative prices, zero capacity, and out-of-range party sizes were all accepted.
- Added `[Required]`, `[StringLength]`, `[Range]`, and `[Phone]` attributes across all three models. New `AddValidationConstraints` migration applied the resulting column-length changes to the database.

**Business Logic — Reservation Rules**
- `AddReservation` accepted any date (including the past) and never checked restaurant capacity, so a restaurant could be double- or over-booked indefinitely.
- Added a past-date check and a capacity check (sum of `NumberOfPeople` already booked for that date, compared against `Restaurant.Capacity`).

**Fixed**
- `Admin/Restaurants/Delete.cshtml.cs` deleted the restaurant's image from the wrong folder (`"restaurants"` instead of `"Restaurant_Img"`), so uploaded images were never actually cleaned up. Corrected the path.

## [0.1.0] - 2026-07-22

**Security — HTTPS Redirection**
- Enabled `app.UseHttpsRedirection();` in `Program.cs`, previously left commented out. HTTP traffic is now redirected to HTTPS.

**Security — Removed Leaked Production Credentials**
- Removed a real Azure SQL connection string (server, username, plaintext password) that had been committed to `appsettings.json` since the first commit. The underlying Azure resource had already been deleted, so no live rotation was required.
- `ConnectionStrings:DefaultConnection` in `appsettings.json` is now empty; real values are supplied per-environment, never committed.

**Security — Secrets Moved Out of Git-Tracked Files**
- Local SQL Server password, local connection string, and the seeded admin account's email/password are no longer stored in any git-tracked file.
- Local (`dotnet run`): supplied via `dotnet user-secrets` (stored outside the repo, at `~/.microsoft/usersecrets/<id>/`).
- Docker (`docker compose up`): supplied via a local `.env` file (gitignored), referenced in `docker-compose.yml` as `${SA_PASSWORD}`, `${SEED_ADMIN_EMAIL}`, `${SEED_ADMIN_PASSWORD}`.
- Added `.env.example` (committed) documenting the expected keys without real values.

**Auth — Runtime Admin Seeding**
- Added a `Development`-only block at the end of `Program.cs` that seeds an admin user (email/password read from configuration) and assigns the `admin` role, if one doesn't already exist. Idempotent: the `INSERT` only happens once, on subsequent runs it's a no-op check.
- Previously there was no way to reach any `/Admin/*` page — `Register.cshtml.cs` only ever assigns the `client` role, and no admin account existed anywhere.

**Reservation Flow — Success/Error Messaging Fix**
- `AddReservationModel.SuccessMessage` moved from a plain field to `[TempData]`, since the previous implementation was lost after `RedirectToPage` (a redirect starts a new request, resetting PageModel state).
- Added rendering of `errorMessage` to `AddReservation.cshtml` — previously set but never displayed.
- Added a success alert to `MyReservations.cshtml` reading from `TempData`.

**Database — Fixed a Migration System That Never Worked**
- Discovered that all four pre-existing migrations (`UserDataMigration`, `FirstMigration`, `RestaurantInfo`, `Reservation_1`) were missing their `.Designer.cs` companion files — never committed, since the very first commit. Without them, EF Core cannot discover a migration at all, so the project could never have been set up from scratch by anyone who cloned it.
- Removed all four broken migrations and replaced them with a single clean `InitialCreate` migration generated from the current model (with matching `.Designer.cs`), verified end-to-end against a fresh database.

**Database — Restaurant Seed Data**
- Removed `Migrations/sqlQuery/*.sql` (`Delete_Account.sql`, `Delete_RoleId.sql`, `Delete_UserId.sql`, `DeleteRestaurantInfo.sql`, `DeleteRestaurantTable.sql`, `Insert_Roles.sql`, `RestaurantDbSet.sql`) — one-off debug scripts with hardcoded GUIDs, outside the EF Core migration system.
- Migrated the 16-restaurant demo dataset into EF Core's `HasData(...)` seeding (`ApplicationDbContext.OnModelCreating`), now part of the `InitialCreate` migration.
- Recreated `wwwroot/Restaurant_Img/` and added the images for all 16 seeded restaurants.

**Repository Hygiene — Untracked Build Artifacts and Vendored Libraries**
- Added `.gitignore` (`bin/`, `obj/`, `.vs/`, `wwwroot/lib/`, `.DS_Store`, `.env`).
- Added `ReservationApp/libman.json` restore manifest for bootstrap 5.1.0, jquery 3.6.0, jquery-validate 1.19.5, jquery-validation-unobtrusive 4.0.0.
- Untracked `wwwroot/lib/`, `bin/`, `obj/`, `.vs/`, and `.DS_Store` from git (files kept on disk).

**Containerization — Docker Support**
- Added a multi-stage `Dockerfile` (SDK image to build/publish, ASP.NET runtime image to run) and `.dockerignore`.
- Extended `docker-compose.yml` with an `app` service alongside the existing `sqlserver` service, connected via Docker's internal network (`Server=sqlserver,1433`).
- Two supported workflows, verified independently: `dotnet run` + `docker compose up sqlserver` for day-to-day development with hot-reload, or `docker compose up` for the fully containerized stack (`http://localhost:8080`).

**Documentation**
- Added inline comments (English) to `Program.cs` and `ApplicationDbContext.cs` explaining the two seeding strategies (migration-time `HasData` for static demo data vs. runtime seeding for credentials) and why the admin-seed block is idempotent.

## [0.0.2] - 2025-04-22
- Removed the `wwwroot/Restaurant_Img` directory (`b8a67cb`) — reintroduced in `0.1.0` above.

## [0.0.1] - 2024-12-10
- Initial commit and first working version of the Restaurant Reservation System: ASP.NET Core Razor Pages app with EF Core, SQL Server, and Identity-based role authentication (`b892790`, `8f49865`).
