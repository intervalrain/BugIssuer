using BugIssuer.Application.Common.Interfaces;
using BugIssuer.Domain;
using BugIssuer.Domain.Enums;

using ErrorOr;

using MediatR;

namespace BugIssuer.Application.Issuer.Queries.ListIssues;

public class ListIssueQueryHandler : IRequestHandler<ListIssueQuery, ErrorOr<List<Issue>>>
{
    private readonly IIssueRepository _issueRepository;

    public ListIssueQueryHandler(IIssueRepository issueRepository)
    {
        _issueRepository = issueRepository;
    }

    public async Task<ErrorOr<List<Issue>>> Handle(ListIssueQuery request, CancellationToken cancellationToken)
    {
        var issues = await _issueRepository.ListIssuesAsync(cancellationToken);

        return issues.Where(issue => issue.Status != Status.Removed).ToList();
    }
}

