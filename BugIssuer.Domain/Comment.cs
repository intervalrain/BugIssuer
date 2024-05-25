using BugIssuer.Domain.Common;

namespace BugIssuer.Domain;

public class Comment : Entity
{
    public int CommentId { get; }
    public int IssueId { get; }
    public string Content { get; set; }
    public string AuthorId { get; }
    public string Author { get; }
    public DateTime DateTime { get; }
    public DateTime LastUpdate { get; }

    public DateOnly Date => DateOnly.FromDateTime(DateTime);
    public DateOnly LastUpdateDate => DateOnly.FromDateTime(LastUpdate);


    public Comment(int id, int issueId, string authorId, string author, string content, DateTime dateTime)
        : base(Guid.NewGuid())
    {
        CommentId = id;
        IssueId = issueId;
        AuthorId = authorId;
        Author = author;
        Content = content;
        DateTime = dateTime;
        LastUpdate = dateTime;
    }
}