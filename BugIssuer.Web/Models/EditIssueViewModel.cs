namespace BugIssuer.Web.Models;

public class EditIssueViewModel
{
    public int IssueId { get; set; }
    public string Title { get; set; }
    public string Category { get; set; }
    public int Urgency { get; set; }
    public string Description { get; set; }
}

