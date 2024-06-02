using BugIssuer.Application.Common.Interfaces.Persistence;

using ErrorOr;

using MediatR;

namespace BugIssuer.Application.Issuer.Commands.CloseIssue;

public class CloseIssueCommandHandler : IRequestHandler<CloseIssueCommand, ErrorOr<Success>>
{
    private readonly IIssueRepository _issueRepository;

    public CloseIssueCommandHandler(IIssueRepository issueRepository)
    {
        _issueRepository = issueRepository;
    }

    public async Task<ErrorOr<Success>> Handle(CloseIssueCommand request, CancellationToken cancellationToken)
    {
        var issue = await _issueRepository.GetIssueByIdAsync(request.IssueId, cancellationToken);

        if (issue is null)
        {
            return Error.NotFound(description: "Issue not found.");
        }

        var result = issue.Close();

        if (result.IsError)
        {
            return result.Errors;
        }

        await _issueRepository.UpdateIssueAsync(issue, cancellationToken);

        return Result.Success;
    }
}

