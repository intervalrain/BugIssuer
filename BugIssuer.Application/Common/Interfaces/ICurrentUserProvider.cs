using BugIssuer.Application.Common.Security.Users;

namespace BugIssuer.Application.Common.Interfaces;

public interface ICurrentUserProvider
{
    CurrentUser CurrentUser { get; }
}