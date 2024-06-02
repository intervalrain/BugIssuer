using BugIssuer.Application.Common.Interfaces;
using BugIssuer.Application.Common.Security.Users;
using BugIssuer.Application.Issuer.Commands.AssignIssue;
using BugIssuer.Application.Issuer.Commands.CloseIssue;
using BugIssuer.Application.Issuer.Commands.CreateIssue;
using BugIssuer.Application.Issuer.Commands.LabelIssue;
using BugIssuer.Application.Issuer.Commands.NewComment;
using BugIssuer.Application.Issuer.Commands.RemoveIssue;
using BugIssuer.Application.Issuer.Commands.ReopenIssue;
using BugIssuer.Application.Issuer.Commands.UpdateIssue;
using BugIssuer.Application.Issuer.Queries.GetIssue;
using BugIssuer.Application.Issuer.Queries.ListIssues;
using BugIssuer.Domain;
using BugIssuer.Web.Models;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace BugIssuer.Web.Controllers;

public class IssuesController : ApiController
{
    private readonly IMediator _mediator;
    private readonly IWebHostEnvironment _environment;
    private readonly CurrentUser _currentUser;
    private readonly IDateTimeProvider _dateTimeProvider;

    public IssuesController(IMediator mediator, IWebHostEnvironment environment, ICurrentUserProvider userProvider, IDateTimeProvider dateTimeProvider)
    {
        _mediator = mediator;
        _environment = environment;
        _currentUser = userProvider.CurrentUser;
        _dateTimeProvider = dateTimeProvider;
    }

    [HttpGet]
    public async Task<IActionResult> Issues(string sortOrder = null, string filterStatus = null)
    {
        var query = new ListIssuesQuery(_currentUser.UserId, sortOrder, filterStatus, _currentUser.IsAdmin());
        var result = await _mediator.Send(query);

        return result.Match(
            issues => View(new IssuesViewModel
            {
                IsAdmin = _currentUser.IsAdmin(),
                Issues = issues,
                SortOrder = sortOrder,
                FilterStatus = filterStatus
            }),
            Problem);
    }

    [HttpGet("Issue/{id:int}")]
    public async Task<IActionResult> Issue(int id)
    {
        var query = new GetIssueQuery(_currentUser.UserId, id);
        var result = await _mediator.Send(query);

        return result.Match(
            issue => View(new IssueViewModel
            {
                IsAdmin = _currentUser.IsAdmin(),
                Issue = issue,
                IsAuthor = _currentUser.UserId == issue.AuthorId
            }),
            Problem);
    }

    [HttpGet("NewIssue")]
    public IActionResult NewIssue()
    {
        return View();
    }

    [HttpPost("NewIssue")]
    public async Task<IActionResult> NewIssue(NewIssueViewModel model)
    {
        var command = new CreateIssueCommand(model.Title, model.Description, model.Category, model.Urgency, _currentUser.UserId, _dateTimeProvider.Now);
        var result = await _mediator.Send(command);

        return result.Match(
            issue => RedirectToAction(nameof(Issue), nameof(Issues), new { id = issue.IssueId }),
            Problem);

    }

    public IActionResult IssuesPartial(IEnumerable<Issue> issues)
    {
        return PartialView("_IssuesPartial", issues);
    }


    [HttpGet("EditIssue/{issueId:int}")]
    public async Task<IActionResult> EditIssue(int issueId)
    {
        var query = new GetIssueQuery(_currentUser.UserId, issueId);
        var result = await _mediator.Send(query);

        return result.Match(
            issue => View(new EditIssueViewModel
            {
                IssueId = issue.IssueId,
                Title = issue.Title,
                Category = issue.Category,
                Urgency = issue.Urgency,
                Description = issue.Description
            }),
            Problem);
    }

    [HttpPost("EditIssue")]
    public async Task<IActionResult> EditIssue(EditIssueViewModel model)
    {
        var command = new UpdateIssueCommand(model.IssueId, _currentUser.UserId, model.Title, model.Description, model.Category, model.Urgency);
        var result = await _mediator.Send(command);

        return result.Match(
            _ => RedirectToAction(nameof(Issue), nameof(Issues), new { id = model.IssueId }),
            Problem);
    }

    [HttpPost("DeleteIssue")]
    public async Task<IActionResult> DeleteIssue([FromBody] DeleteIssueRequest request)
    {
        var command = new RemoveIssueCommand(_currentUser.UserId, request.IssueId);
        var result = await _mediator.Send(command);

        return result.Match(
            _ => RedirectToAction(nameof(Issues), nameof(Issues)),
                Problem);
    }

    [HttpPost("NewComment")]
    public async Task<IActionResult> NewComment(NewCommentViewModel model)
    {
        var command = new NewCommentCommand(model.IssueId, _currentUser.UserId, model.CommentContent);
        var result = await _mediator.Send(command);

        return result.Match(
            _ => RedirectToAction(nameof(Issue), new { id = model.IssueId }),
            Problem);
    }

    [HttpPost("Assign")]
    public async Task<IActionResult> Assign(AssignViewModel model)
    {
        var command = new AssignIssueCommand(_currentUser.UserId, model.IssueId, model.Assignee);
        var result = await _mediator.Send(command);

        return result.Match(
            _ => RedirectToAction(nameof(Issue), new { id = model.IssueId }),
            Problem);
    }

    [HttpPost("Close")]
    public async Task<IActionResult> Close(int issueId)
    {
        var command = new CloseIssueCommand(_currentUser.UserId, issueId);
        var result = await _mediator.Send(command);

        return result.Match(
            _ => RedirectToAction(nameof(Issue), new { id = issueId }),
            Problem);
    }

    [HttpPost("Reopen")]
    public async Task<IActionResult> Reopen(int issueId)
    {
        var command = new ReopenIssueCommand(_currentUser.UserId, issueId);
        var result = await _mediator.Send(command);

        return result.Match(
            _ => RedirectToAction(nameof(Issue), new { id = issueId }),
            Problem);
    }

    [HttpPost("Label")]
    public async Task<IActionResult> Label(LabelViewModel model)
    {
        var command = new LabelIssueCommand(_currentUser.UserId, model.IssueId, model.Label);
        var result = await _mediator.Send(command);

        return result.Match(
            _ => RedirectToAction(nameof(Issue), new { id = model.IssueId }),
            Problem);
    }

    [HttpPost("UploadImage")]
    public async Task<IActionResult> UploadImage(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return Json(new { success = false, message = "No file selected" });

        var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
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