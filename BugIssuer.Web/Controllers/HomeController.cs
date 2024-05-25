using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BugIssuer.Web.Models;
using MediatR;
using BugIssuer.Application.Issuer.Queries.ListIssues;
using BugIssuer.Application.Issuer.Queries.GetIssue;
using BugIssuer.Application.Issuer.Queries.SearchIssues;
using BugIssuer.Application.Issuer.Commands.NewComment;

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

    public IActionResult NewIssue()
    {
        return View();
    }

    [HttpPost]
    //[ValidateAntiForgeryToken]
    public async Task<IActionResult> PostComment([FromBody] CommentViewModel model)
    {
        if (ModelState.IsValid)
        {
            var command = new NewCommentCommand(
                model.IssueId,
                User.Identity.Name,
                User.Identity.Name,
                model.Content
                );

            var result = await _sender.Send(command);
            if (result.IsError)
            {
                return StatusCode(500, "Internal server error");
            }
            var query = new GetIssueQuery(model.IssueId);
            var response = await _sender.Send(query);
            return PartialView("_CommentListPartial", response.Value.Comments);
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

