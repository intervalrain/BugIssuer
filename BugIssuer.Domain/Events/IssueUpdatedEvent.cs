using BugIssuer.Domain.Common;

namespace BugIssuer.Domain.Events;

public record IssueUpdatedEvent(int IssueId) : IDomainEvent;