using BugIssuer.Application.Common.Security.Request;
using BugIssuer.Application.Common.Security.Permissions;
using BugIssuer.Domain;
using ErrorOr;
using MediatR;

namespace BugIssuer.Application.Issuer.Queries.SearchIssues;

[Authorize(Permissions = Permission.Issue.Search)]
public record SearchIssuesQuery(string UserId, string Text) : IAuthorizableRequest<ErrorOr<List<Issue>>>;