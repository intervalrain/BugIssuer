using BugIssuer.Domain.Common;
using BugIssuer.Domain.Enums;
using BugIssuer.Domain.Events;

using ErrorOr;

namespace BugIssuer.Domain;

public class Issue : Entity
{
	public int IssueId { get; }
	public string Title { get; private set; }
	public string Description { get; private set; }
	public string Category { get; private set; }
	public string AuthorId { get; }
	public string Author { get; }
	public int Urgency { get; private set; }
	public DateTime DateTime { get; }
	public DateTime LastUpdate { get; private set; } 
	public string Assignee { get; private set; }
	public Status Status { get; private set; }
	public Label Label { get; private set; } 
	public List<Comment> Comments { get; }

	public DateOnly Date => DateOnly.FromDateTime(DateTime);
    public DateOnly LastUpdateDate => DateOnly.FromDateTime(LastUpdate);

	public TimeOnly Time => TimeOnly.FromDateTime(DateTime);
	public TimeOnly LastUpdateTime => TimeOnly.FromDateTime(LastUpdate); 

    internal Issue(int id, string title, string category, string authorId, string author, string description, int urgency, DateTime dateTime)
		: base(Guid.NewGuid())
	{
		IssueId = id;
		Title = title;
		Category = category;
		AuthorId = authorId;
		Author = author;
		Description = description;
		Urgency = urgency;
		DateTime = dateTime;
		LastUpdate = dateTime;
		Assignee = string.Empty;
		Status = Status.Open;
		Label = Label.None;
		Comments = new List<Comment>();
	}

	private static int _issueCount = 0; 

	public static Issue Create(string title, string category, string authorId, string author, string description, int urgency, DateTime dateTime)
	{
		return new Issue(++_issueCount, title, category, authorId, author, description, urgency, dateTime);
	}

	public ErrorOr<Success> Delete()
	{

        if (Status == Status.Deleted)
        {
            return Error.NotFound(description: "The issue has already been deleted.");
        }

		if (Status == Status.Closed)
		{
			return Error.Unauthorized(description: "The issue has already been closed.");
		}

		Status = Status.Deleted;

		_domainEvents.Add(new IssueRemovedEvent(IssueId));

		return Result.Success;
	}

    public ErrorOr<Success> Update(string title, string description, string category, int urgency, DateTime dateTime)
    {
        if (Status == Status.Deleted)
        {
            return Error.NotFound(description: "The issue has already been deleted.");
        }

        if (Status == Status.Closed)
        {
            return Error.Unauthorized(description: "The issue has already been closed.");
        }

		Title = title;
		Description = description;
		Category = category;
		Urgency = urgency;
		LastUpdate = dateTime;

        _domainEvents.Add(new IssueUpdatedEvent(IssueId, title, description, category, urgency, dateTime));

        return Result.Success;
    }

	public ErrorOr<Success> AddComment(string authorId, string author, string content, DateTime dateTime)
	{
		if (Status == Status.Deleted)
		{
			return Error.NotFound(description: "The issue has already been deleted.");
		}

        var now = dateTime;
		Comments.Add(new Comment(Comments.Count + 1, IssueId, authorId, author, content, now));
		LastUpdate = dateTime;

		return Result.Success;
	}

	public ErrorOr<Success> Assign(string assignee)
	{
        if (Status == Status.Deleted)
		{
            return Error.NotFound("The issue has already been deleted");
        }
		Status = Status.Ongoing;
		Assignee = assignee;

		return Result.Success;
	}

	public ErrorOr<Success> Reopen()
	{
		if (Status == Status.Open || Status == Status.Ongoing)
		{
			return Error.Conflict($"The issue has already been open.");
		}
		 
		Status = string.IsNullOrEmpty(Assignee) ? Status.Open : Status.Ongoing;

		return Result.Success;
	}

	public ErrorOr<Success> Close()
	{
        if (Status == Status.Deleted)
        {
            return Error.NotFound(description: "The issue has already been deleted.");
        }

        if (Status == Status.Closed)
        {
            return Error.Conflict(description: "The issue has already been closed.");
        }

        Status = Status.Closed;

		return Result.Success;
	}

    public ErrorOr<Success> Unassign()
    {
        if (Status == Status.Deleted)
        {
            return Error.NotFound(description: "The issue has already been deleted.");
        }

        if (Status == Status.Closed)
        {
            return Error.Unauthorized(description: "The issue has already been closed.");
        }

		Status = Status.Open;
		Assignee = string.Empty;

		return Result.Success;
    }

	public ErrorOr<Success> LabelAs(Label label)
	{
        if (Status == Status.Deleted)
        {
            return Error.NotFound(description: "The issue has already been deleted.");
        }

        if (Status == Status.Closed)
        {
            return Error.Unauthorized(description: "The issue has already been closed.");
        }

		Label = label;

		return Result.Success;
    }
}