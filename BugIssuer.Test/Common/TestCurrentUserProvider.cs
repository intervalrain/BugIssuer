using BugIssuer.Application.Common.Interfaces;
using BugIssuer.Application.Common.Security.Users;

namespace BugIssuer.Test.Common;

public class TestCurrentUserProvider : ICurrentUserProvider
{
    private CurrentUser? _currentUser;

    public void Returns(CurrentUser currentUser)
    {
        _currentUser = currentUser;
    }

    public CurrentUser CurrentUser => _currentUser ?? CurrentUserFactory.Create(); 
}

