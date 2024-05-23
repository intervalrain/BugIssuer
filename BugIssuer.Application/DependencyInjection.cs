using BugIssuer.Application.Common.Behaviors;

using Microsoft.Extensions.DependencyInjection;

namespace BugIssuer.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
            // AuthorizationBehavior
            options.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        return services;
    }
}

