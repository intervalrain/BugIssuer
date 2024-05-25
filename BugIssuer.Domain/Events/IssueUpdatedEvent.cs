using BugIssuer.Domain.Common;

namespace BugIssuer.Domain.Events;

public record IssueUpdatedEvent(int IssueId, string Title, string Description, string Category, DateTime LastUpdate) : IDomainEvent;