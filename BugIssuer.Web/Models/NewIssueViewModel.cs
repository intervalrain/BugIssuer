using System.ComponentModel.DataAnnotations;

namespace BugIssuer.Web.Models;

public class NewIssueViewModel
{
    [Required]
    public string Title { get; set; }

    [Required]
    public string Category { get; set; }

    [Required]
    [Range(1, 5)]
    public int Urgency { get; set; }

    [Required]
    public string Description { get; set; }
}

