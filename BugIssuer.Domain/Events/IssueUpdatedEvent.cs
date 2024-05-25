using BugIssuer.Domain.Common;

namespace BugIssuer.Domain.Events;

public record IssueUpdatedEvent(int IssueId, string Title, string Description, string Category, int Urgency, DateTime LastUpdate) : IDomainEvent;