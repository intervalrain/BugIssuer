using BugIssuer.Application.Common.Interfaces.Persistence;

using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

using System.Security.Claims;
using System.Text.Encodings.Web;

using BugIssuer.Application.Common.Security.Permissions;

namespace BugIssuer.Web;

public class FakeAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly IUserRepository _userRepository;
    private readonly IAdminProvider _adminProvider;
    private readonly string _userId = "00053997";

    public FakeAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock,
        IUserRepository userRepository,
        IAdminProvider adminProvider)
        : base(options, logger, encoder, clock)
    {
        _userRepository = userRepository;
        _adminProvider = adminProvider;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var user = await _userRepository.GetUserByIdAsync(_userId, CancellationToken.None);

        bool isAdmin = _adminProvider.IsAdmin(_userId);

        var roles = isAdmin ? new string[] { "user", "admin" } : new string[] { "user" };

        var permissions = new string[]
        {
            Permission.Issue.Create,
            Permission.Issue.Get,
            Permission.Issue.List,
            Permission.Issue.ListMy,
            Permission.Issue.Comment,
            Permission.Issue.Update,
            Permission.Issue.Remove,
            Permission.Issue.Search
        };

        var claims = new List<Claim>
        {
            new Claim("id", _userId),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email),
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        foreach (var permission in permissions)
        {
            claims.Add(new Claim("permissions", permission));
        }

        var identity = new ClaimsIdentity(claims, "FakeScheme");
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, "FakeScheme");

        return await Task.FromResult(AuthenticateResult.Success(ticket));
    }
}

