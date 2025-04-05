using Microsoft.AspNetCore.Identity;
using PosMvc.Models;

namespace PosMvc.Context
{
    public class DbSeed
    {
        public async static Task Initialize(IServiceProvider serviceProvider, AppDbContext context,
           UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            context.Database.EnsureCreated();
            // Seed Roles
            await SeedRoles(roleManager);

            // Seed Users
            await SeedUsers(userManager, roleManager);

            // Seed Menus
            await SeedMenus(context);

        }

        private static async Task SeedRoles(RoleManager<Role> roleManager)
        {
            string[] roleNames = { "Admin", "Manager", "Cashier" };

            foreach (var roleName in roleNames)
            {
                var role = await roleManager.FindByNameAsync(roleName);
                if (role == null)
                {
                    role = new Role { Name = roleName, Description = $"{roleName} role" };
                    await roleManager.CreateAsync(role);
                }
            }
        }

        private static async Task SeedUsers(UserManager<User> userManager,
            RoleManager<Role> roleManager)
        {
            if (userManager.Users.Any()) return; // Check if users already exist

            // Ensure roles exist before assigning to users
            var adminRole = await roleManager.FindByNameAsync("Admin");
            var managerRole = await roleManager.FindByNameAsync("Manager");
            var cashierRole = await roleManager.FindByNameAsync("Cashier");

            if (adminRole == null || managerRole == null || cashierRole == null)
            {
                throw new Exception("One or more roles were not found.");
            }

            var adminUser = new User
            {
                UserName = "admin@mypos.com",
                Email = "admin@mypos.com",
                FullName = "Admin User",
                Address = "123 Admin Street"
                ,
                RoleId = adminRole.Id
            };

            var managerUser = new User
            {
                UserName = "manager@mypos.com",
                Email = "manager@mypos.com",
                FullName = "Manager User",
                Address = "456 Manager Street",
                RoleId = managerRole.Id
            };

            var cashierUser = new User
            {
                UserName = "cashier@mypos.com",
                Email = "cashier@mypos.com",
                FullName = "Cashier User",
                Address = "789 Cashier Street",
                RoleId = cashierRole.Id
            };

            var users = new[] { adminUser, managerUser, cashierUser };

            foreach (var appUser in users)
            {
                var userResult = await userManager.CreateAsync(appUser, "Test@123");
                if (userResult.Succeeded)
                {
                    // Assign roles based on username
                    if (appUser.UserName == "admin@mypos.com")
                    {
                        await userManager.AddToRoleAsync(appUser, adminRole.Name);  // Adding to Admin role
                    }
                    else if (appUser.UserName == "manager@mypos.com")
                    {
                        await userManager.AddToRoleAsync(appUser, managerRole.Name);  // Adding to Manager role
                    }
                    else if (appUser.UserName == "cashier@mypos.com")
                    {
                        await userManager.AddToRoleAsync(appUser, cashierRole.Name);  // Adding to Cashier role
                    }
                }
                else
                {
                    // Log or handle the error if user creation failed
                    foreach (var error in userResult.Errors)
                    {
                        Console.WriteLine($"Error creating user {appUser.UserName}: {error.Description}");
                    }
                }
            }
        }

        private static async Task SeedMenus(AppDbContext context)
        {
            if (context.Menus.Any()) return; // Check if menus already exist

            var menus = new[]
            {
                new Menu { Name = "Dashboard", Path = "/dashboard", Description = "Overview of system" },
                new Menu { Name = "Sales", Path = "/sales", Description = "Manage sales transactions" },
                new Menu { Name = "Products", Path = "/products", Description = "Manage products" },
                new Menu { Name = "Inventory", Path = "/inventory", Description = "Manage inventory" },
                new Menu { Name = "Reports", Path = "/reports", Description = "View reports" }
            };

            context.Menus.AddRange(menus);
            await context.SaveChangesAsync();
        }
    }
}
