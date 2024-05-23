namespace BugIssuer.Domain.Common;

public abstract class Entity
{
    public int Id { get; private init; }

    protected readonly List<IDomainEvent> _domainEvents;

    public Entity(int id)
    {
        _domainEvents = new List<IDomainEvent>();
        Id = id;
    }

    public List<IDomainEvent> PopDomainEvents()
    {
        var copy = _domainEvents.ToList();
        _domainEvents.Clear();

        return copy;
    }
}