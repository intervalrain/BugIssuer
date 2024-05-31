using Microsoft.AspNetCore.Mvc;

namespace BugIssuer.Web.Controllers;

public class OverviewController : ApiController
{
    [HttpGet]
    public IActionResult Overview()
    {
        return View();
    }
}

