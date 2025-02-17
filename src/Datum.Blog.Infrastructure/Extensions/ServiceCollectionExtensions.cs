using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Datum.Blog.Domain.Interfaces;
using Datum.Blog.Infrastructure.Persistence;
using Datum.Blog.Infrastructure.Repositories;
using Datum.Blog.Infrastructure.Service;
using Datum.Blog.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Datum.Blog.Infrastructure.Authentication;

namespace Datum.Blog.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString));

        using var scope = services.BuildServiceProvider().CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        if (dbContext.Database.GetPendingMigrations().Any())
        {
            dbContext.Database.Migrate();
        }

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IPostRepository, PostRepository>();

        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

        services.AddScoped<JwtService>(provider =>
    new JwtService(configuration["Jwt:SecretKey"]!));

        return services;
    }
}