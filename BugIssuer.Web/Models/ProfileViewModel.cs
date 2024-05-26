using BugIssuer.Application.Common.Security.Users;
using BugIssuer.Domain;
using BugIssuer.Domain.Enums;

namespace BugIssuer.Web.Models;

public class ProfileViewModel
{
    public CurrentUser CurrentUser { get; }
    public IEnumerable<Issue> Issues { get; }
    public int OpenIssuesCount => Issues.Count(issue => issue.Status == Status.Open);
    public int OngoingIssuesCount => Issues.Count(issue => issue.Status == Status.Ongoing);
    public int ClosedIssuesCount => Issues.Count(issue => issue.Status == Status.Closed);

    public ProfileViewModel(IEnumerable<Issue> issues, CurrentUser currentUser)
    {
        Issues = issues;
        CurrentUser = currentUser;
    }
}

