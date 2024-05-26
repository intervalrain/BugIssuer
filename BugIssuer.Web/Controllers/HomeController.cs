using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BugIssuer.Web.Models;
using MediatR;
using BugIssuer.Application.Issuer.Queries.ListIssues;
using BugIssuer.Application.Issuer.Queries.GetIssue;
using BugIssuer.Application.Issuer.Queries.SearchIssues;
using BugIssuer.Application.Issuer.Commands.NewComment;
using BugIssuer.Application.Issuer.Commands.CreateIssue;
using BugIssuer.Application.Issuer.Commands.UpdateIssue;
using BugIssuer.Application.Issuer.Commands.RemoveIssue;

namespace BugIssuer.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ISender _sender;
    private readonly IWebHostEnvironment _environment;

    public HomeController(ILogger<HomeController> logger, ISender sender, IWebHostEnvironment environment)
    {
        _logger = logger;
        _sender = sender;
        _environment = environment;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Overview()
    {
        return View();
    }

    public IActionResult VersionLog()
    {
        return View();
    }

    public IActionResult Profile()
    {
        return View();
    }

    public IActionResult Issues(string sortOrder, string filterStatus)
    {
        var query = new ListIssuesQuery(sortOrder, filterStatus);

        ViewData["IdSortParm"] = String.IsNullOrEmpty(sortOrder) ? "id_desc" : "";
        ViewData["CategorySortParm"] = sortOrder == "Category" ? "category_desc" : "Category";
        ViewData["TitleSortParm"] = sortOrder == "Title" ? "title_desc" : "Title";
        ViewData["AuthorSortParm"] = sortOrder == "Author" ? "author_desc" : "Author";
        ViewData["DateTimeSortParm"] = sortOrder == "DateTime" ? "datetime_desc" : "DateTime";
        ViewData["LastUpdateSortParm"] = sortOrder == "LastUpdate" ? "lastupdate_desc" : "LastUpdate";
        ViewData["CommentsSortParm"] = sortOrder == "Comments" ? "comments_desc" : "Comments";
        ViewData["AssigneeSortParm"] = sortOrder == "Assignee" ? "assignee_desc" : "Assignee";
        ViewData["UrgencySortParm"] = sortOrder == "Urgency" ? "urgency_desc" : "Urgency";
        ViewData["StatusSortParm"] = sortOrder == "Status" ? "status_desc" : "Status";

        ViewData["CurrentFilter"] = filterStatus;
        ViewData["CurrentSort"] = sortOrder;

        var result = _sender.Send(query).GetAwaiter().GetResult();

        return View(result.Value);
    }

    public IActionResult Issue(int id)
    {
        var query = new GetIssueQuery(id);

        var issue = _sender.Send(query).GetAwaiter().GetResult();

        return View(issue.Value);
    }

    [HttpGet]
    public IActionResult NewIssue()
    {
        return View();
    }

    [HttpPost]
    public IActionResult NewIssue(NewIssueViewModel model)
    {
        if (ModelState.IsValid)
        {
            var command = new CreateIssueCommand(model.Title, model.Description, model.Category, model.Urgency, "00123", "Yao");

            var issue = _sender.Send(command).GetAwaiter().GetResult();

            return RedirectToAction(nameof(Issues));
        }
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> EditIssue(int id)
    {
        var query = new GetIssueQuery(id);

        var result  = await _sender.Send(query);
        
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

    [HttpPost]
    public async Task<IActionResult> EditIssue(EditIssueViewModel model)
    {
        if (ModelState.IsValid)
        {
            var query = new GetIssueQuery(model.IssueId);
            var result = await _sender.Send(query);
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
            var response = await _sender.Send(command);
            return RedirectToAction(nameof(Issue), new { id = model.IssueId });
        }
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteIssue([FromBody] DeleteIssueRequest request)
    {
        try
        {
            var command = new RemoveIssueCommand("00012415", request.IssueId);

            var result = await _sender.Send(command);

            if (result.IsError)
            {
                return Problem(detail: result.FirstError.Description);
            }
            return RedirectToAction(nameof(Issues));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while deleting issues.");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost]
    public IActionResult NewComment(NewCommentViewModel model)
    {
        if (ModelState.IsValid)
        {
            var command = new NewCommentCommand(
                model.IssueId,
                "00053997",
                "Rain Hu",
                model.CommentContent
                );

            var comment = _sender.Send(command).GetAwaiter().GetResult();

            return RedirectToAction(nameof(Issue), new { id = model.IssueId });
        }
        return BadRequest("Invalid comment data"); 
    }

    [HttpGet]
    public async Task<IActionResult> SearchIssues(string searchText, CancellationToken token)
    {
        try
        {
            var query = new SearchIssuesQuery(searchText);
            var issues = await _sender.Send(query, token);
            return PartialView("_IssueListPartial", issues.Value);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while searching issues.");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost]
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

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

