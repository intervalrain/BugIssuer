using BugIssuer.Application.Common.Interfaces;
using BugIssuer.Infrastructure.Common.Persistance;
using BugIssuer.Infrastructure.Common.Services;
using BugIssuer.Infrastructure.Issuer.Persistence;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Npgsql.EntityFrameworkCore.PostgreSQL;

namespace BugIssuer.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddHttpContextAccessor()
            .AddServices()
            .AddPersistence();
            //.AddBackgroundServices(configuration)

        return services;       
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddSingleton<IDateTimeProvider, SystemDateTimeProvider>();
        return services;
    }

    private static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>(options => options.UseNpgsql(""));

        services.AddScoped<IIssueRepository, IssueRepository>();

        return services;
    }

    //private static IServiceCollection AddBackgroundService(this IServiceCollection services, IConfiguration configuration)
    //{
    //    services.AddEmailNotifications(configuration);
    //    return services;
    //}

    //private static IServiceCollection AddEmailNotifications(this IServiceCollection services, IConfiguration configuration)
    //{

    //}
}

