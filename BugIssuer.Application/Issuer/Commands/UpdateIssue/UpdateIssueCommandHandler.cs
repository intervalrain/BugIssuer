using BugIssuer.Application.Common.Interfaces;

using ErrorOr;

using MediatR;

namespace BugIssuer.Application.Issuer.Commands.UpdateIssue
{
    public class UpdateIssueCommandHandler : IRequestHandler<UpdateIssueCommand, ErrorOr<Success>>
    {
        private readonly IIssueRepository _issueRepository;

        public UpdateIssueCommandHandler(IIssueRepository issueRepository)
        {
            _issueRepository = issueRepository;
        }

        public async Task<ErrorOr<Success>> Handle(UpdateIssueCommand request, CancellationToken cancellationToken)
        {
            var authorId = request.AuthorId;
            var issue = await _issueRepository.GetIssueByIdAsync(request.IssueId, cancellationToken);

            if (issue.AuthorId != authorId)
            {
                return Error.Forbidden(description: "You are not allowed to delete this issue.");
            }

            var result = issue.Update(request.Title, request.Description, request.Category);

            if (result.IsError)
            {
                return result.Errors;
            }

            await _issueRepository.UpdateIssueAsync(issue, cancellationToken);

            return Result.Success;
        }
    }
}

