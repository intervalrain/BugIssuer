using BugIssuer.Application.Common.Security.Request;

using ErrorOr;

namespace BugIssuer.Application.Common.Interfaces;

public interface IAuthorizationService
{
    ErrorOr<Success> AuthorizaCurrentUser<T>(
        IAuthorizableRequest<T> request,
        List<string> requiredRoles,
        List<string> requiredPermissions,
        List<string> requiredPolicies);
}

