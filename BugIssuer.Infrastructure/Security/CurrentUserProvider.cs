using System.Security.Claims;
using System.Security.Principal;

using BugIssuer.Application.Common.Interfaces;
using BugIssuer.Application.Common.Security.Users;

using Microsoft.AspNetCore.Http;

using Throw;

namespace BugIssuer.Infrastructure.Security;

public class CurrentUserProvider : ICurrentUserProvider
{
    private readonly CurrentUser _currentUser;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUser CurrentUser => _currentUser;

    public CurrentUserProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;

        httpContextAccessor.HttpContext.ThrowIfNull();

        _currentUser = new CurrentUser
        {
            UserId = GetSingleClaimValue("id"),
            UserName = GetSingleClaimValue(ClaimTypes.Name),
            Email = GetSingleClaimValue(ClaimTypes.Email),
            Permissions = GetClaimValues("permissions"),
            Roles = GetClaimValues(ClaimTypes.Role)
        };
    }

    private List<string> GetClaimValues(string claimType) =>
        _httpContextAccessor.HttpContext!.User.Claims
            .Where(claim => claim.Type == claimType)
            .Select(claim => claim.Value)
            .ToList();

    private string GetSingleClaimValue(string claimType) =>
        _httpContextAccessor.HttpContext!.User.Claims
            .Single(claim => claim.Type == claimType)
            .Value;
}

