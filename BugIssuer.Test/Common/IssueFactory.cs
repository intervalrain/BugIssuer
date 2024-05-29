using BugIssuer.Domain;

namespace BugIssuer.Test.Common;

public static class IssueFactory
{
    public static Issue CreateIssue()
    {
        var title = StringGenerator.GenerateRandomString(5, 12);
        var cateogry = StringGenerator.GenerateRandomString(5, 12);
        var description = StringGenerator.GenerateContent();
        var currentUser = CurrentUserFactory.Create();
        var urgency = new Random().Next(1, 6);
        return Issue.Create(title, cateogry, currentUser.UserId, currentUser.UserName, description, urgency, DateTime.UtcNow);
    }
}

