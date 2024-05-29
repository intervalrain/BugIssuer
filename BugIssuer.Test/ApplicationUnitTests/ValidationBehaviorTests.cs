using BugIssuer.Application.Common.Behaviors;

using ErrorOr;

using MediatR;

using NSubstitute;

using BugIssuer.Application.Issuer.Commands.CreateIssue;
using BugIssuer.Domain;
using BugIssuer.Test.Common;

using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;

namespace BugIssuer.Test.ApplicationUnitTests;

[TestClass]
public class ValidationBehaviorTests
{
    private ValidationBehavior<CreateIssueCommand, ErrorOr<Issue>> _validationBehavior;
    private IValidator<CreateIssueCommand> _mockValidator;
    private RequestHandlerDelegate<ErrorOr<Issue>> _mockNextBehavior;

    [TestInitialize]
    public void Setup()
    {
        _mockNextBehavior = Substitute.For<RequestHandlerDelegate<ErrorOr<Issue>>>();
        _mockValidator = Substitute.For<IValidator<CreateIssueCommand>>();

        _validationBehavior = new(_mockValidator);
    }

    [TestMethod]
    public async Task InvokeValidationBehavior_WhenValidatorResultIsValid_ShouldInvokeNextBehavior()
    {
        // Arrange
        var createIssueCommand = IssueCommandFactory.CreateCreateIssueCommand(IssueFactory.CreateIssue());
        var issue = IssueFactory.CreateIssue();

        _mockValidator
            .ValidateAsync(createIssueCommand, Arg.Any<CancellationToken>())
            .Returns(new ValidationResult());

        _mockNextBehavior.Invoke().Returns(issue);

        // Act
        var result = await _validationBehavior.Handle(createIssueCommand, _mockNextBehavior, default);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().BeEquivalentTo(issue);
    }

    [TestMethod]
    public async Task InvokeValidationBehavior_WhenValidationResultIsNotValid_ShouldReturnListOfErrors()
    {
        // Arrange
        var createIssueCommand = IssueCommandFactory.CreateCreateIssueCommand(IssueFactory.CreateIssue());
        List<ValidationFailure> validationFailures = new List<ValidationFailure>
        {
            new ValidationFailure(
                propertyName: "foo",
                errorMessage: "bad foo")
        };

        _mockValidator
            .ValidateAsync(createIssueCommand, Arg.Any<CancellationToken>())
            .Returns(new ValidationResult(validationFailures));

        // Act
        var result = await _validationBehavior.Handle(createIssueCommand, _mockNextBehavior, default);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Code.Should().Be("foo");
        result.FirstError.Description.Should().Be("bad foo");
    }

    [TestMethod]
    public async Task InvokeValidationBehavior_WhenNoValidator_ShouldInvokeNextBehavior()
    {
        // Arrange
        var createIssueCommand = IssueCommandFactory.CreateCreateIssueCommand(IssueFactory.CreateIssue());
        var validationBehavior = new ValidationBehavior<CreateIssueCommand, ErrorOr<Issue>>();

        var issue = IssueFactory.CreateIssue();
        _mockNextBehavior.Invoke().Returns(issue);

        // Act
        var result = await validationBehavior.Handle(createIssueCommand, _mockNextBehavior, default);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().Be(issue);
    }
}
