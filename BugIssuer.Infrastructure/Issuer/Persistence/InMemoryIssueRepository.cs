﻿using BugIssuer.Application.Common.Interfaces;
using BugIssuer.Domain;
using BugIssuer.Domain.Enums;

namespace BugIssuer.Infrastructure.Issuer.Persistence;


public class InMemoryIssueRepository : IIssueRepository
{
    private readonly Dictionary<int, Issue> _dbContext;
    private static int _issueCount;

    private async Task AddFakeData()
    {
        await AddIssueAsync(new Issue(_issueCount + 1, "test1", "Raw Data", "00012415", "Yao", "test1", DateTime.UtcNow), CancellationToken.None);
        await AddIssueAsync(new Issue(_issueCount + 1, "test2", "Query UI", "00012415", "Yao", "test2", DateTime.UtcNow), CancellationToken.None);
        await AddIssueAsync(new Issue(_issueCount + 1, "test3", "Wafer Map", "00012415", "Yao", "test3", DateTime.UtcNow), CancellationToken.None);
        await AddIssueAsync(new Issue(_issueCount + 1, "test4", "Contour Plot", "00012415", "Yao", "test4", DateTime.UtcNow), CancellationToken.None);
        await AddIssueAsync(new Issue(_issueCount + 1, "test1", "Raw Data", "00012415", "Yao", "test1", DateTime.UtcNow), CancellationToken.None);
        var issue = _dbContext[1];
        issue.Comments.Add(new Comment(1, issue.IssueId, "00053997", "Rain Hu", "Hello", DateTime.UtcNow));
        issue.Comments.Add(new Comment(1, issue.IssueId, "00053997", "Rain Hu", "Yao sir, Can you describe the issue in detail?", DateTime.UtcNow));
    }

    public InMemoryIssueRepository()
    {
        _dbContext = new Dictionary<int, Issue>();
        _issueCount = 0;
        AddFakeData().GetAwaiter().GetResult();
    }

    public async Task AddIssueAsync(Issue issue, CancellationToken cancellationToken)
    {
        _issueCount++;
        await Task.Run(() => _dbContext.Add(issue.IssueId, issue));
    }

    public async Task<int> CountIssuesAsync(CancellationToken cancellationToken)
    {
        return await Task.Run(() => _issueCount);
    }

    public async Task<Issue?> GetIssueByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await Task.Run(() => _dbContext.TryGetValue(id, out var issue) ? issue : throw new KeyNotFoundException());
    }

    public async Task<List<Issue>> ListIssuesAsync(CancellationToken cancellationToken)
    {
        return await Task.Run(() => _dbContext.Select(x => x.Value).ToList());
    }

    public async Task<List<Issue>> ListIssuesByStatusAsync(Status status, CancellationToken cancellationToken)
    {
        return await Task.Run(() => _dbContext.Select(x => x.Value).Where(x => x.Status == status).ToList());
    }

    public async Task RemoveIssueAsync(Issue issue, CancellationToken cancellationToken)
    {
        if (_dbContext.ContainsKey(issue.IssueId))
        {
            await Task.Run(() => _dbContext.Remove(issue.IssueId));
        }
    }

    public async Task UpdateIssueAsync(Issue issue, CancellationToken cancellationToken)
    {
        if (_dbContext.ContainsKey(issue.IssueId))
        {
            await Task.Run(() => _dbContext[issue.IssueId] = issue);
        }
    }
}