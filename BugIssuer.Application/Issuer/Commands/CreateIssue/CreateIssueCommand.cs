using BugIssuer.Application.Common.Security.Request;
using BugIssuer.Application.Common.Security.Permissions;
using BugIssuer.Domain;

using ErrorOr;

namespace BugIssuer.Application.Issuer.Commands.CreateIssue;

[Authorize(Permissions = Permission.Issue.Create)]
public record CreateIssueCommand(
	string Title,
	string Description,
	string Category,
	int Urgency,
	string UserId,
	DateTime DateTime) : IAuthorizableRequest<ErrorOr<Issue>>;