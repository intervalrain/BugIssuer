using BugIssuer.Application.Common.Security.Request;
using BugIssuer.Application.Common.Security.Permissions;
using BugIssuer.Application.Common.Security.Roles;

using ErrorOr;

namespace BugIssuer.Application.Issuer.Commands.AssignIssue;

[Authorize(Permissions = Permission.Issue.Assign, Roles = Role.Admin)]
public record AssignIssueCommand(string UserId, int IssueId, string Assignee) : IAuthorizableRequest<ErrorOr<Success>>;