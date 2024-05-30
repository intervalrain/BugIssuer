using BugIssuer.Domain;

namespace BugIssuer.Test.Common;

public static class IssueFactory
{
    public static IssueConstants CreateInfo()
    {
        return new IssueConstants
        {
            Title = StringGenerator.GenerateRandomString(5, 12),
            Category = StringGenerator.GenerateRandomString(5, 12),
            Description = StringGenerator.GenerateContent(),
            Urgency = new Random().Next(1, 6),
            DateTime = DateTime.Now
        };
    }
    public static Issue CreateIssue()
    {
        var user = CurrentUserFactory.Create();
        var info = CreateInfo();
        return Issue.Create(
            title: info.Title,
            category: info.Category,
            authorId: user.UserId,
            author: user.UserName,
            description: info.Description,
            urgency: info.Urgency,
            dateTime: info.DateTime);
    }
}

