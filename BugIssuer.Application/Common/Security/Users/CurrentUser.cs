namespace BugIssuer.Application.Common.Security.Users;

public class CurrentUser
{
    public string UserId { get; init; }
    public string UserName { get; init; }
    public string Email { get; init; }
    public IReadOnlyList<string> Permissions { get; init; }
    public IReadOnlyList<string> Roles { get; init; }

    public bool IsAdmin() => Roles.Contains("admin");
}

