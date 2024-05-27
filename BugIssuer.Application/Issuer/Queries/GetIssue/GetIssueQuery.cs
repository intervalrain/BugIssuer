using BugIssuer.Domain;

using ErrorOr;

using MediatR;

namespace BugIssuer.Application.Issuer.Queries.GetIssue;

public record GetIssueQuery(string Applicant, int IssueId) : IRequest<ErrorOr<Issue>>;