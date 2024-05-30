using BugIssuer.Application.Common.Interfaces;
using BugIssuer.Application.Common.Security.Users;
using BugIssuer.Application.Issuer.Queries.ListMyIssues;
using BugIssuer.Domain;
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

        var result = await Mediator.Send(query);

        return result.Match(
            issues => View(ToViewModel(issues, CurrentUser)),
            Problem);
    }

    private ProfileViewModel ToViewModel(List<Issue> issues, CurrentUser currentUser)
    {
        return new ProfileViewModel(issues, currentUser);
    }
}

