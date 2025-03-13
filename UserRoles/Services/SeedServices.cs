﻿using Microsoft.AspNetCore.Identity;
using UserRoles.Data;
using UserRoles.Models;

namespace UserRoles.Services
{
    public class SeedServices
    {
        public static async Task SeedDatabase(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<Users>>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<SeedServices>>();

            try
            {
                //ensure the database is ready
                logger.LogInformation("Ensuring the Database is created");
                await context.Database.EnsureCreatedAsync();

                // add roles
                logger.LogInformation("Adding roles");
                await AddRoleAsync(roleManager, "Admin");
                await AddRoleAsync(roleManager, "User");

                //add Admin user
                logger.LogInformation("Adding Admin user");
                var adminEmail = "admin@cement.com";
                if(await userManager.FindByEmailAsync(adminEmail) == null)
                {
                    var adminUser = new Users
                    {
                        FullName = "Admin",
                        UserName = "admin",
                        NormalizedUserName = "admin".ToUpper(),
                        Email = adminEmail,
                        NormalizedEmail = adminEmail.ToUpper(),
                        EmailConfirmed = true,
                        SecurityStamp = Guid.NewGuid().ToString()
                    };
                    var result = await userManager.CreateAsync(adminUser, "Admin@123");
                    if (result.Succeeded)
                    {
                        logger.LogInformation("Assining Admin Role to the admin user");
                        await userManager.AddToRoleAsync(adminUser, "Admin");

                    }
                    else
                    {
                        logger.LogError("An error occurred while creating the Admin user");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while seeding the database.");
            }
        }

        private static async Task AddRoleAsync(RoleManager<IdentityRole> roleManager, string roleName)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                var result = await roleManager.CreateAsync(new IdentityRole(roleName));
                if (!result.Succeeded)
                {
                    throw new Exception($"An error occurred while creating the role '{roleName}':{string.Join(", ", result.Errors.Select(e=>e.Description))}");
                }
            }
        }
    }
}
