using ErrorOr;

using MediatR;

namespace BugIssuer.Application.Issuer.Commands.UpdateIssue;

public record UpdateIssueCommand(int IssueId, string AuthorId, string Title, string Description, string Category) : IRequest<ErrorOr<Success>>;