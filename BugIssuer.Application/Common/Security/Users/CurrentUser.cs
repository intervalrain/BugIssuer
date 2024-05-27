namespace BugIssuer.Application.Common.Security.Users;

public class CurrentUser
{
    public string UserId;
    public string UserName;
    public string Email;
    public IReadOnlyList<string> Permissions;
    public IReadOnlyList<string> Roles;

    public bool IsAdmin() => Roles.Contains("admin");
}

