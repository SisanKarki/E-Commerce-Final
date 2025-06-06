using Book.Models;
using Book.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Book.DataAccess.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // Create roles if they don't exist
            if (!await roleManager.RoleExistsAsync(SD.Role_Admin))
            {
                await roleManager.CreateAsync(new IdentityRole(SD.Role_Admin));
            }
            if (!await roleManager.RoleExistsAsync(SD.Role_Customer))
            {
                await roleManager.CreateAsync(new IdentityRole(SD.Role_Customer));
            }
            if (!await roleManager.RoleExistsAsync(SD.Role_Company))
            {
                await roleManager.CreateAsync(new IdentityRole(SD.Role_Company));
            }
            if (!await roleManager.RoleExistsAsync(SD.Role_Employee))
            {
                await roleManager.CreateAsync(new IdentityRole(SD.Role_Employee));
            }

            // Add admin role to existing user
            var adminEmail = "admin@gmail.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser != null)
            {
                if (!await userManager.IsInRoleAsync(adminUser, SD.Role_Admin))
                {
                    await userManager.AddToRoleAsync(adminUser, SD.Role_Admin);
                }
            }
        }
    }
} 