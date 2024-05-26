using BugIssuer.Application.Common.Interfaces;
using BugIssuer.Application.Common.Security.Request;
using BugIssuer.Application.Common.Security.Users;

using ErrorOr;

namespace BugIssuer.Infrastructure.Security;

public class PolicyEnforcer : IPolicyEnforcer
{
    public const string SelfOrAdmin = nameof(SelfOrAdmin);
    public const string Admin = nameof(Admin);

    public ErrorOr<Success> Authorize<T>(IAuthorizableRequest<T> request, CurrentUser currentUser, string policy)
    {
        return policy switch
        {
            SelfOrAdmin => SelfOrAdminPolicy(request, currentUser),
            _ => Error.Unexpected(description: "Unknown policy name")
        };
    }

    private static ErrorOr<Success> SelfOrAdminPolicy<T>(IAuthorizableRequest<T> request, CurrentUser currentUser)
    {
        return request.UserId == currentUser.UserId || currentUser.Roles.Contains(Admin)
            ? Result.Success
            : Error.Unauthorized(description: "Requesting user failed policy requirement");
    }
}

