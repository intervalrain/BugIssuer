using BugIssuer.Domain;

namespace BugIssuer.Application.Common.Interfaces;

public interface IIssueRepository
{
    Task AddIssueAsync(Issue issue, CancellationToken cancellationToken);
    Task<Issue> GetIssueByIdAsync(int id, CancellationToken cancellationToken);
    Task<int> CountIssuesAsync(CancellationToken cancellationToken);
    Task<List<Issue>> ListIssuesAsync(CancellationToken cancellationToken);
    Task RemoveIssueAsync(Issue issue, CancellationToken cancellationToken);
    Task UpdateIssueAsync(Issue issue, CancellationToken cancellationToken);
}