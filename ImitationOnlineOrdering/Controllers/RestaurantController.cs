using ImitationOnlineOrdering.Database;
using ImitationOnlineOrdering.Infrastructure;
using ImitationOnlineOrdering.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ImitationOnlineOrdering.Controllers
{
    public class RestaurantController : Controller
    {
        private readonly RestaurantDbHandler RestaurantHandler;
        private readonly FranchiseDbHandler FranchiseHandler;
        private readonly IIdentity Identity;

        public RestaurantController(RestaurantDbHandler restaurantHandler, FranchiseDbHandler franchiseHandler, IIdentity identity)
        {
            RestaurantHandler = restaurantHandler;
            FranchiseHandler = franchiseHandler;
            Identity = identity;
        }

        // GET: Restaurant
        public async Task<IActionResult> Index()
        {

            List<Restaurant> restaurants;

            try
            {
                restaurants = await RestaurantHandler.GetRestaurants();
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }

            return View(restaurants);

        }
        // GET: Restaurant/Details/5
        public async Task<IActionResult> Details(int id)
        {

            Restaurant restaurant;

            try
            {
                restaurant = await RestaurantHandler.GetRestaurant(id);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }

            return View(restaurant);            
        }

        // GET: Restaurant/Create/{franchiseID}
        [Authorize(Policy = AppRoles.AuthorizationPolicies.AssignmentToFranchiseOwnerRequired)]
        public async Task<IActionResult> Create(int franchiseID)
        {
            return View();
        }

        // POST: Restaurant/Create/{franchiseID}
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = AppRoles.AuthorizationPolicies.AssignmentToFranchiseOwnerRequired)]
        public async Task<IActionResult> Create(int franchiseID, [Bind("RestaurantName")] Restaurant restaurant)
        {

            var authenticatedUserID = Identity.GetUserID();

            if (!await FranchiseHandler.IsFranchiseOwner(franchiseID, authenticatedUserID))
            {
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }

            restaurant.FranchiseID = franchiseID;
            restaurant.RestaurantManagerUserID = authenticatedUserID;

            if (ModelState.IsValid)
            {

                try { 
                    await RestaurantHandler.PostRestaurant(restaurant);
                }
                catch (Exception ex)
                {
                    return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
                }

                return RedirectToAction(nameof(Details), new { id = restaurant.RestaurantID });
            }
            return View(restaurant);
        }

        // GET: Restaurant/Edit/5
        [Authorize(Policy = AppRoles.AuthorizationPolicies.AssignmentToFranchiseOwnerRequired)]
        public async Task<IActionResult> Edit(int id)
        {

            Restaurant restaurant;

            try
            { 
                restaurant = await RestaurantHandler.GetRestaurant(id);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }

            return View(restaurant);

        }

        // POST: Restaurant/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = AppRoles.AuthorizationPolicies.AssignmentToFranchiseOwnerRequired)]
        public async Task<IActionResult> Edit(int id, [Bind("RestaurantID,RestaurantName")] RestaurantPatchCommand restaurant)
        {
            if (id != restaurant.RestaurantID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await RestaurantHandler.PatchRestaurant(restaurant);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!(await RestaurantHandler.RestaurantExists(id)))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (Exception)
                {
                    return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
                }
                return RedirectToAction(nameof(Details), new { id = restaurant.RestaurantID });
            }
            return View(restaurant);
        }

        // GET: Restaurant/Delete/5
        [Authorize(Policy = AppRoles.AuthorizationPolicies.AssignmentToFranchiseOwnerRequired)]
        public async Task<IActionResult> Delete(int id)
        {

            Restaurant restaurant;

            try
            {
                restaurant = await RestaurantHandler.GetRestaurant(id);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }

            return View(restaurant);

        }

        // POST: Restaurant/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = AppRoles.AuthorizationPolicies.AssignmentToFranchiseOwnerRequired)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            int restaurantID;

            try
            {
                restaurantID = (await RestaurantHandler.GetRestaurant(id)).RestaurantID;
                await RestaurantHandler.DeleteRestaurant(id);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }

            return RedirectToAction(nameof(Details), "Franchise", new { id = restaurantID }) ;
        
        }

    }
}
