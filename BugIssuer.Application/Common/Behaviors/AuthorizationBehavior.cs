using System.Reflection;

using BugIssuer.Application.Common.Interfaces;
using BugIssuer.Application.Common.Security.Request;

using ErrorOr;

using MediatR;

namespace BugIssuer.Application.Common.Behaviors;

public class AuthorizationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IAuthorizableRequest<TResponse>
    where TResponse : IErrorOr
{
    private readonly IAuthorizationService _authorizationService;

    public AuthorizationBehavior(IAuthorizationService authorizationService)
    {
        _authorizationService = authorizationService;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var authorizationAttributes = request.GetType().GetCustomAttributes<AuthorizeAttribute>().ToList();

        if (authorizationAttributes.Count == 0)
        {
            return await next();
        }

        var requiredRoles = authorizationAttributes
            .SelectMany(attr => attr.Roles?.Split(',') ?? new string[0]).ToList();

        var requiredPermissions = authorizationAttributes
            .SelectMany(attr => attr.Permissions?.Split(',') ?? new string[0]).ToList();

        var requiredPolicies = authorizationAttributes
            .SelectMany(attr => attr.Policies?.Split(',') ?? new string[0]).ToList();

        var result = _authorizationService.AuthorizeCurrentUser(
            request,
            requiredRoles,
            requiredPermissions,
            requiredPolicies);

        return result.IsError
            ? (dynamic)result.Errors
            : await next();
    }
}

