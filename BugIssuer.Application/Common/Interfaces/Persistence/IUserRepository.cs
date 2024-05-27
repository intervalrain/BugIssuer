using BugIssuer.Domain;

namespace BugIssuer.Application.Common.Interfaces.Persistence;

public interface IUserRepository
{
    Task<User?> GetUserByIdAsync(string id, CancellationToken cancellationToken);

    Task<List<User>?> GetUserByIdsAsync(IEnumerable<string> ids, CancellationToken cancellationToken);
}

