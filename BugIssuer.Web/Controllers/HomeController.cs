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

    public HomeController(ILogger<HomeController> logger, ISender sender)
    {
        _logger = logger;
        _sender = sender;
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

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

