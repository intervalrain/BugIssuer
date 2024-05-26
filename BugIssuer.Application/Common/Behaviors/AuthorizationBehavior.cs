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
        var attrs = request.GetType().GetCustomAttributes<AuthorizeAttribute>().ToList();

        if (attrs.Count == 0)
        {
            return await next();
        }

        var roles = attrs
            .SelectMany(attr => attr.Roles?.Split(',') ?? new string[0]).ToList();

        var permissions = attrs
            .SelectMany(attr => attr.Permissions?.Split(',') ?? new string[0]).ToList();

        var policies  = attrs
            .SelectMany(attr => attr.Policies?.Split(',') ?? new string[0]).ToList();

        var result = _authorizationService.AuthorizaCurrentUser(
            request,
            roles,
            permissions,
            policies);

        return result.IsError
            ? (dynamic)result.Errors
            : await next();
    }
}

