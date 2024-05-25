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
	public DateTime DateTime { get; }
	public DateTime LastUpdate { get; private set; } 
	public string Assignee { get; private set; }
	public Status Status { get; private set; }
	public List<Comment> Comments { get; }

	public DateOnly Date => DateOnly.FromDateTime(DateTime);
    public DateOnly LastUpdateDate => DateOnly.FromDateTime(LastUpdate);

	public TimeOnly Time => TimeOnly.FromDateTime(DateTime);
	public TimeOnly LastUpdateTime => TimeOnly.FromDateTime(LastUpdate); 

    internal Issue(int id, string title, string category, string authorId, string author, string description, DateTime dateTime)
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

	private static int _issueCount = 0; 

	public static Issue Create(string title, string category, string authorId, string author, string description)
	{
		return new Issue(++_issueCount, title, category, authorId, author, description, DateTime.UtcNow);
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
		LastUpdate = DateTime.UtcNow;

        _domainEvents.Add(new IssueUpdatedEvent(IssueId, title, description, category, LastUpdate));

        return Result.Success;
    }

	public ErrorOr<Success> AddComment(string authorId, string author, string content)
	{
		if (Status == Status.Deleted)
		{
			return Error.NotFound(description: "The issue has already been deleted.");
		}
		Comments.Add(new Comment(Comments.Count + 1, IssueId, authorId, author, content, DateTime.UtcNow));

		return Result.Success;
	}

	public ErrorOr<Success> Assign(string assignee)
	{
        if (Status != Status.Open && Status != Status.Ongoing)
		{
            return Error.Conflict("Please reopen the issue before accept the issue.");
        }
		Status = Status.Ongoing;
		Assignee = assignee;

		return Result.Success;
	}

	public ErrorOr<Success> Reopen()
	{
		if (Status != Status.Closed)
		{
			return Error.Conflict($"{Status} cannot be reopen.");
		}
		
		Status = string.IsNullOrEmpty(Assignee) ? Status.Open : Status.Ongoing;

		return Result.Success;
	}

	public ErrorOr<Success> Close()
	{
		Status = Status.Closed;

		return Result.Success;
	}
}