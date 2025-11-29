using DerreksLens.Application.Services;
using DerreksLens.Core.Domain.Entities;
using DerreksLens.Core.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration; // Needed for IConfiguration
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DerreksLens.Infrastructure.Data
{
    public class DbInitializer
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<DbInitializer>>();
            var config = scope.ServiceProvider.GetRequiredService<IConfiguration>(); // 💉 Inject Config

            // 1. Ensure Database Created
            await context.Database.MigrateAsync();

            // 2. Check if Admin Exists
            if (await context.Users.AnyAsync(u => u.Role == UserRole.Admin))
            {
                return;
            }

            if (!await context.Categories.AnyAsync())
            {
                await context.Categories.AddRangeAsync(
                    new Category { Name = "The Lab", Slug = "the-lab", Description = "Code and Experiments" },
                    new Category { Name = "The Journey", Slug = "the-journey", Description = "Growth" },
                    new Category { Name = "Hot Takes", Slug = "hot-takes", Description = "Opinions" }
                );
                await context.SaveChangesAsync();
            }

            logger.LogInformation("🚀 No Admin found. Seeding from Configuration...");

            // 3. Get Credentials from Secrets (with fallbacks if missing)
            var adminUsername = config["AdminSettings:Username"] ?? "Admin";
            var adminEmail = config["AdminSettings:Email"] ?? "admin@localhost";
            var adminPassword = config["AdminSettings:Password"];

            if (string.IsNullOrEmpty(adminPassword))
            {
                logger.LogError("❌ Admin password not configured in User Secrets! Skipping seeding.");
                return;
            }

            // 4. Create Hash
            PasswordHasher.CreatePasswordHash(adminPassword, out string hash, out string salt);

            var adminUser = new User
            {
                Username = adminUsername,
                Email = adminEmail,
                Role = UserRole.Admin,
                Bio = "System Administrator",
                PasswordHash = hash,
                PasswordSalt = salt,
                CreatedAt = DateTime.UtcNow
            };

            await context.Users.AddAsync(adminUser);
            await context.SaveChangesAsync();

            logger.LogInformation("✅ Admin seeded: {Email}", adminEmail);
        }
    }
}