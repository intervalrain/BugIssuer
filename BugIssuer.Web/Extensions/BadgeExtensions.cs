using BugIssuer.Domain.Enums;

namespace BugIssuer.Web.Extensions;

public static class Badge
{
    public static string UrgencyToBadge(int urgency)
    {
        string badgeClass;
        switch (urgency)
        {
            case 1:
                badgeClass = "badge badge-secondary";
                break;
            case 2:
                badgeClass = "badge badge-success";
                break;
            case 3:
                badgeClass = "badge badge-info";
                break;
            case 4:
                badgeClass = "badge badge-warning";
                break;
            case 5:
                badgeClass = "badge badge-danger";
                break;
            default:
                badgeClass = "badge badge-secondary";
                break;
        }
        return badgeClass;
    }

    public static string StatusToBadge(Status status)
    {
        string badgeClass;
        switch (status)
        {
            case Status.Open:
                badgeClass = "badge badge-primary";
                break;
            case Status.Ongoing:
                badgeClass = "badge badge-danger";
                break;
            case Status.Closed:
                badgeClass = "badge badge-default";
                break;
            default:
                badgeClass = "badge badge-info";
                break;
        }

        return badgeClass;
    }
}

