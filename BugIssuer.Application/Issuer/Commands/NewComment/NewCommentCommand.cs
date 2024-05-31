using BugIssuer.Application.Common.Security.Request;
using BugIssuer.Application.Common.Security.Permissions;

using BugIssuer.Domain;
using ErrorOr;

namespace BugIssuer.Application.Issuer.Commands.NewComment;

[Authorize(Permissions = Permission.Issue.Comment)]
public record NewCommentCommand(
    int IssueId,
    string UserId,
    string Content
    ) : IAuthorizableRequest<ErrorOr<Comment>>;