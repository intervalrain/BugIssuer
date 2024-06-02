using BugIssuer.Application.Common.Interfaces.Persistence;
using BugIssuer.Domain.Enums;

using ErrorOr;

using MediatR;

namespace BugIssuer.Application.Issuer.Commands.LabelIssue;

public class LabelIssueCommandHandler : IRequestHandler<LabelIssueCommand, ErrorOr<Success>>
{
    private readonly IIssueRepository _issueRepository;

    public LabelIssueCommandHandler(IIssueRepository issueRepository)
    {
        _issueRepository = issueRepository;
    }

    public async Task<ErrorOr<Success>> Handle(LabelIssueCommand request, CancellationToken cancellationToken)
    {
        var issue = await _issueRepository.GetIssueByIdAsync(request.IssueId, cancellationToken);

        if (issue is null)
        {
            return Error.NotFound(description: "Issue not found.");
        }

        issue.LabelAs(ToLabel(request.Label));

        await _issueRepository.UpdateIssueAsync(issue, cancellationToken);

        return Result.Success;
    }

    private Label ToLabel(string label)
    {
        Label result = label switch
        {
            "None" => Label.None,
            "NA" => Label.NA,
            "CIP" => Label.CIP,
            "Bug" => Label.Bug,
            "Feature" => Label.Feature,
            _ => Label.None 
        };
        return result;
    }
}

