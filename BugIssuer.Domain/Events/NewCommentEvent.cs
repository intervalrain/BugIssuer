using BugIssuer.Domain.Common;

namespace BugIssuer.Domain.Events;

public record NewCommentEvent(int IssueId, int CommentId) : IDomainEvent;