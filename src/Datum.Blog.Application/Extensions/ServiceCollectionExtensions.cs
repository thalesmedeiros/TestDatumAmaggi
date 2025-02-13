using Microsoft.Extensions.DependencyInjection;
using Datum.Blog.Application.Interfaces;
using Datum.Blog.Application.Mapping;
using Datum.Blog.Application.Services;

namespace Datum.Blog.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MappingProfile));
        services.AddMediatR(configuration =>
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            configuration.RegisterServicesFromAssemblies(assemblies.ToArray());
        });

        return services;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IPostService, PostService>();
        services.AddScoped<IUserService, UserService>();
        return services;
    }
}