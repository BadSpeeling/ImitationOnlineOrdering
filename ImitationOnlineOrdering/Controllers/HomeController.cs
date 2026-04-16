using ImitationOnlineOrdering.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ImitationOnlineOrdering.Infrastructure;

namespace ImitationOnlineOrdering.Controllers;

[Authorize]
public class HomeController : OnlineOrderingController
{

    public HomeController(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
    {

    }

    public IActionResult Index()
    {
        return View();
    }

    [Authorize(Policy = AppRoles.AuthorizationPolicies.AssignmentToFranchiseOwnerRequired)]
    public IActionResult Privacy()
    {

        ViewData["UserID"] = GetUserID();
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
