using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
namespace BugIssuer.Web.Controllers;

[Authorize]
[Route("")]
public class HomeController : ApiController
{
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
}

