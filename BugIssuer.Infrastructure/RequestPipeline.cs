using BugIssuer.Infrastructure.Common.Middleware;

using Microsoft.AspNetCore.Builder;

namespace BugIssuer.Infrastructure;

public static class RequestPipeline
{
    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
    {
        app.UseMiddleware<EventualConsistencyMiddleware>();
        return app;
    }
}