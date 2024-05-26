using System.ComponentModel.DataAnnotations;

namespace BugIssuer.Web.Models;

public class NewCommentViewModel
{
    [Required]
    public int IssueId { get; set; }

    [Required]
    public string CommentContent { get; set; }
}

