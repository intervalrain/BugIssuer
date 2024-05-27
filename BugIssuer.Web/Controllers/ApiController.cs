using BugIssuer.Application.Common.Interfaces;
using BugIssuer.Application.Common.Security.Users;

using ErrorOr;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BugIssuer.Web.Controllers;

[ApiController]
[Authorize]
[Route("[Controller]")]
public class ApiController : Controller
{
    private readonly ILogger<Controller> _logger;
    private readonly IMediator _mediator;
    private readonly IWebHostEnvironment _environment;
    private readonly CurrentUser _currentUser;
    private readonly IDateTimeProvider _dateTimeProvider;

    public ILogger<Controller> Logger => _logger;
    public IMediator Mediator => _mediator;
    public IWebHostEnvironment Environment => _environment;
    public CurrentUser CurrentUser => _currentUser;
    public IDateTimeProvider DateTimeProvider => _dateTimeProvider;

    public ApiController(ILogger<Controller> logger, IMediator mediator, IWebHostEnvironment environment, ICurrentUserProvider userProvider, IDateTimeProvider dateTimeProvider)
    {
        _logger = logger;
        _mediator = mediator;
        _environment = environment;
        _currentUser = userProvider.CurrentUser;
        _dateTimeProvider = dateTimeProvider;
    }

    protected ActionResult Problem(List<Error> errors)
    {
        if (errors.Count is 0)
        {
            return Problem();
        }

        if (errors.All(error => error.Type == ErrorType.Validation))
        {
            return ValidationProblem(errors);
        }

        return Problem(errors[0]);
    }

    private ObjectResult Problem(Error error)
    {
        var statusCode = error.Type switch
        {
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Unauthorized => StatusCodes.Status403Forbidden,
            _ => StatusCodes.Status500InternalServerError
        };

        return Problem(statusCode: statusCode, title: error.Description);
    }

    private ActionResult ValidationProblem(List<Error> errors)
    {
        var modelStateDictionary = new ModelStateDictionary();

        errors.ForEach(error => modelStateDictionary.AddModelError(error.Code, error.Description));

        return ValidationProblem(modelStateDictionary);
    }
}

