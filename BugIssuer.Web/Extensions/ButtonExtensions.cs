using BugIssuer.Domain.Enums;

namespace BugIssuer.Web.Extensions;

public static class Button
{
    public static string UrgencyToButton(int urgency)
    {
        string btnClass;
        switch (urgency)
        {
            case 1:
                btnClass = "btn-secondary";
                break;
            case 2:
                btnClass = "btn-success";
                break;
            case 3:
                btnClass = "btn-info";
                break;
            case 4:
                btnClass = "btn-warning";
                break;
            case 5:
                btnClass = "btn-danger";
                break;
            default:
                btnClass = "btn-secondary";
                break;
        }
        return btnClass;
    }
}