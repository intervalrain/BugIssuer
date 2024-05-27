using BugIssuer.Application.Common.Interfaces;
using BugIssuer.Application.Common.Interfaces.Persistence;
using BugIssuer.Domain;

using ErrorOr;

using MediatR;

namespace BugIssuer.Application.Issuer.Commands.NewComment;

public class NewCommentCommandHandler : IRequestHandler<NewCommentCommand, ErrorOr<Comment>>
{
    private readonly IIssueRepository _issueRepository;
    private readonly IDateTimeProvider _dateTimeProvider;

    public NewCommentCommandHandler(IIssueRepository issueRepository, IDateTimeProvider dateTimeProvider)
    {
        _issueRepository = issueRepository;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<ErrorOr<Comment>> Handle(NewCommentCommand request, CancellationToken cancellationToken)
    {
        var issue = await _issueRepository.GetIssueByIdAsync(request.IssueId, cancellationToken);

        if (issue is null)
        {
            return Error.NotFound(description: "Issue not found.");
        }

        if (issue.IssueId != request.IssueId)
        {
            return Error.Conflict(description: "Invalid Operation");
        }

        var result = issue.AddComment(
            request.AuthorId,
            request.Author,
            request.Content,
            _dateTimeProvider.Now
            );

        if (result.IsError)
        {
            return result.Errors;
        }

        await _issueRepository.UpdateIssueAsync(issue, cancellationToken);

        return issue.Comments.Last();
    }
}