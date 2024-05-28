using BugIssuer.Application.Common.Behaviors;
using BugIssuer.Application.Common.Interfaces;
using BugIssuer.Application.Common.Security.Request;
using BugIssuer.Infrastructure.Security;
using BugIssuer.Test.Common;

using ErrorOr;
using MediatR;
using NSubstitute;
using FluentAssertions;


namespace BugIssuer.Test.ApplicationUnitTests;

#region responses
public record Response()
{
    public static readonly Response Instance = new();
}
#endregion

#region requests
public record RequestWithNoAuthorizationAttribute(string UserId) : IAuthorizableRequest<ErrorOr<Response>>;

[Authorize(Permissions = "Permission")]
public record RequestWithSingleAuthorizationAttribute(string UserId) : IAuthorizableRequest<ErrorOr<Response>>;

[Authorize(Permissions = "Permission1,Permission2")]
[Authorize(Roles = "Role1,Role2")]
[Authorize(Policies = "Policy1,Policy2")]
[Authorize(Permissions = "Permission3", Roles = "Role3", Policies = "Policy3")]
public record RequestWithMultipleAuthorizationAttribute(string UserId) : IAuthorizableRequest<ErrorOr<Response>>;
#endregion

[TestClass]
public class AuthorizationBehaviorTests
{
    private IAuthorizationService _mockAuthorizationService;
    private ICurrentUserProvider _mockCurrentUserProvider;
    private IPolicyEnforcer _mockPolicyEnforcer;
    private RequestHandlerDelegate<ErrorOr<Response>> _mockNextBehavior;

    [TestInitialize]
    public void Initialize()
    {
        _mockAuthorizationService = Substitute.For<IAuthorizationService>();
        _mockCurrentUserProvider = Substitute.For<ICurrentUserProvider>();
        _mockPolicyEnforcer = Substitute.For<IPolicyEnforcer>();

        _mockCurrentUserProvider.CurrentUser.Returns(CurrentUserFactory.Create());
        var defaultCurrentUser = _mockCurrentUserProvider.CurrentUser;
        var defaultRequest = new RequestWithSingleAuthorizationAttribute(defaultCurrentUser.UserId);
        var defaultPolicy = string.Empty;

        _mockPolicyEnforcer
            .Authorize(defaultRequest, defaultCurrentUser, defaultPolicy)
            .Returns(Result.Success);

        _mockNextBehavior = Substitute.For<RequestHandlerDelegate<ErrorOr<Response>>>();
        _mockNextBehavior
            .Invoke()
            .Returns(Response.Instance);
    }

    [TestMethod]
    public void InvokeAuthorizationService_WhenUserHasPermission_ShouldBeAuthorized()
    {
        // Arrange
        _mockCurrentUserProvider.CurrentUser.Returns(CurrentUserFactory.Create(
            permissions: new List<string> { "Permission" }));
        var currentUser = _mockCurrentUserProvider.CurrentUser;
        var request = new RequestWithSingleAuthorizationAttribute(currentUser.UserId);
        var policy = string.Empty;

        _mockPolicyEnforcer
            .Authorize(request, currentUser, policy)
            .Returns(Result.Success);

        _mockAuthorizationService = new AuthorizationService(
            _mockPolicyEnforcer,
            _mockCurrentUserProvider);

        // Act
        var result = _mockAuthorizationService
            .AuthorizeCurrentUser(
                request: request,
                requiredRoles: new List<string>(),
                requiredPermissions: new List<string> { "Permission" },
                requiredPolicies: new List<string>()
            );

        // Assert
        result.IsError.Should().BeFalse();
    }

    [TestMethod]
    public void InvokeAuthorizationService_WhenUserHasNoPermission_ShouldBeUnauthorized()
    {
        // Arrange
        _mockCurrentUserProvider.CurrentUser.Returns(CurrentUserFactory.Create());
        var currentUser = _mockCurrentUserProvider.CurrentUser;
        var request = new RequestWithSingleAuthorizationAttribute(currentUser.UserId);
        var policy = string.Empty;

        _mockPolicyEnforcer
            .Authorize(request, currentUser, policy)
            .Returns(Result.Success);

        _mockAuthorizationService = new AuthorizationService(
            _mockPolicyEnforcer,
            _mockCurrentUserProvider);

        // Act
        var result = _mockAuthorizationService
            .AuthorizeCurrentUser(
                request: request,
                requiredRoles: new List<string>(),
                requiredPermissions: new List<string> { "Permission" },
                requiredPolicies: new List<string>()
            );

        // Assert
        result.IsError.Should().BeTrue();
    }

    [TestMethod]
    public async Task InvokeAuthorizationBehavior_WhenNoAuthorizationAttribute_ShouldInvokeNextBehavior()
    {
        // Arrange
        var currentUser = _mockCurrentUserProvider.CurrentUser;
        var request = new RequestWithNoAuthorizationAttribute(currentUser.UserId);

        var authorizationBehavior = new AuthorizationBehavior<RequestWithNoAuthorizationAttribute, ErrorOr<Response>>(_mockAuthorizationService);

        // Act
        var result = await authorizationBehavior.Handle(request, _mockNextBehavior, default);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().Be(Response.Instance);
    }

    [TestMethod]
    public async Task InvokeAuthorizationBehavior_WhenSingleAuthorizationAttributeAndUserIsAuthorized_ShouldInvokeNextBehavior()
    {
        // Arrange
        var currentUser = _mockCurrentUserProvider.CurrentUser;
        var request = new RequestWithSingleAuthorizationAttribute(currentUser.UserId);

        _mockAuthorizationService
            .AuthorizeCurrentUser(
                request: request,
                requiredRoles: new List<string>(),
                requiredPermissions: new List<string> { "Permission" },
                requiredPolicies: new List<string>()
            ).Returns(Result.Success);

        var authorizationBehavior = new AuthorizationBehavior<RequestWithSingleAuthorizationAttribute, ErrorOr<Response>>(_mockAuthorizationService);

        // Act
        var result = await authorizationBehavior.Handle(request, _mockNextBehavior, default);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().Be(Response.Instance);
    }

    [TestMethod]
    public async Task InvokeAuthorizationBehavior_WhenSingleAuthorizationAttributeAndUserIsNotAuthorized_ShouldReturnUnauthorized()
    {
        // Arrange
        var currentUser = _mockCurrentUserProvider.CurrentUser;
        var request = new RequestWithSingleAuthorizationAttribute(currentUser.UserId);

        var error = Error.Unauthorized(code: "bad.user", description: "bad user");

        _mockAuthorizationService = new AuthorizationService(_mockPolicyEnforcer, _mockCurrentUserProvider);

        var authorizationBehavior = new AuthorizationBehavior<RequestWithSingleAuthorizationAttribute, ErrorOr<Response>>(_mockAuthorizationService);

        // Act
        var result = await authorizationBehavior.Handle(request, _mockNextBehavior, default);

        // Assert
        result.IsError.Should().BeTrue();
    }

    [TestMethod]
    public async Task InvokeAuthorizationBehavior_WhenMultipleAuthorizationAttributesAndUserIsAuthorized_ShouldInvokeNextBehavior()
    {
        // Arrange
        //var polices = _mockPolicyEnforcer
        _mockCurrentUserProvider.CurrentUser.Returns(CurrentUserFactory.Create(
            permissions: new List<string> { "Permission1", "Permission2", "Permission3" },
            roles: new List<string> { "Role1", "Role2", "Role3" }));
        var currentUser = _mockCurrentUserProvider.CurrentUser;
        var request = new RequestWithMultipleAuthorizationAttribute(currentUser.UserId);

        _mockAuthorizationService = new AuthorizationService(_mockPolicyEnforcer, _mockCurrentUserProvider);
            
        var authorizationBehavior = new AuthorizationBehavior<RequestWithMultipleAuthorizationAttribute, ErrorOr<Response>>(_mockAuthorizationService);

        // Act
        var result = await authorizationBehavior.Handle(request, _mockNextBehavior, default);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().Be(Response.Instance);
    }

    [TestMethod]
    public async Task InvokeAuthorizationBehavior_WhenMultipleAuthorizationAttributesAndUserIsNotAuthorized_ShouldReturnUnauthorized()
    {
        // Arrange
        var currentUser = _mockCurrentUserProvider.CurrentUser;
        var request = new RequestWithMultipleAuthorizationAttribute(currentUser.UserId);

        _mockAuthorizationService = new AuthorizationService(_mockPolicyEnforcer, _mockCurrentUserProvider);

        var authorizationBehavior = new AuthorizationBehavior<RequestWithMultipleAuthorizationAttribute, ErrorOr<Response>>(_mockAuthorizationService);

        // Act
        var result = await authorizationBehavior.Handle(request, _mockNextBehavior, default);

        // Assert
        result.IsError.Should().BeTrue();
    }
}

