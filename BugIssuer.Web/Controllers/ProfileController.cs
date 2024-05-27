using BugIssuer.Application.Common.Interfaces;
using BugIssuer.Application.Issuer.Queries.ListMyIssues;
using BugIssuer.Web.Extensions;
using BugIssuer.Web.Models;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace BugIssuer.Web.Controllers;

public class ProfileController : ApiController
{
    public ProfileController(ILogger<Controller> logger, IMediator mediator, IWebHostEnvironment environment, ICurrentUserProvider userProvider, IDateTimeProvider dateTimeProvider)
        : base(logger, mediator, environment, userProvider, dateTimeProvider)
    {
    }

    [HttpGet]
    public async Task<IActionResult> Profile(string sortOrder = null, string filterStatus = null)
    {
        var query = new ListMyIssuesQuery(CurrentUser.UserId, sortOrder, filterStatus);

        ViewData = StatusModeling.UpdateStatus(ViewData, sortOrder, filterStatus);

        var result = await Mediator.Send(query);

        if (result.IsError)
        {
            return Problem(result.Errors);
        }

        var issues = result.Value;

        var model = new ProfileViewModel(issues, CurrentUser);

        return View(model);
    }
}

