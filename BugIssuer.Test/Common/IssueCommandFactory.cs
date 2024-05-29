using BugIssuer.Application.Issuer.Commands.CreateIssue;
using BugIssuer.Domain;

namespace BugIssuer.Test.Common;

public static class IssueCommandFactory
{
    public static CreateIssueCommand CreateCreateIssueCommand(Issue issue)
    {
        var currentUser = CurrentUserFactory.Create();

        return new CreateIssueCommand(
            Title: issue.Title,
            Description: issue.Description,
            Category: issue.Category,
            Urgency: issue.Urgency,
            Author: issue.Author,
            AuthorId: issue.AuthorId,
            DateTime: issue.DateTime);
    }
}

