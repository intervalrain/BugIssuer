using BugIssuer.Application.Common.Security.Request;
using BugIssuer.Application.Common.Security.Roles;
using BugIssuer.Application.Common.Security.Permissions;
using ErrorOr;

namespace BugIssuer.Application.Issuer.Commands.LabelIssue;

[Authorize(Permissions = Permission.Issue.Label, Roles = Role.Admin)]
public record LabelIssueCommand(string UserId, int IssueId, string Label) : IAuthorizableRequest<ErrorOr<Success>>;