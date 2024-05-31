using BugIssuer.Application.Common.Interfaces;
using BugIssuer.Application.Common.Interfaces.Persistence;

using ErrorOr;

using MediatR;

namespace BugIssuer.Application.Issuer.Commands.UpdateIssue
{
    public class UpdateIssueCommandHandler : IRequestHandler<UpdateIssueCommand, ErrorOr<Success>>
    {
        private readonly IIssueRepository _issueRepository;
        private readonly IUserRepository _userRepository;
        private readonly IDateTimeProvider _dateTimeProvider;

        public UpdateIssueCommandHandler(IIssueRepository issueRepository, IUserRepository userRepository, IDateTimeProvider dateTimeProvider)
        {
            _issueRepository = issueRepository;
            _userRepository = userRepository;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<ErrorOr<Success>> Handle(UpdateIssueCommand request, CancellationToken cancellationToken)
        {
            var authorId = request.UserId;
            var issue = await _issueRepository.GetIssueByIdAsync(request.IssueId, cancellationToken);

            if (issue is null)
            {
                return Error.NotFound(description: "The issue is not found");
            }

            if (issue.AuthorId != authorId)
            {
                return Error.Forbidden(description: "You are not allowed to delete this issue.");
            }

            var result = issue.Update(request.Title, request.Description, request.Category, request.Urgency, _dateTimeProvider.Now);

            if (result.IsError)
            {
                return result.Errors;
            }

            await _issueRepository.UpdateIssueAsync(issue, cancellationToken);

            return Result.Success;
        }
    }
}

