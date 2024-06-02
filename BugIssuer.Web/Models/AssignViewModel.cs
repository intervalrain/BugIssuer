using System.ComponentModel.DataAnnotations;

namespace BugIssuer.Web.Models;

public class AssignViewModel
{
    [Required]
    public int IssueId { get; set; }
    public string Assignee { get; set; }
}

