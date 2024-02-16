using Domain.Entities.Products;
using Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Context;

namespace Persistence;

public class Seed
{
    public static async Task SeedData(StoreContext context, IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
        if (!await context.Users.AnyAsync())
        {
            if (!await context.Roles.AnyAsync())
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
                await roleManager.CreateAsync(new IdentityRole("DeleteProducts"));
                await roleManager.CreateAsync(new IdentityRole("EditProducts"));
                await roleManager.CreateAsync(new IdentityRole("ShowProducts"));
                await roleManager.CreateAsync(new IdentityRole("CreateProducts"));
            }
            var user = new User
            {
                UserName = "admin@ali.com",
                Email = "admin@ali.com",
                Avatar = "default.png" // Provide your own avatar URL if needed
            };
            await userManager.CreateAsync(user, "12345678!");

            // Assign the "Admin" role to the user
            await userManager.AddToRoleAsync(user, "Admin");
        }
        if (!context.Categories.Any())
        {
            // Add some sample categories
            var categories = new List<Category>
            {
                new Category { Name = "Electronics" },
                new Category { Name = "Clothing" },
                new Category { Name = "Books" }
                // Add more categories as needed
            };

            // Add categories to the database
            context.Categories.AddRange(categories);
            await context.SaveChangesAsync();
        }

        // Check if there are any existing products
        if (!context.Products.Any())
        {
            // Add some sample products
            var products = new List<Product>
            {
                new Product { Name = "Smartphone", Price = 499.99m, Description = "High-end smartphone", CategoryId = 1,Image = "default.png" },
                new Product { Name = "T-shirt", Price = 19.99m, Description = "Casual cotton t-shirt", CategoryId = 2,Image = "default.png" },
                new Product { Name = "Programming Book", Price = 39.99m, Description = "Learn programming with this book", CategoryId = 3 ,Image = "default.png"}
                // Add more products as needed
            };

            // Add products to the database
            context.Products.AddRange(products);
            await context.SaveChangesAsync();
        }
     
    }
}

 