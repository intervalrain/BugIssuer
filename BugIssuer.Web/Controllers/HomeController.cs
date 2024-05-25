using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BugIssuer.Web.Models;
using MediatR;
using BugIssuer.Application.Issuer.Queries.ListIssues;
using BugIssuer.Application.Issuer.Queries.GetIssue;

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

