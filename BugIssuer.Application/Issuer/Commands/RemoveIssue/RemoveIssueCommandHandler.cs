using BugIssuer.Application.Common.Interfaces;

using ErrorOr;

using MediatR;

namespace BugIssuer.Application.Issuer.Commands.RemoveIssue;

public class RemoveIssueCommandHandler : IRequestHandler<RemoveIssueCommand, ErrorOr<Success>>
{
    private readonly IIssueRepository _issueRepository;

    public RemoveIssueCommandHandler(IIssueRepository issueRepository)
    {
        _issueRepository = issueRepository;
    }

    public async Task<ErrorOr<Success>> Handle(RemoveIssueCommand request, CancellationToken cancellationToken)
    {
        var authorId = request.AuthorId;
        var issue = await _issueRepository.GetIssueByIdAsync(request.IssueId, cancellationToken);

        if (issue is null)
        {
            return Error.NotFound(description: "The issue is not found.");
        }

        if (issue.AuthorId != authorId)
        {
            return Error.Forbidden(description: "You are not allowed to delete this issue.");
        }

        var result = issue.Remove();

        if (result.IsError)
        {
            return result.Errors;
        }

        await _issueRepository.UpdateIssueAsync(issue, cancellationToken);

        return Result.Success;
    }
}

