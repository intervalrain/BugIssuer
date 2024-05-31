using BugIssuer.Application.Common.Security.Policies;
using BugIssuer.Application.Common.Security.Request;
using BugIssuer.Application.Common.Security.Permissions;
using ErrorOr;
using MediatR;

namespace BugIssuer.Application.Issuer.Commands.UpdateIssue;

[Authorize(Permissions = Permission.Issue.Remove, Policies = Policy.SelfOrAdmin)]
public record UpdateIssueCommand(int IssueId, string UserId, string Title, string Description, string Category, int Urgency) : IAuthorizableRequest<ErrorOr<Success>>;