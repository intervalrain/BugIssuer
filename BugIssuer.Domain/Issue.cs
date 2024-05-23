using BugIssuer.Domain.Common;
using BugIssuer.Domain.Enums;
using BugIssuer.Domain.Events;

using ErrorOr;

namespace BugIssuer.Domain;

public class Issue : Entity
{
	public int Id { get; }
	public string Title { get; set; }
	public string Description { get; set; }
	public string Category { get; set; }
	public string AuthorId { get; }
	public string Author { get; }
	public DateTime EventTime { get; }
	public DateTime LastUpdateTime { get; set; } 
	public string Assignee { get; set; }
	public Status Status { get; set; }
	public List<Comment> Comments { get; set; }

	public Issue(int id, string title, string category, string authorId, string author, DateTime dateTime)
		: base(id)
	{
		Id = id;
		Title = title;
		Category = category;
		AuthorId = authorId;
		Author = author;
		EventTime = dateTime;
		LastUpdateTime = dateTime;
		Assignee = string.Empty;
		Status = Status.Waiting;
		Comments = new List<Comment>();
	}

	public ErrorOr<Success> Remove()
	{
		if (Status != Status.Waiting)
		{
			return Error.Forbidden(description: "The issue has already been accepted.");
		}

		if (Status == Status.Removed)
		{
            return Error.Conflict(description: "The issue has already been deleted.");
        }

		if (Comments.Any())
		{
			return Error.Forbidden(description: "The issue cannot be deleted because there are some comments.");
		}
		Status = Status.Removed;

		_domainEvents.Add(new IssueRemovedEvent(Id));

		return Result.Success;
	}

    public ErrorOr<Success> Update(string title, string description, string category)
    {
        throw new NotImplementedException();
    }
}