using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using MediatR;

using BugIssuer.Application.Common.Interfaces;
using BugIssuer.Web.Models;
using System.Diagnostics;

namespace BugIssuer.Web.Controllers;

[Authorize]
[Route("")]
public class HomeController : ApiController
{
    public HomeController(ILogger<Controller> logger, IMediator mediator, IWebHostEnvironment environment, ICurrentUserProvider userProvider, IDateTimeProvider dateTimeProvider)
        : base(logger, mediator, environment, userProvider, dateTimeProvider)
    {
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
}

