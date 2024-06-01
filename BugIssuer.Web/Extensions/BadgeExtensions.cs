using BugIssuer.Domain.Enums;

namespace BugIssuer.Web.Extensions;

public static class Badge
{
    public static string UrgencyToBadge(int urgency)
    {
        string badgeClass = urgency switch
        {
            1 => "badge badge-secondary",
            2 => "badge badge-success",
            3 => "badge badge-info",
            4 => "badge badge-warning",
            5 => "badge badge-danger",
            _ => "badge badge-secondary",
        };
        return badgeClass;
    }

    public static string StatusToBadge(Status status)
    {
        string badgeClass = status switch
        {
            Status.Open => "badge badge-primary",
            Status.Ongoing => "badge badge-danger",
            Status.Closed => "badge badge-success",
            _ => "badge badge-default",
        };
        return badgeClass;
    }

    public static string LabelToBadge(Label label)
    {
        string badgeClass = label switch
        {
            Label.None => "badge badge-secondary",
            Label.NA => "badge badge-info",
            Label.CIP => "badge badge-primary",
            Label.Bug => "badge badge-danger",
            Label.Feature => "badge badge-warning",
            _ => "badge badge-default",
        };
        return badgeClass;
    }
}

