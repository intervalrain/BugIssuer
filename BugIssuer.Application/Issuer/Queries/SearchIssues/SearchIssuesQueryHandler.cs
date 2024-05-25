using System.Reflection;

using BugIssuer.Application.Common.Interfaces;
using BugIssuer.Domain;

using ErrorOr;

using MediatR;

namespace BugIssuer.Application.Issuer.Queries.SearchIssues;

public class SearchIssuesQueryHandler : IRequestHandler<SearchIssuesQuery, ErrorOr<List<Issue>>>
{
    private readonly IIssueRepository _issueRepository;

    public SearchIssuesQueryHandler(IIssueRepository issueRepository)
    {
        _issueRepository = issueRepository;
    }

    public async Task<ErrorOr<List<Issue>>> Handle(SearchIssuesQuery request, CancellationToken cancellationToken)
    {
        var issues = await _issueRepository.ListIssuesAsync(cancellationToken);

        if (issues is null)
        {
            return Error.Unexpected(description: "There are some unexpected errors happened. Please contact admin.");
        }
        return issues.Where(issue => Match(request.Text, issue)).ToList();
    }

    private bool Match(string text, Issue issue)
    {
        if (issue is null) return false;
        if (string.IsNullOrEmpty(text)) return true;

        var properties = issue.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var property in properties)
        {
            if (property.CanRead)
            {
                var value = property.GetValue(issue)?.ToString();
                if (!string.IsNullOrEmpty(value) && value.IndexOf(text, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return true;
                }
            }
        }

        return false;
    }
}