using AutoMapper;
using ImitationOnlineOrdering.Database;
using ImitationOnlineOrdering.Infrastructure;
using ImitationOnlineOrdering.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace ImitationOnlineOrdering.Controllers
{
    public class FranchiseController : Controller
    {
        private readonly FranchiseDbHandler FranchiseHandler;
        private readonly IIdentity Identity;
        private readonly IMapper Mapper;

        public FranchiseController(FranchiseDbHandler franchiseHandler, IIdentity identity, IMapper mapper)
        {
            FranchiseHandler = franchiseHandler;
            Identity = identity;
            Mapper = mapper;
        }

        // GET: Franchise
        public async Task<IActionResult> Index()
        {
            List<Franchise> franchises;

            try
            {
                franchises = await FranchiseHandler.GetFranchises(Identity.GetUserID());
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }

            return View(franchises);
        }

        // GET: Franchise/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Franchise? franchise;

            try
            {
                franchise = await FranchiseHandler.GetFranchise(id.Value);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }

            if (franchise == null)
            {
                return NotFound();
            }

            return View(franchise);
        }

        // GET: Franchise/Create
        [Authorize(Policy = AppRoles.AuthorizationPolicies.AssignmentToFranchiseOwnerRequired)]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Franchise/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = AppRoles.AuthorizationPolicies.AssignmentToFranchiseOwnerRequired)]
        public async Task<IActionResult> Create([Bind("FranchiseName")] Franchise franchise)
        {
            franchise.FranchiseOwnerUserID = Identity.GetUserID();

            if (ModelState.IsValid)
            {
                try
                {
                    await FranchiseHandler.PostFranchise(franchise);
                }
                catch (Exception ex)
                {
                    return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
                }

                return RedirectToAction(nameof(Index));
            }
            return View(franchise);
        }

        // GET: Franchise/Edit/5
        [Authorize(Policy = AppRoles.AuthorizationPolicies.AssignmentToFranchiseOwnerRequired)]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Franchise? franchise;

            try
            {
                franchise = await FranchiseHandler.GetFranchise(id.Value);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }

            if (franchise == null)
            {
                return NotFound();
            }
            return View(franchise);
        }

        // POST: Franchise/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = AppRoles.AuthorizationPolicies.AssignmentToFranchiseOwnerRequired)]
        public async Task<IActionResult> Edit(int id, [Bind("FranchiseID,FranchiseName")] Franchise franchise)
        {
            if (id != franchise.FranchiseID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await FranchiseHandler.PatchFranchise(Mapper.Map<FranchisePatchCommand>(franchise));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!(await FranchiseHandler.FranchiseExists(id)))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(franchise);
        }

        // GET: Franchise/Delete/5
        [Authorize(Policy = AppRoles.AuthorizationPolicies.AssignmentToFranchiseOwnerRequired)]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Franchise? franchise;

            try
            {
                franchise = await FranchiseHandler.GetFranchise(id.Value);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }

            if (franchise == null)
            {
                return NotFound();
            }

            return View(franchise);
        }

        // POST: Franchise/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = AppRoles.AuthorizationPolicies.AssignmentToFranchiseOwnerRequired)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await FranchiseHandler.DeleteFranchise(id);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }

            return RedirectToAction(nameof(Index));
        }

    }
}
