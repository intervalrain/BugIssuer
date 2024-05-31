using BugIssuer.Application.Common.Interfaces.Persistence;

using ErrorOr;

using MediatR;

namespace BugIssuer.Application.Issuer.Commands.RemoveIssue;

public class RemoveIssueCommandHandler : IRequestHandler<RemoveIssueCommand, ErrorOr<Success>>
{
    private readonly IIssueRepository _issueRepository;
    private readonly IAdminProvider _adminProvider;

    public RemoveIssueCommandHandler(IIssueRepository issueRepository, IAdminProvider adminProvider)
    {
        _issueRepository = issueRepository;
        _adminProvider = adminProvider;
    }

    public async Task<ErrorOr<Success>> Handle(RemoveIssueCommand request, CancellationToken cancellationToken)
    {
        var applicant = request.UserId;
        var issue = await _issueRepository.GetIssueByIdAsync(request.IssueId, cancellationToken);

        if (issue is null)
        {
            return Error.NotFound(description: "The issue is not found.");
        }

        if (issue.AuthorId != applicant && !_adminProvider.IsAdmin(applicant))
        {
            return Error.Forbidden(description: "You are not allowed to delete this issue.");
        }

        var result = issue.Delete();

        if (result.IsError)
        {
            return result.Errors;
        }

        await _issueRepository.UpdateIssueAsync(issue, cancellationToken);

        return Result.Success;
    }
}

