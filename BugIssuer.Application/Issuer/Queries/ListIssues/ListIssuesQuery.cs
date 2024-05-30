using BugIssuer.Application.Common.Security.Policies;
using BugIssuer.Application.Common.Security.Request;
using BugIssuer.Application.Common.Security.Permissions;
using BugIssuer.Domain;
using ErrorOr;
using MediatR;

namespace BugIssuer.Application.Issuer.Queries.ListIssues;

[Authorize(Permissions = Permission.Issue.List)]
public record ListIssuesQuery(string SortOrder, string FilterStatus, bool IsAdmin) : IRequest<ErrorOr<List<Issue>>>;