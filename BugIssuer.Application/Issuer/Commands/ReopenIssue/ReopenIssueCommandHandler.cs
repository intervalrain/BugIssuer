using BugIssuer.Application.Common.Interfaces.Persistence;

using ErrorOr;

using MediatR;

namespace BugIssuer.Application.Issuer.Commands.ReopenIssue;

public class ReopenIssueCommandHandler : IRequestHandler<ReopenIssueCommand, ErrorOr<Success>>
{
    private readonly IIssueRepository _issueRepository;

    public ReopenIssueCommandHandler(IIssueRepository issueRepository)
    {
        _issueRepository = issueRepository;
    }

    public async Task<ErrorOr<Success>> Handle(ReopenIssueCommand request, CancellationToken cancellationToken)
    {
        var issue = await _issueRepository.GetIssueByIdAsync(request.IssueId, cancellationToken);

        if (issue is null)
        {
            return Error.NotFound(description: "Issue not found.");
        }

        var result = issue.Reopen();

        if (result.IsError)
        {
            return result.Errors;
        }

        await _issueRepository.UpdateIssueAsync(issue, cancellationToken);

        return Result.Success;
    }
}