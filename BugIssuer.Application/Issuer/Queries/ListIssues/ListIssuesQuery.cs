using BugIssuer.Domain;

using ErrorOr;

using MediatR;

namespace BugIssuer.Application.Issuer.Queries.ListIssues;

public record ListIssuesQuery(string SortOrder, string filterStatus) : IRequest<ErrorOr<List<Issue>>>;