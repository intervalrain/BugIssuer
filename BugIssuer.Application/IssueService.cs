using System;
using BugIssuer.Application.Repositories;
using BugIssuer.Domain;

namespace BugIssuer.Application;

public class IssueService : IIssueService
{
    public IssueService(IIssueRepository repository)
	{
	}

    public Task<IEnumerable<Issue>> GetAllIssueAsync()
    {
        throw new NotImplementedException();
    }


    public Task CreateIssueAsync(Issue issue)
    {
        throw new NotImplementedException();
    }

    public Task DeleteIssueAsync(int id)
    {
        throw new NotImplementedException();
    }


    public Task<Issue> GetIssueByIdAsync()
    {
        throw new NotImplementedException();
    }

    public Task UpdateIssueAsync(Issue issue)
    {
        throw new NotImplementedException();
    }
}

