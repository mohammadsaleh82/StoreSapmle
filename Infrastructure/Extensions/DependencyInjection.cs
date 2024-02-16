using Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Persistence.Context;

namespace Infrastructure.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
 
        services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<StoreContext>()
            .AddDefaultTokenProviders();
        
        // Configure Identity options
        services.Configure<IdentityOptions>(options =>
        {
            // Password settings
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 8;
        
            // Lockout settings
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
            options.Lockout.MaxFailedAccessAttempts = 5;
        
            // User settings
            options.User.RequireUniqueEmail = true;
        });
        
        services.ConfigureApplicationCookie(options =>
        {
            // Cookie settings
            options.Cookie.HttpOnly = false;
            options.ExpireTimeSpan = TimeSpan.FromDays(30);
        
            // Login path
            options.LoginPath = "/auth";
        
            // Access denied path
            options.AccessDeniedPath = "/auth/logout";

            options.LogoutPath = "/auth/logout";
        
            // ReturnUrl parameter
            options.SlidingExpiration = true;
        });
        
        services.AddDbContext<StoreContext>(opt => { opt.UseSqlServer(configuration.GetConnectionString("Default")); });
    
        return services;
    }
}