using BugIssuer.Application.Common.Security.Request;
using BugIssuer.Application.Common.Security.Permissions;

using ErrorOr;
using BugIssuer.Application.Common.Security.Policies;

namespace BugIssuer.Application.Issuer.Commands.CloseIssue;

[Authorize(Permissions = Permission.Issue.Close, Policies = Policy.SelfOrAdmin)]
public record CloseIssueCommand(string UserId, int IssueId) : IAuthorizableRequest<ErrorOr<Success>>;