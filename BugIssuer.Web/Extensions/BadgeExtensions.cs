using BugIssuer.Domain.Enums;

namespace BugIssuer.Web.Extensions;

public static class Badge
{
    private const string Blue = "badge badge-primary";
    private const string Gray = "badge badge-secondary";
    private const string Green = "badge badge-success";
    private const string Red = "badge badge-danger";
    private const string Orange = "badge badge-warning";
    private const string Cyan = "badge badge-info";
    private const string White = "badge badge-light";
    private const string Black = "badge badge-dark";


    public static string UrgencyToBadge(int urgency)
    {
        string badgeClass = urgency switch
        {
            1 => Gray,
            2 => Green,
            3 => Cyan,
            4 => Orange,
            5 => Red,
            _ => Gray
        };
        return badgeClass;
    }

    public static string StatusToBadge(Status status)
    {
        string badgeClass = status switch
        {
            Status.Open => Blue,
            Status.Ongoing => Red,
            Status.Closed => Green,
            _ => Gray,
        };
        return badgeClass;
    }

    public static string LabelToBadge(Label label)
    {
        string badgeClass = label switch
        {
            Label.None => Black,
            Label.NA => White,
            Label.CIP => Blue,
            Label.Bug => Red,
            Label.Feature => Cyan,
            _ => Gray
        };
        return badgeClass;
    }
}

