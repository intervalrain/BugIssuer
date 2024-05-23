using BugIssuer.Domain.Common;

namespace BugIssuer.Domain.Events;

public record IssueCreatedEvent(int IssueId) : IDomainEvent;