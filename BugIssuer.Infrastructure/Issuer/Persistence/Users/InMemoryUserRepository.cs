using BugIssuer.Application.Common.Interfaces.Persistence;
using BugIssuer.Domain;

namespace BugIssuer.Infrastructure.Issuer.Persistence.Users;

public class InMemoryUserRepository : IUserRepository
{
    private readonly List<User> _users = new List<User>
    {
        User.Create("00053997", "Rain Hu", "rain_hu@umc.com", "SMG"),
        User.Create("00057886", "Carlos Tsui", "carlos_tsui@umc.com", "SMG"),
        User.Create("00058163", "Mark QH Chen", "mark_qh_chen@umc.com", "SMG"),
        User.Create("00012415", "Bin Yao", "bin_yao@umcsx.com", "PEI")
    };

    public async Task<User?> GetUserByIdAsync(string id, CancellationToken cancellationToken)
    {
        return await Task.Run(() => _users.FirstOrDefault(user => user.UserId == id));
    }

    public async Task<List<User>?> GetUserByIdsAsync(IEnumerable<string> ids, CancellationToken cancellationToken)
    {
        List<User> users = new List<User>();
        foreach (var id in ids)
        {
            var user = await GetUserByIdAsync(id, cancellationToken);
            if (user is null) continue;
            users.Add(user);
        }
        return users;
    }
}

