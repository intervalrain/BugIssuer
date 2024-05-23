using BugIssuer.Domain.Common;

namespace BugIssuer.Domain.Events;

public record IssueRemovedEvent(int IssueId) : IDomainEvent;