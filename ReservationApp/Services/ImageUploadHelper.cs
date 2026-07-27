namespace ReservationApp.Services
{
    // Shared validation/save logic for restaurant photo uploads (cover image on
    // Create/Edit, gallery photos on the Images management pages) — one place for the
    // extension whitelist and size limit instead of duplicating them per page.
    public static class ImageUploadHelper
    {
        public static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".webp" };
        public const long MaxSizeBytes = 5 * 1024 * 1024; // 5 MB

        public static string? Validate(IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!AllowedExtensions.Contains(extension))
            {
                return "Only .jpg, .jpeg, .png, and .webp image files are allowed.";
            }

            if (file.Length > MaxSizeBytes)
            {
                return "Image file must be smaller than 5 MB.";
            }

            return null;
        }

        public static async Task<string> SaveAsync(IFormFile file, string webRootPath)
        {
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            var fileName = $"{DateTime.Now:yyyyMMddHHmmssfff}_{Guid.NewGuid():N}{extension}";
            var fullPath = Path.Combine(webRootPath, "Restaurant_Img", fileName);

            using var stream = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(stream);

            return fileName;
        }
    }
}
