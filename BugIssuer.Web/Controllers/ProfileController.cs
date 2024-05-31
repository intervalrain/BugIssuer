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
    private readonly IMediator _mediator;
    private readonly CurrentUser _currentUser;

    public ProfileController(IMediator mediator, ICurrentUserProvider currentUserProvider)
    {
        _mediator = mediator;
        _currentUser = currentUserProvider.CurrentUser;
    }

    [HttpGet]
    public async Task<IActionResult> Profile(string sortOrder = null, string filterStatus = null)
    {
        var query = new ListMyIssuesQuery(_currentUser.UserId, sortOrder, filterStatus);
        var result = await _mediator.Send(query);

        return result.Match(
            issues => View(ToViewModel(issues, _currentUser)),
            Problem);
    }

    private ProfileViewModel ToViewModel(List<Issue> issues, CurrentUser currentUser)
    {
        return new ProfileViewModel(issues, currentUser);
    }
}

