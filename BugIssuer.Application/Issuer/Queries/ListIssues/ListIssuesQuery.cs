using BugIssuer.Domain;

using ErrorOr;

using MediatR;

namespace BugIssuer.Application.Issuer.Queries.ListIssues;

public record ListIssuesQuery(string SortOrder, string FilterStatus, bool IsAdmin) : IRequest<ErrorOr<List<Issue>>>;