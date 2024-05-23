using BugIssuer.Domain;
using BugIssuer.Domain.Common;
using BugIssuer.Infrastructure.Common.Middleware;

using MediatR;

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace BugIssuer.Infrastructure.Common.Persistance;

public class AppDbContext : DbContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IPublisher _publisher;

    public DbSet<Issue> Issues { get; set; } = null!;
    public DbSet<Comment> Comments { get; set; } = null!;

    public AppDbContext(DbContextOptions<AppDbContext> options, IHttpContextAccessor httpContextAccessor, IPublisher publisher)
        : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
        _publisher = publisher;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }

    public async override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var domainEvents = ChangeTracker.Entries<Entity>()
            .SelectMany(entry => entry.Entity.PopDomainEvents())
            .ToList();

        if (IsUserWaitingOnline())
        {
            AddDomainEventsToOfflineProcessingQueue(domainEvents);
            return await base.SaveChangesAsync(cancellationToken);
        }

        await PublishDomainEvents(domainEvents);
        return await base.SaveChangesAsync(cancellationToken);
    }

    private async Task PublishDomainEvents(List<IDomainEvent> domainEvents)
    {
        foreach (var @event in domainEvents)
        {
            await _publisher.Publish(@event);
        }
    }

    private void AddDomainEventsToOfflineProcessingQueue(List<IDomainEvent> domainEvents)
    {
        var queue = _httpContextAccessor.HttpContext!.Items.TryGetValue(EventualConsistencyMiddleware.DomainEventKey, out var value) &&
            value is Queue<IDomainEvent> events ? events : new();

        domainEvents.ForEach(queue.Enqueue);
        _httpContextAccessor.HttpContext.Items[EventualConsistencyMiddleware.DomainEventKey] = queue;
    }

    public bool IsUserWaitingOnline() => _httpContextAccessor.HttpContext is not null;
}

