using System.Text.Json;
using booksy.API.Models.DTOs;
using booksy.API.Models.Enums;
using booksy.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace booksy.API.Data
{
    public class DataSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider, string filePath)
        {
            var context = serviceProvider.GetRequiredService<AppDbContext>();
            var hardwareService = serviceProvider.GetRequiredService<IHardwareService>();
            var userService = serviceProvider.GetRequiredService<IUserService>();

            // 0. Ensure default Admin exists
            var adminEmail = "admin@admin.com";
            var existingAdmin = await context.Users.FirstOrDefaultAsync(u => u.Email == adminEmail && u.DateDeleted == null);
            if (existingAdmin == null)
            {
                await userService.CreateAsync(new CreateUserDto
                {
                    Email = adminEmail,
                    Password = "Password123!",
                    Role = UserRole.Admin
                });
            }

            // 1. Clear RentalRecords and Hardware tables
            await context.Database.ExecuteSqlRawAsync("DELETE FROM RentalRecords");
            await context.Database.ExecuteSqlRawAsync("DELETE FROM Hardware");
            
            // Reset auto-increment IDs
            try {
                await context.Database.ExecuteSqlRawAsync("DELETE FROM sqlite_sequence WHERE name='RentalRecords' OR name='Hardware'");
            } catch {
                // Ignore if sqlite_sequence doesn't exist or errors out
            }

            // 2. Read JSON
            if (!File.Exists(filePath)) return;

            var json = await File.ReadAllTextAsync(filePath);
            var items = JsonSerializer.Deserialize<List<SeederItem>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (items == null) return;

            // 3. Seed data using services
            foreach (var item in items)
            {
                // Ensure user exists if AssignedTo is provided
                if (!string.IsNullOrWhiteSpace(item.AssignedTo))
                {
                    var existingUser = await context.Users.FirstOrDefaultAsync(u => u.Email == item.AssignedTo && u.DateDeleted == null);
                    if (existingUser == null)
                    {
                        await userService.CreateAsync(new CreateUserDto
                        {
                            Email = item.AssignedTo,
                            Password = "Password123!",
                            Role = UserRole.User
                        });
                    }
                }

                DateOnly? purchaseDate = null;
                if (!string.IsNullOrWhiteSpace(item.PurchaseDate))
                {
                    if (DateOnly.TryParseExact(item.PurchaseDate, new[] { "yyyy-MM-dd", "dd-MM-yyyy" }, CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
                    {
                        purchaseDate = date;
                    }
                }

                var status = HardwareStatus.Unknown;
                if (item.Status == "Available") status = HardwareStatus.Available;
                else if (item.Status == "In Use") status = HardwareStatus.InUse;
                else if (item.Status == "Repair") status = HardwareStatus.Repair;

                var createDto = new CreateHardwareDto
                {
                    Name = item.Name ?? "Unknown",
                    Brand = item.Brand ?? "",
                    PurchaseDate = purchaseDate,
                    Status = status,
                    Notes = item.Notes,
                    History = item.History,
                    AssignedTo = item.AssignedTo
                };

                await hardwareService.CreateAsync(createDto);
            }
        }

        private class SeederItem
        {
            public int Id { get; set; }
            public string? Name { get; set; }
            public string? Brand { get; set; }
            public string? PurchaseDate { get; set; }
            public string? Status { get; set; }
            public string? Notes { get; set; }
            public string? AssignedTo { get; set; }
            public string? History { get; set; }
        }
    }
}
