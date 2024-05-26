using System;

using BugIssuer.Application.Common.Interfaces;
using BugIssuer.Application.Common.Security.Request;

using ErrorOr;

namespace BugIssuer.Infrastructure.Security;

public class AuthorizationService : IAuthorizationService
{
    public AuthorizationService(IPolicyEnforcer policyEnforcer, ICurrentUserProvider currentUserProvider)
    {
    }

    public ErrorOr<Success> AuthorizaCurrentUser<T>(IAuthorizableRequest<T> request, List<string> requiredRoles, List<string> requiredPermissions, List<string> requiredPolicies)
    {
        throw new NotImplementedException();
    }
}

