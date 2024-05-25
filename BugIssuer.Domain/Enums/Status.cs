namespace BugIssuer.Domain.Enums;

public enum Status
{
	Open,	
	Ongoing,
    Closed,
	Deleted
}

public static class StatusExtension
{
	public static string Badge(this Status status)
	{
		switch (status)
		{
			case Status.Open:
				return "badge-primary";
			case Status.Ongoing:
				return "badge-danger";
			case Status.Closed:
				return "badge-default";
			default:
				return "badge-info";
		}
	}
}