using BugIssuer.Domain;
using ErrorOr;
using MediatR;

namespace BugIssuer.Application.Issuer.Commands.NewComment;

public record NewCommentCommand(
    int IssueId,
    string AuthorId,
    string Author,
    string Content
    ) : IRequest<ErrorOr<Comment>>;