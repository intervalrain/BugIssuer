using BugIssuer.Domain;

namespace BugIssuer.Application;

public interface IIssueService
{
    Task<IEnumerable<Issue>> GetAllIssueAsync();
    Task<Issue> GetIssueByIdAsync();
    Task CreateIssueAsync(Issue issue);
    Task UpdateIssueAsync(Issue issue);
    Task DeleteIssueAsync(int id);
}