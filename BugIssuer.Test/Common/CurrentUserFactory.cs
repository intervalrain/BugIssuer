using BugIssuer.Application.Common.Security.Users;

namespace BugIssuer.Test.Common;

public static class CurrentUserFactory
{
    public static CurrentUser Create(List<string>? permissions = null, List<string>? roles = null)
    {
        string userId = StringGenerator.GenerateUserId();
        string userName = StringGenerator.GenerateUserName();
        string email = StringGenerator.GenerateEmail(userName);

        return new CurrentUser
        {
            UserId = userId,
            UserName = userName,
            Email = email,
            Permissions = (permissions ?? new List<string>()).AsReadOnly(),
            Roles = (roles ?? new List<string>()).AsReadOnly()
        };
    }
}