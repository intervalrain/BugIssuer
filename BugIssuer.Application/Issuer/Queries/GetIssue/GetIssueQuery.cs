using BugIssuer.Domain;

using ErrorOr;

using MediatR;

namespace BugIssuer.Application.Issuer.Queries.GetIssue;

public record GetIssueQuery(int IssueId) : IRequest<ErrorOr<Issue>>;