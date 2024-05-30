using BugIssuer.Domain;

namespace BugIssuer.Web.Models;

public class IssuesViewModel
{
    public bool IsAdmin { get; set; }
    public IEnumerable<Issue> Issues { get; set; }
    public string SortOrder { get; set; }
    public string FilterStatus { get; set; }
}

