using BugIssuer.Application.Common.Security.Policies;
using BugIssuer.Application.Common.Security.Request;
using BugIssuer.Application.Common.Security.Permissions;
using BugIssuer.Domain;
using ErrorOr;

namespace BugIssuer.Application.Issuer.Queries.ListMyIssues;

[Authorize(Permissions = Permission.Issue.ListMy, Policies = Policy.SelfOrAdmin)]
public record ListMyIssuesQuery(string UserId, string SortOrder, string FilterStatus) : IAuthorizableRequest<ErrorOr<List<Issue>>>;