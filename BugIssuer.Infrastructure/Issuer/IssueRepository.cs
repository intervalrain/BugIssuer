using BugIssuer.Application.Common.Interfaces;
using BugIssuer.Domain;
using BugIssuer.Infrastructure.Common.Persistance;

using Microsoft.EntityFrameworkCore;

namespace BugIssuer.Infrastructure.Issuer;

public class IssueRepository : IIssueRepository
{
    private readonly AppDbContext _dbContext;
    private static int _issueCount;

    public IssueRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
        _issueCount = 0;
    }

    public async Task AddIssueAsync(Issue issue, CancellationToken cancellationToken)
    {
        _issueCount++;
        await _dbContext.AddAsync(issue, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<int> CountIssuesAsync(CancellationToken cancellationToken)
    {
        return Task.Run(() => _issueCount);
    }

    public async Task<Issue?> GetIssueByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _dbContext.Issues.FindAsync(id, cancellationToken);
    }

    public async Task<List<Issue>> ListIssuesAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.Issues
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task RemoveIssueAsync(Issue issue, CancellationToken cancellationToken)
    {
        _dbContext.Remove(issue);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateIssueAsync(Issue issue, CancellationToken cancellationToken)
    {
        _dbContext.Update(issue);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}

