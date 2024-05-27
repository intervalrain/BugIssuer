using System.ComponentModel.DataAnnotations;

using BugIssuer.Domain;

namespace BugIssuer.Web.Models;

public class IssueViewModel
{
    [Required]
    public Issue Issue { get; set; }

    [Required]
    public bool IsAdmin { get; set; }
}

