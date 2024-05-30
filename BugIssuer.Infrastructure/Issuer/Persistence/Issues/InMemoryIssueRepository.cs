using BugIssuer.Application.Common.Interfaces;
using BugIssuer.Application.Common.Interfaces.Persistence;
using BugIssuer.Domain;
using BugIssuer.Domain.Enums;

namespace BugIssuer.Infrastructure.Issuer.Persistence.Issues;


public class InMemoryIssueRepository : IIssueRepository
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly Dictionary<int, Issue> _dbContext;
    private static int _issueCount;

    private async Task AddFakeData()
    {
        var issues = new List<Issue>
        {
            Issue.Create("test1", "Raw Data", "00012415", "Bin Yao", "test1", 1, _dateTimeProvider.Now),
            Issue.Create("test2", "Query UI", "00012415", "Bin Yao", "test2", 2, _dateTimeProvider.Now),
            Issue.Create("test3", "Wafer Map", "00012415", "Bin Yao", "test3", 3, _dateTimeProvider.Now),
            Issue.Create("test4", "Contour Plot", "00012415", "Bin Yao", "test4", 4, _dateTimeProvider.Now),
            Issue.Create("test5", "Raw Data", "00012415", "Bin Yao", "test5", 5, _dateTimeProvider.Now),
            Issue.Create("test6", "Raw Data", "00058163", "Mark QH Chen", "test6", 3, _dateTimeProvider.Now),
            Issue.Create("test7", "Raw Data", "00058163", "Mark QH Chen", "test7", 3, _dateTimeProvider.Now),
            Issue.Create("test8", "Raw Data", "00053997", "Rain Hu", "test8", 3, _dateTimeProvider.Now),
            Issue.Create("test9", "Raw Data", "00053997", "Rain Hu", "test9", 3, _dateTimeProvider.Now),
            Issue.Create("test10", "Raw Data", "00053997", "Rain Hu", "test10", 3, _dateTimeProvider.Now)
        };
        await AddIssueAsync(issues[0], default);
        await AddIssueAsync(issues[1], default);
        await AddIssueAsync(issues[2], default);
        await AddIssueAsync(issues[3], default);
        await AddIssueAsync(issues[4], default);
        await AddIssueAsync(issues[5], default);
        await AddIssueAsync(issues[6], default);
        await AddIssueAsync(issues[7], default);
        await AddIssueAsync(issues[8], default);
        await AddIssueAsync(issues[9], default);

        issues[0].AddComment("00053997", "Rain Hu", "Hello", DateTime.Now);
        issues[0].AddComment("00053997", "Rain Hu", "Yao sir, Can you describe the issue in detail?", DateTime.Now);

        issues[1].Close();
        issues[2].Assign("Kun");
        issues[3].Assign("Yue");
    }

    public InMemoryIssueRepository(IDateTimeProvider dateTimeProvider)
    {
        _dbContext = new Dictionary<int, Issue>();
        _issueCount = 0;
        _dateTimeProvider = dateTimeProvider;
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

    public async Task<List<Issue>> ListIssuesByAuthorIdAsync(string authorId, CancellationToken cancellationToken)
    {
        return await Task.Run(() => _dbContext.Select(x => x.Value).Where(x => x.AuthorId == authorId).ToList());
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