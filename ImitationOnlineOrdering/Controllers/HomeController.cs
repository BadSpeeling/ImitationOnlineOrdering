using ImitationOnlineOrdering.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ImitationOnlineOrdering.Infrastructure;

namespace ImitationOnlineOrdering.Controllers;

[Authorize]
public class HomeController : Controller
{

    public HomeController()
    {

    }

    public IActionResult Index()
    {
        return View();
    }

    [Authorize(Policy = AppRoles.AuthorizationPolicies.AssignmentToFranchiseOwnerRequired)]
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
