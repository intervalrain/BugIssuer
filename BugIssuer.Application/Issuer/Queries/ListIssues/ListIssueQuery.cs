using BugIssuer.Domain;

using ErrorOr;

using MediatR;

namespace BugIssuer.Application.Issuer.Queries.ListIssues;

public record ListIssueQuery() : IRequest<ErrorOr<List<Issue>>>;