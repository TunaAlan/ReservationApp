# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/), and this project adheres to [Semantic Versioning](https://semver.org/).

> **Status:** Pre-production demo / learning project — no formal release has shipped yet.

## [0.5.0] - 2026-07-27

**Fixed — Retroactive Double-Booking via Settings Changes**
- The booking engine always computed how long a reservation occupied its table from the restaurant's *current* `RestaurantSettings`, never from what was in effect at the moment it was actually booked. An owner shortening turn-time or cleanup buffer after a guest had already booked would make the engine treat that guest's table as free earlier than promised — a real double-booking window for anyone who booked into the gap in between.
- `Reservation` now snapshots `DurationMinutes`/`BufferMinutes` at booking time (`Migrations/20260725012007_AddReservationDurationBufferSnapshot`, backfilled from each existing reservation's restaurant's settings at the time of the migration). `ReservationAvailability.Overlaps` is now intentionally asymmetric: an *existing* reservation's occupied window always comes from its own snapshot, while a *candidate* new booking's window comes from the restaurant's live settings — answering "would this new request collide with what was already promised?" rather than "would it collide under today's settings?"
- Verified end-to-end: booked the only large-enough table at a restaurant, shortened the restaurant's turn-time afterward, and confirmed the table stayed correctly blocked through its original window and became bookable again exactly on time — not early.

## [0.4.0] - 2026-07-27

**Added — Restaurant Owner Portal**
- Self-registration now offers an "Account type" choice (client or restaurant) — previously every signup was hardcoded to the client role, so the pages below were unreachable.
- New `/Owner/Index` dashboard (today's peak-occupied tables, a 7-day occupancy strip, quick links) and `/Owner/Edit` profile page, both resolving the target restaurant via `OwnerUserId == currentUser` — never a route/query parameter.

**Added — Category & City Reference Data**
- Restaurants previously stored `Category` as free text with no `District` field at all. Replaced with proper `Category`/`City` entities, full Admin CRUD (`/Admin/Categories`, `/Admin/Cities`), and dropdown selection on the restaurant Create/Edit forms — deleting a category/city in use is blocked with a count of affected restaurants.

**Added — Table-Based Capacity**
- Replaced the single `Capacity` number with individually-labeled tables per restaurant (`/Admin/Restaurants/Tables`, `/Owner/Tables`): seat count, live occupied/free status, next-reservation lookahead, and a 7-day schedule per table. The booking engine now assigns each reservation to a specific best-fit table instead of just checking a capacity sum.

**Added — Restaurant Image Gallery**
- Restaurants can now have a multi-photo gallery (`/Owner/Images`, `/Admin/Restaurants/Images`) instead of a single cover image — reorder, delete, and add photos, each validated through a shared upload helper (extension whitelist + 5MB limit, same policy as the original `0.2.0` fix).

**Added — Restaurant Browsing & Filtering**
- Client restaurant list gained a category/city/price filter bar, a photo gallery per card, and per-restaurant "full today"/weekly-occupancy badges driven by the new booking engine. Also added an About Us page.

**Added — Restaurant Settings & Booking Rules Engine**
- Every restaurant's booking behavior — turn-time, cleanup buffer, time-slot granularity, business hours, advance-booking window, max guests — used to be a single hardcoded set of constants shared by the whole app. New Settings pages (`/Owner/Settings`, `/Admin/Restaurants/Settings`) let each restaurant configure all of it independently, including per-day business hours with a "copy Monday to all days" shortcut.
- **Operating Policies**: same-day acceptance, guest notes, auto-confirm vs. pending, and cancellation (with a configurable deadline) are now per-restaurant toggles, enforced in `AddReservation` and `DeleteReservation`. A reservation left unconfirmed shows a "Pending confirmation" badge on the guest's reservation list and the owner's table schedule.
- Every newly created restaurant now gets a default Settings row and 7 BusinessHours rows set before the first save, so the booking engine never encounters a restaurant with no configuration at all.

**Fixed**
- `MaxGuestsPerReservation` and `MinAdvanceBookingHours` existed on the settings model from the start of this work but were never actually checked at booking time — both are now enforced, with a clear error message when violated.

## [0.3.0] - 2026-07-23

**Fixed — Additional Crash Bugs**
- Welcome page role buttons used the wrong role-name casing (`"Admin"`/`"Client"` vs. the actual `"admin"`/`"client"`) and an invalid redirect path, so they silently did nothing for a logged-in user. Fixed both.
- `MyReservations` had no `[Authorize]` and a broken `RedirectToPage("/Account/Login")` (missing the `Identity` area) — an unauthenticated visit crashed with a 500. Added `[Authorize(Roles = "client")]` and corrected the redirect.
- `AddReservation` had two more broken `RedirectToPage` targets (`/Client/ReservationList`, `/NotFound`) that crashed with a 500 for a deleted or invalid restaurant id. Corrected both.
- `AddReservationModel.Restaurant` was `[BindProperty]`-bound but never submitted by the form; once `Restaurant` gained `[Required]` fields (0.2.0), every reservation submission silently failed validation. Removed `[BindProperty]`.
- Two CSS bugs: a missing unit (`margin-right: 10` → `10px`) and trailing whitespace inside four `asp-validation-for` attributes that broke validation-message binding.
- `Edit.cshtml`'s "Current Image" preview referenced the wrong property (the upload `IFormFile` instead of the stored filename) — always rendered broken.

**Infrastructure — Data Protection Key Persistence**
- Rebuilding the Docker `app` container regenerated the Data Protection key each time, silently logging everyone out on every rebuild. Added a named volume (`reservationapp-dpkeys`) so the key survives rebuilds.

**Added — Seeded Client Account**
- Added a `SeedClient` counterpart to `SeedAdmin` (same idempotent pattern, factored into a shared `SeedUserAsync` helper in `Program.cs`) so a ready-to-use client login exists alongside the admin one.

**Frontend — Visual Refresh**
- Restyled the welcome page, navbar, admin restaurant list, and client restaurant browsing/reservation flow: icons, consistent button sizing/colors, restaurant cards with a detail modal, role-based navigation, and a shared `_RestaurantDetailList` partial. Cosmetic only — no behavior change.

**Removed**
- Deleted unused scaffold leftovers: `ScaffoldingReadMe.txt`, an empty `reservation.http`, and `wwwroot/js/site.js` (comments only, no code) along with its now-dead `<script>` reference.

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
