using BugIssuer.Application.Common.Security.Request;
using BugIssuer.Application.Common.Security.Users;

using ErrorOr;

namespace BugIssuer.Application.Common.Interfaces;

public interface IPolicyEnforcer
{
    public ErrorOr<Success> Authorize<T>(
        IAuthorizableRequest<T> request,
        CurrentUser currentUser,
        string policy);
}

