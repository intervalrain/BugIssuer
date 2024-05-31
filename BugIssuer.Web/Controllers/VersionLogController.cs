using Microsoft.AspNetCore.Mvc;

namespace BugIssuer.Web.Controllers;

public class VersionLogController : ApiController
{
    [HttpGet]
    public IActionResult VersionLog()
    {
        return View();
    }
}

