using ErrorOr;

using MediatR;

namespace BugIssuer.Application.Issuer.Commands.RemoveIssue;

public record RemoveIssueCommand(string Applicant, int IssueId) : IRequest<ErrorOr<Success>>;