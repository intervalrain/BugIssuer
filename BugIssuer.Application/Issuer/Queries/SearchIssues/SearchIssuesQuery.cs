using BugIssuer.Domain;

using ErrorOr;

using MediatR;

namespace BugIssuer.Application.Issuer.Queries.SearchIssues;

public record SearchIssuesQuery(string Text) : IRequest<ErrorOr<List<Issue>>>;