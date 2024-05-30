using BugIssuer.Application.Common.Security.Policies;
using BugIssuer.Application.Common.Security.Request;
using BugIssuer.Application.Common.Security.Permissions;
using BugIssuer.Domain;
using ErrorOr;
using MediatR;

namespace BugIssuer.Application.Issuer.Queries.ListMyIssues;

[Authorize(Permissions = Permission.Issue.ListMy, Policies = Policy.SelfOrAdmin)]
public record ListMyIssuesQuery(string AuthorId, string SortOrder, string FilterStatus) : IRequest<ErrorOr<List<Issue>>>;