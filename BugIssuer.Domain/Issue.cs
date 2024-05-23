using BugIssuer.Domain.Common;
using BugIssuer.Domain.Enums;

namespace BugIssuer.Domain;

public class Issue : Entity
{
	public int Id { get; }
	public string Title { get; set; }
	public string Description { get; set; }
	public string Category { get; set; }
	public string Author { get; }
	public DateTime EventTime { get; }
	public DateTime LastUpdateTime { get; set; } 
	public string Assignee { get; set; }
	public Status Status { get; set; }
	public List<Comment> Comments { get; set; }

	public Issue(int id, string title, string category, string author, DateTime dateTime)
		: base(id)
	{
		Id = id;
		Title = title;
		Category = category;
		Author = author;
		EventTime = dateTime;
		LastUpdateTime = dateTime;
		Assignee = string.Empty;
		Status = Status.Waiting;
		Comments = new List<Comment>();
	}
}