using BugIssuer.Domain;

using ErrorOr;

using MediatR;

namespace BugIssuer.Application.Issuer.Queries.ListMyIssues;

public record ListMyIssuesQuery(string AuthorId, string SortOrder, string FilterStatus) : IRequest<ErrorOr<List<Issue>>>;