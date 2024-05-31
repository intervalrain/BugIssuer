using BugIssuer.Application.Issuer.Queries.GetIssue;
using BugIssuer.Application.Issuer.Queries.ListIssues;

namespace BugIssuer.Test.Common;

public static class IssueQueryFactory
{
    public static ListIssuesQuery CreateListIssuesQuery(
        string sortOrder = "",
        string filterStatus = "",
        bool isAdmin = false)
    {
        return new ListIssuesQuery(CurrentUserFactory.Create().UserId, sortOrder, filterStatus, isAdmin);
    }

    public static GetIssueQuery CreateGetIssueQuery(
        string? applicant = null,
        int? issueId = null)
    {
        return new GetIssueQuery(
            applicant ?? CurrentUserFactory.Create().UserId,
            issueId ?? new Random().Next());
    }
}

