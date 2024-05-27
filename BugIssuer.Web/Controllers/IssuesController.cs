using BugIssuer.Application.Common.Interfaces;
using BugIssuer.Application.Issuer.Commands.CreateIssue;
using BugIssuer.Application.Issuer.Commands.NewComment;
using BugIssuer.Application.Issuer.Commands.RemoveIssue;
using BugIssuer.Application.Issuer.Commands.UpdateIssue;
using BugIssuer.Application.Issuer.Queries.GetIssue;
using BugIssuer.Application.Issuer.Queries.ListIssues;
using BugIssuer.Application.Issuer.Queries.SearchIssues;
using BugIssuer.Domain;
using BugIssuer.Web.Extensions;
using BugIssuer.Web.Models;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace BugIssuer.Web.Controllers;

public class IssuesController : ApiController
{
    public IssuesController(ILogger<Controller> logger, IMediator mediator, IWebHostEnvironment environment, ICurrentUserProvider userProvider, IDateTimeProvider dateTimeProvider)
        : base(logger, mediator, environment, userProvider, dateTimeProvider)
    {
    }

    [HttpGet]
    public async Task<IActionResult> Issues(string sortOrder = null, string filterStatus = null)
    {
        var query = new ListIssuesQuery(sortOrder, filterStatus, CurrentUser.IsAdmin());

        ViewData = StatusModeling.UpdateStatus(ViewData, sortOrder, filterStatus);

        var result = await Mediator.Send(query);

        if (result.IsError)
        {
            return Problem(result.Errors);
        }

        var issues = result.Value;

        return View(issues);
    }

    [HttpGet("Issue/{id:int}")]
    public IActionResult Issue(int id)
    {
        var authorId = CurrentUser.UserId;

        var query = new GetIssueQuery(authorId, id);

        var result = Mediator.Send(query).GetAwaiter().GetResult();

        if (result.IsError)
        {
            return Problem(result.Errors);
        }
        var issue = result.Value;

        var model = new IssueViewModel
        {
            Issue = issue,
            IsAdmin = CurrentUser.IsAdmin()
        };

        return View(model);
    }

    [HttpGet("NewIssue")]
    public IActionResult NewIssue()
    {
        return View();
    }

    [HttpPost("NewIssue")]
    public async Task<IActionResult> NewIssue([FromBody] NewIssueViewModel model)
    {
        try
        {
            if (ModelState.IsValid)
            {
                var command = new CreateIssueCommand(model.Title, model.Description, model.Category, model.Urgency, CurrentUser.UserId, CurrentUser.UserName, DateTimeProvider.Now);

                var result = await Mediator.Send(command);

                if (result.IsError)
                {
                    return Problem(result.Errors);
                }

                var issue = result.Value;
                var redirectUrl = Url.Action(nameof(Issues));

                return Ok(new { redirectUrl }); 
            }
            return BadRequest(ModelState);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "An error occurred while posting issues.");
            return StatusCode(500, "Internal server error");
        }
    }

    public IActionResult IssuesPartial(IEnumerable<Issue> issues)
    {
        return PartialView("_IssuesPartial", issues);
    }



    [HttpGet("EditIssue/{id:int}")]
    public async Task<IActionResult> EditIssue(int id)
    {
        var query = new GetIssueQuery(CurrentUser.UserId, id);

        var result = await Mediator.Send(query);

        if (result.IsError)
        {
            return Problem();
        }

        var issue = result.Value;
        var model = new EditIssueViewModel
        {
            IssueId = issue.IssueId,
            Title = issue.Title,
            Category = issue.Category,
            Urgency = issue.Urgency,
            Description = issue.Description
        };

        return View(model);
    }

    [HttpPost("EditIssue")]
    public async Task<IActionResult> EditIssue(EditIssueViewModel model)
    {
        if (ModelState.IsValid)
        {
            var query = new GetIssueQuery(CurrentUser.UserId, model.IssueId);

            var result = await Mediator.Send(query);

            if (result.IsError)
            {
                return Problem();
            }
            var issue = result.Value;

            var command = new UpdateIssueCommand(
                issue.IssueId,
                issue.AuthorId,
                model.Title,
                model.Description,
                model.Category,
                model.Urgency
                );
            var response = await Mediator.Send(command);
            return RedirectToAction(nameof(Issue), new { id = model.IssueId });
        }
        return View(model);
    }

    [HttpPost("DeleteIssue")]
    public async Task<IActionResult> DeleteIssue([FromBody] DeleteIssueRequest request)
    {
        try
        {
            var command = new RemoveIssueCommand(CurrentUser.UserId, request.IssueId);

            var result = await Mediator.Send(command);

            if (result.IsError)
            {
                return Problem(detail: result.FirstError.Description);
            }
            return RedirectToAction(nameof(Issues));
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "An error occurred while deleting issues.");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost("NewComment")]
    public async Task<IActionResult> NewComment(NewCommentViewModel model)
    {
        try
        {
            if (ModelState.IsValid)
            {
                var command = new NewCommentCommand(
                    model.IssueId,
                    CurrentUser.UserId,
                    CurrentUser.UserName,
                    model.CommentContent
                    );

                var result = await Mediator.Send(command);

                if (result.IsError)
                {
                    return Problem(result.Errors);
                }

                var comment = result.Value;
                var redirectUrl = Url.Action(nameof(Issue), new { id = model.IssueId });

                return Ok(new { redirectUrl });
            }
            return BadRequest(ModelState);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "An error occurred while posting issues.");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("SearchIssues")]
    public async Task<IActionResult> SearchIssues(string searchText, CancellationToken token)
    {
        try
        {
            var query = new SearchIssuesQuery(searchText);

            var issues = await Mediator.Send(query, token);
            return PartialView("_IssueListPartial", issues.Value);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "An error occurred while searching issues.");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost]
    public async Task<IActionResult> UploadImage(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return Json(new { success = false, message = "No file selected" });

        var uploadsFolder = Path.Combine(Environment.WebRootPath, "uploads");
        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        var filePath = Path.Combine(uploadsFolder, fileName);

        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(fileStream);
        }

        var fileUrl = Url.Content("~/uploads/" + fileName);
        return Json(new { success = true, url = fileUrl });
    }
}