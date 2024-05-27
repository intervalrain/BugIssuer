using BugIssuer.Application.Common.Interfaces;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace BugIssuer.Web.Controllers;

public class OverviewController : ApiController
{
    public OverviewController(ILogger<Controller> logger, IMediator mediator, IWebHostEnvironment environment, ICurrentUserProvider userProvider, IDateTimeProvider dateTimeProvider)
        : base(logger, mediator, environment, userProvider, dateTimeProvider)
    {
    }

    [HttpGet]
    public IActionResult Overview()
    {
        return View();
    }
}

