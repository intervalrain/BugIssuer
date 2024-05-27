using BugIssuer.Domain;

namespace BugIssuer.Application.Common.Interfaces.Persistence;

public interface IAdminProvider
{
    bool IsAdmin(string userId);

    List<User> GetAdmins();
}

