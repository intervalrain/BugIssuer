using FluentValidation;

namespace BugIssuer.Application.Issuer.Commands.CreateIssue
{
    public class CreateIssueCommandValidator : AbstractValidator<CreateIssueCommand>
    {
        public CreateIssueCommandValidator()
        {
            RuleFor(x => x.Title).MinimumLength(2).MaximumLength(32);
            RuleFor(x => x.UserId.Length).Equal(8);
            RuleFor(x => x.Category).MinimumLength(2).MaximumLength(32);
            RuleFor(x => x.Description).MinimumLength(2).MaximumLength(1024);
            RuleFor(x => x.Urgency).GreaterThanOrEqualTo(1).LessThanOrEqualTo(5);
        }
    }
}

