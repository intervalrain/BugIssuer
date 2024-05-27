using BugIssuer.Application.Common.Interfaces.Persistence;
using BugIssuer.Domain;
using BugIssuer.Domain.Enums;

using ErrorOr;

using MediatR;

namespace BugIssuer.Application.Issuer.Queries.ListIssues;

public class ListIssuesQueryHandler : IRequestHandler<ListIssuesQuery, ErrorOr<List<Issue>>>
{
    private readonly IIssueRepository _issueRepository;

    public ListIssuesQueryHandler(IIssueRepository issueRepository)
    {
        _issueRepository = issueRepository;
    }

    public async Task<ErrorOr<List<Issue>>> Handle(ListIssuesQuery request, CancellationToken cancellationToken)
    {
        List<Issue>? issues = default;
        if (request.FilterStatus == "All" || string.IsNullOrEmpty(request.FilterStatus))
        {
            issues = await _issueRepository.ListIssuesAsync(cancellationToken);
        }
        else
        {
            var status = request.FilterStatus switch
            {
                "Open" => Status.Open,
                "Ongoing" => Status.Ongoing,
                "Closed" => Status.Closed,
                "Deleted" => Status.Deleted,
                _ => Status.Open 
            };
            issues = await _issueRepository.ListIssuesByStatusAsync(status, cancellationToken);
        }

        if (issues is null)
        {
            return Error.Unexpected(description: "There are some unexpected errors happened. Please contact admin.");
        }

        Sort(ref issues, request.SortOrder);

        return request.IsAdmin
            ? issues
            : issues.Where(issue => issue.Status != Status.Deleted).ToList();
    }

    private void Sort(ref List<Issue> issues, string sortOrder)
    {
        issues = sortOrder switch
        {
            "id_desc" => issues.OrderBy(i => i.IssueId).ToList(),
            "Category" => issues.OrderBy(i => i.Category).ToList(),
            "category_desc" => issues.OrderByDescending(i => i.Category).ToList(),
            "Title" => issues.OrderBy(i => i.Title).ToList(),
            "title_desc" => issues.OrderByDescending(i => i.Title).ToList(),
            "Author" => issues.OrderBy(i => i.Author).ToList(),
            "author_desc" => issues.OrderByDescending(i => i.Author).ToList(),
            "DateTime" => issues.OrderBy(i => i.DateTime).ToList(),
            "datetime_desc" => issues.OrderByDescending(i => i.DateTime).ToList(),
            "LastUpdate" => issues.OrderBy(i => i.LastUpdate).ToList(),
            "lastupdate_desc" => issues.OrderByDescending(i => i.LastUpdate).ToList(),
            "Comments" => issues.OrderBy(i => i.Comments.Count).ToList(),
            "comments_desc" => issues.OrderByDescending(i => i.Comments.Count).ToList(),
            "Assignee" => issues.OrderBy(i => i.Assignee).ToList(),
            "assignee_desc" => issues.OrderByDescending(i => i.Assignee).ToList(),
            "Urgency" => issues.OrderBy(i => i.Urgency).ToList(),
            "urgency_desc" => issues.OrderByDescending(i => i.Urgency).ToList(),
            "Status" => issues.OrderBy(i => i.Status).ToList(),
            "status_desc" => issues.OrderByDescending(i => i.Status).ToList(),
            _ => issues.OrderByDescending(i => i.IssueId).ToList(),
        };
    }
}

