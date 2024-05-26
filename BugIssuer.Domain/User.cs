using System.Globalization;

namespace BugIssuer.Domain;

public class User
{
    private readonly List<int> _issueIds = new();

    public string Email { get; } = null!;

    public string UserId { get; }

    public string UserName { get; }
}

