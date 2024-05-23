using BugIssuer.Domain.Common;

namespace BugIssuer.Domain.Events;

public record IssueDoneEvent(int IssueId) : IDomainEvent;