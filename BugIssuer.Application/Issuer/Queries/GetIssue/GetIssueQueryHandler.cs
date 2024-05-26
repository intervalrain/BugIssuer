using BugIssuer.Application.Common.Interfaces;
using BugIssuer.Domain;
using BugIssuer.Domain.Enums;

using ErrorOr;

using MediatR;

namespace BugIssuer.Application.Issuer.Queries.GetIssue;

public class GetIssueQueryHandler : IRequestHandler<GetIssueQuery, ErrorOr<Issue>>
{
    private readonly IIssueRepository _issueRepository;

    public GetIssueQueryHandler(IIssueRepository issueRepository )
    {
        _issueRepository = issueRepository;
    }

    public async Task<ErrorOr<Issue>> Handle(GetIssueQuery request, CancellationToken cancellationToken)
    {
        var issue = await _issueRepository.GetIssueByIdAsync(request.IssueId, cancellationToken);

        if (issue is null || (issue.Status == Status.Deleted && request.AuthorId != issue.Author))
        {
            return Error.NotFound(description: "Issue not found.");
        }

        return issue;
    }
}