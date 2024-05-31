using BugIssuer.Application.Common.Interfaces;
using BugIssuer.Application.Common.Interfaces.Persistence;
using BugIssuer.Domain;

using ErrorOr;

using MediatR;

namespace BugIssuer.Application.Issuer.Commands.CreateIssue;

public class CreateIssueCommandHandler : IRequestHandler<CreateIssueCommand, ErrorOr<Issue>>
{
    private readonly IIssueRepository _issueRepository;
    private readonly IUserRepository _userRepository;

    public CreateIssueCommandHandler(IIssueRepository issueRepository, IUserRepository userRepository)
    {
        _issueRepository = issueRepository;
        _userRepository = userRepository;
    }

    public async Task<ErrorOr<Issue>> Handle(CreateIssueCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByIdAsync(request.UserId, cancellationToken);

        if (user is null)
        {
            return Error.NotFound(description: "User not found.");
        }

        var issue = Issue.Create(
            request.Title,
            request.Category,
            request.UserId,
            user.UserName,
            request.Description,
            request.Urgency,
            request.DateTime);

        await _issueRepository.AddIssueAsync(issue, cancellationToken);

        return issue;
    }
}

