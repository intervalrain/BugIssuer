using BugIssuer.Application.Common.Interfaces;

using FluentValidation;

namespace BugIssuer.Application.Issuer.Commands.CreateIssue
{
    public class CreateIssueCommandValidator : AbstractValidator<CreateIssueCommand>
    {
        public CreateIssueCommandValidator(IDateTimeProvider dateTimeProvider)
        {
            RuleFor(x => x.Author).MinimumLength(2).MaximumLength(10);
            RuleFor(x => x.Category).MinimumLength(2).MaximumLength(15);
            RuleFor(x => x.Description).MaximumLength(300);
        }
    }
}

