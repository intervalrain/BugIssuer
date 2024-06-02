using BugIssuer.Application.Common.Interfaces.Persistence;

using ErrorOr;
using MediatR;

namespace BugIssuer.Application.Issuer.Commands.AssignIssue;

public class AssignIssueCommandHandler : IRequestHandler<AssignIssueCommand, ErrorOr<Success>>
{
    private readonly IIssueRepository _issueRepository;

    public AssignIssueCommandHandler(IIssueRepository issueRepository)
    {
        _issueRepository = issueRepository;
    }

    public async Task<ErrorOr<Success>> Handle(AssignIssueCommand request, CancellationToken cancellationToken)
    {
        var issue = await _issueRepository.GetIssueByIdAsync(request.IssueId, cancellationToken);

        if (issue is null)
        {
            return Error.NotFound(description: "Issue not found.");
        }

        issue.Assign(request.Assignee);

        await _issueRepository.UpdateIssueAsync(issue, cancellationToken);

        return Result.Success;
    }
}