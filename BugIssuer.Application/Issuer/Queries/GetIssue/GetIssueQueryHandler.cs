using BugIssuer.Application.Common.Interfaces.Persistence;
using BugIssuer.Domain;
using BugIssuer.Domain.Enums;

using ErrorOr;

using MediatR;

namespace BugIssuer.Application.Issuer.Queries.GetIssue;

public class GetIssueQueryHandler : IRequestHandler<GetIssueQuery, ErrorOr<Issue>>
{
    private readonly IIssueRepository _issueRepository;
    private readonly IAdminProvider _adminProvider;

    public GetIssueQueryHandler(IIssueRepository issueRepository, IAdminProvider adminProvider)
    {
        _issueRepository = issueRepository;
        _adminProvider = adminProvider;
    }

    public async Task<ErrorOr<Issue>> Handle(GetIssueQuery request, CancellationToken cancellationToken)
    {
        var issue = await _issueRepository.GetIssueByIdAsync(request.IssueId, cancellationToken);

        if (issue is null)
        {
            return Error.NotFound(description: "Issue not found.");
        }

        if (issue.Status == Status.Deleted && (request.Applicant != issue.Author && !_adminProvider.IsAdmin(request.Applicant)))
        {
            return Error.NotFound(description: "Issue has been deleted.");
        }

        return issue;
    }
}