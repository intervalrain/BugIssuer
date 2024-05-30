using BugIssuer.Application.Common.Security.Request;
using BugIssuer.Application.Common.Security.Permissions;
using BugIssuer.Domain;
using ErrorOr;
using MediatR;

namespace BugIssuer.Application.Issuer.Queries.SearchIssues;

[Authorize(Permissions = Permission.Issue.Search)]
public record SearchIssuesQuery(string Text) : IRequest<ErrorOr<List<Issue>>>;