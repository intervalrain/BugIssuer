using BugIssuer.Application.Common.Security.Request;
using BugIssuer.Application.Common.Security.Permissions;
using BugIssuer.Domain;
using ErrorOr;

namespace BugIssuer.Application.Issuer.Queries.ListIssues;

[Authorize(Permissions = Permission.Issue.List)]
public record ListIssuesQuery(string UserId, string SortOrder, string FilterStatus, bool IsAdmin) : IAuthorizableRequest<ErrorOr<List<Issue>>>;