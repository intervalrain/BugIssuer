using BugIssuer.Application.Common.Interfaces;
using BugIssuer.Application.Common.Interfaces.Persistence;
using BugIssuer.Infrastructure.Common.Services;
using BugIssuer.Infrastructure.Issuer.Persistence.Issues;
using BugIssuer.Infrastructure.Issuer.Persistence.Users;
using BugIssuer.Infrastructure.Security;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace BugIssuer.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)//, IConfiguration configuration)
    {
        services
            .AddHttpContextAccessor()
            .AddServices()
            .AddAuthorization()
            .AddPersistence();
            //.AddBackgroundServices(configuration)

        return services;       
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddSingleton<IDateTimeProvider, SystemDateTimeProvider>();
        return services;
    }

    private static IServiceCollection AddAuthorization(this IServiceCollection services)
    {
        services.AddSingleton<ICurrentUserProvider, CurrentUserProvider>();
        services.AddSingleton<IAdminProvider, AdminProvider>();
        services.AddScoped<IAuthorizationService, AuthorizationService>();
        services.AddScoped<IPolicyEnforcer, PolicyEnforcer>();
        return services;
    }

    private static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        //services.AddDbContext<AppDbContext>(options => options.UseNpgsql(""));

        services.AddSingleton<IIssueRepository, InMemoryIssueRepository>();
        services.AddSingleton<IUserRepository, InMemoryUserRepository>();

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

