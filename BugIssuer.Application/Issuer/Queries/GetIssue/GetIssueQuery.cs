using BugIssuer.Application.Common.Security.Request;
using BugIssuer.Application.Common.Security.Permissions;
using BugIssuer.Domain;
using ErrorOr;
using MediatR;

namespace BugIssuer.Application.Issuer.Queries.GetIssue;

[Authorize(Permissions = Permission.Issue.Get)]
public record GetIssueQuery(string UserId, int IssueId) : IAuthorizableRequest<ErrorOr<Issue>>;