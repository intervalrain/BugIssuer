using BugIssuer.Application.Common.Interfaces;
using BugIssuer.Domain;

using ErrorOr;

using MediatR;

namespace BugIssuer.Application.Issuer.Commands.CreateIssue;

public class CreateIssueCommandHandler : IRequestHandler<CreateIssueCommand, ErrorOr<Issue>>
{
    private readonly IIssueRepository _issueRepository;
    private readonly IDateTimeProvider _dateTimeProvider;

    public CreateIssueCommandHandler(IIssueRepository issueRepository, IDateTimeProvider dateTimeProvider)
    {
        _issueRepository = issueRepository;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<ErrorOr<Issue>> Handle(CreateIssueCommand request, CancellationToken cancellationToken)
    {
        int issueId = await _issueRepository.CountIssuesAsync(cancellationToken) + 1;

        var issue = new Issue(
            issueId,
            request.Title,
            request.Category,
            request.AuthorId,
            request.Author,
            _dateTimeProvider.UtcNow);

        await _issueRepository.AddIssueAsync(issue, cancellationToken);

        return issue;
    }
}

