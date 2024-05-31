namespace BugIssuer.Web.Models;

public class NewIssueViewModel
{
    public string Title { get; set; }
    public string Category { get; set; }
    public int Urgency { get; set; }
    public string Description { get; set; }
}

