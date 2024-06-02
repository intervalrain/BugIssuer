using BugIssuer.Application.Common.Security.Request;
using BugIssuer.Application.Common.Security.Permissions;
using BugIssuer.Application.Common.Security.Policies;

using ErrorOr;

namespace BugIssuer.Application.Issuer.Commands.RemoveIssue;

[Authorize(Permissions = Permission.Issue.Remove, Policies = Policy.SelfOrAdmin)]
public record RemoveIssueCommand(string UserId, int IssueId) : IAuthorizableRequest<ErrorOr<Success>>;