using BugIssuer.Application.Common.Security.Policies;
using BugIssuer.Application.Common.Security.Request;
using BugIssuer.Application.Common.Security.Permissions;

using ErrorOr;

namespace BugIssuer.Application.Issuer.Commands.ReopenIssue;

[Authorize(Permissions = Permission.Issue.Reopen, Policies = Policy.SelfOrAdmin)]
public record ReopenIssueCommand(string UserId, int IssueId) : IAuthorizableRequest<ErrorOr<Success>>;