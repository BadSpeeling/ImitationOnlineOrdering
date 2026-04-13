using identity_client_web_app.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using identity_client_web_app.Infrastructure;

namespace identity_client_web_app.Controllers;

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

    [Authorize(Policy = AppRoles.AuthorizationPolicies.AssignmentToRestaurantOwnerRequired)]
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
