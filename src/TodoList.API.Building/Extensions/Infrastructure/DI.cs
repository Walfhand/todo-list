using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TodoList.API.Building.Persistence.Contexts;
using TodoList.Common.EFCore;

namespace TodoList.API.Building.Extensions.Infrastructure;

public static class DI
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCustomDbContext<AppDbContext>();
        return services;
    }

    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder webApplication)
    {
        webApplication.UseMigration<AppDbContext>();
        return webApplication;
    }
}
