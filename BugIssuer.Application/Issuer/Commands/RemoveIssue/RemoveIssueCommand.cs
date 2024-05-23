using ErrorOr;

using MediatR;

namespace BugIssuer.Application.Issuer.Commands.RemoveIssue;

public record RemoveIssueCommand(string AuthorId, int IssueId) : IRequest<ErrorOr<Success>>;