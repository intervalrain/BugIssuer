namespace BugIssuer.Domain;

public class Comment
{
	public int IssueId { get; set; }
	public int Id { get; set; }
	public string Content { get; set; }
	public string Author { get; set; }
	public DateTime EventTime { get; set; }
}

