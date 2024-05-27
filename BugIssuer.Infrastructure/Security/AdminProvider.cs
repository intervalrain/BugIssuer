using BugIssuer.Application.Common.Interfaces.Persistence;
using BugIssuer.Domain;

using Microsoft.Extensions.Configuration;

namespace BugIssuer.Infrastructure.Security;

public class AdminProvider : IAdminProvider
{
    private readonly IUserRepository _userRepository;

    private readonly List<string> _admins;

    public AdminProvider(IUserRepository userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _admins = configuration.GetSection("Admin").Get<List<string>>() ?? new List<string>();
    }

    public List<User> GetAdmins()
    {
        return _userRepository.GetUserByIdsAsync(_admins, CancellationToken.None).GetAwaiter().GetResult() ?? new List<User>();
    }

    public bool IsAdmin(string userId)
    {
        return _admins.Contains(userId);
    }
}

