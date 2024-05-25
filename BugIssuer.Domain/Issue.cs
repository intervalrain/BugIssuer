using BugIssuer.Domain.Common;
using BugIssuer.Domain.Enums;
using BugIssuer.Domain.Events;

using ErrorOr;

namespace BugIssuer.Domain;

public class Issue : Entity
{
	public int IssueId { get; }
	public string Title { get; set; }
	public string Description { get; set; }
	public string Category { get; set; }
	public string AuthorId { get; }
	public string Author { get; }
	public DateTime DateTime { get; }
	public DateTime LastUpdate { get; set; } 
	public string Assignee { get; set; }
	public Status Status { get; set; }
	public List<Comment> Comments { get; set; }

	public DateOnly Date => DateOnly.FromDateTime(DateTime);
    public DateOnly LastUpdateDate => DateOnly.FromDateTime(LastUpdate);

    public Issue(int id, string title, string category, string authorId, string author, string description, DateTime dateTime)
		: base(Guid.NewGuid())
	{
		IssueId = id;
		Title = title;
		Category = category;
		AuthorId = authorId;
		Author = author;
		Description = description;
		DateTime = dateTime;
		LastUpdate = dateTime;
		Assignee = string.Empty;
		Status = Status.Open;
		Comments = new List<Comment>();
	}

	public ErrorOr<Success> Remove()
	{

        if (Status == Status.Deleted)
        {
            return Error.Conflict(description: "The issue has already been deleted.");
        }

		if (Status != Status.Open)
		{
			return Error.Forbidden(description: "The issue has already been accepted.");
		}

		if (Comments.Any())
		{
			return Error.Forbidden(description: "The issue cannot be deleted because there are some comments.");
		}
		Status = Status.Deleted;

		_domainEvents.Add(new IssueRemovedEvent(IssueId));

		return Result.Success;
	}

    public ErrorOr<Success> Update(string title, string description, string category)
    {
        if (Status == Status.Deleted)
        {
            return Error.Conflict(description: "The issue has already been deleted.");
        }

        if (Status != Status.Open)
        {
            return Error.Forbidden(description: "The issue has already been accepted.");
        }

		Title = title;
		Description = description;
		Category = category;

        _domainEvents.Add(new IssueUpdatedEvent(IssueId, title, description, category));

        return Result.Success;
    }
}