using BugIssuer.Domain.Common;

namespace BugIssuer.Domain.Events;

public record IssueAcceptedEvent(int IssueId) : IDomainEvent;