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
    public class RestaurantController : OnlineOrderingController
    {
        private readonly RestaurantDbHandler restaurantHandler;

        public RestaurantController(RestaurantDbHandler restaurantHandler, IHttpContextAccessor httpContext) : base(httpContext)
        {
            this.restaurantHandler = restaurantHandler;
        }

        // GET: Restaurant
        public async Task<IActionResult> Index()
        {

            List<Restaurant> restaurants;

            try
            {
                restaurants = await restaurantHandler.GetRestaurants();
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }

            return View(restaurants);

        }
        // GET: Restaurant/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Restaurant? restaurant;

            try
            {
                restaurant = await restaurantHandler.GetRestaurant(id.Value);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }

            if (restaurant == null)
            {
                return NotFound();
            }

            return View(restaurant);            
        }

        // GET: Restaurant/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Restaurant/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RestaurantName")] Restaurant restaurant)
        {

            restaurant.UserID = GetUserID();

            if (ModelState.IsValid)
            {

                try { 
                    await restaurantHandler.PostRestaurant(restaurant);
                }
                catch (Exception ex)
                {
                    return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
                }

                return RedirectToAction(nameof(Index));
            }
            return View(restaurant);
        }

        // GET: Restaurant/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Restaurant? restaurant;

            try
            { 
                restaurant = await restaurantHandler.GetRestaurant(id.Value);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }

            if (restaurant == null)
            {
                return NotFound();
            }
            return View(restaurant);
        }

        // POST: Restaurant/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RestaurantID,RestaurantName")] Restaurant restaurant)
        {
            if (id != restaurant.RestaurantID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await restaurantHandler.PutRestaurant(restaurant);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!(await restaurantHandler.RestaurantExists(id)))
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
            return View(restaurant);
        }

        // GET: Restaurant/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Restaurant? restaurant;

            try
            {
                restaurant = await restaurantHandler.GetRestaurant(id.Value);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }

            if (restaurant == null)
            {
                return NotFound();
            }

            return View(restaurant);
        }

        // POST: Restaurant/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            try
            {
                await restaurantHandler.DeleteRestaurant(id);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }

            return RedirectToAction(nameof(Index));
        }

    }
}
