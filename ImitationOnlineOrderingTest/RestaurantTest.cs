using ImitationOnlineOrdering.Database;
using ImitationOnlineOrdering.Infrastructure;
using ImitationOnlineOrdering.Models;
using Microsoft.EntityFrameworkCore;

namespace ImitationOnlineOrderingTest
{
    [TestClass]
    public sealed class RestaurantTest
    {

        private TestingDatabase testingDatabase;
        private FakeIdentity authenticatedIdentity;

        public RestaurantTest ()
        {
            authenticatedIdentity = new FakeIdentity();
            testingDatabase = new TestingDatabase(authenticatedIdentity);
        }

        [TestMethod]
        public async Task TestGetRestaurants()
        {

            var dbContext = testingDatabase.CreateContext();
            var restaurantHandler = new RestaurantDbHandler(dbContext);
            var restaurants = await restaurantHandler.GetRestaurants();

            Assert.IsGreaterThan(0, restaurants.Count);

        }

        [TestMethod]
        public async Task TestPostRestaurant()
        {

            var dbContext = testingDatabase.CreateContext();
            var restaurantHandler = new RestaurantDbHandler(dbContext);
 
            var franchise = dbContext.Franchise.First();
            var restaurant = new Restaurant()
            {
                RestaurantName = "Eric's Icecream Palace",
                RestaurantManagerUserID = Guid.NewGuid(),
                FranchiseID = franchise.FranchiseID,
                Franchise = franchise,                
                StreetAddress = "1201 Wilson Blvd",
                State = "VA",
                Zip = "22201",
                City = "Arlington"
            };

            await restaurantHandler.PostRestaurant(restaurant);
            Assert.IsGreaterThan(0, restaurant.RestaurantID);

        }

        [TestMethod]
        public async Task TestGetRestaurant()
        {

            var dbContext = testingDatabase.CreateContext();

            var restaurant = dbContext.Restaurant.First();
            var restaurantHandler = new RestaurantDbHandler(dbContext);

            Assert.IsNotNull(restaurant, "No restaurants");

            var foundRestaurant = await restaurantHandler.GetRestaurant(restaurant.RestaurantID);
            Assert.IsNotNull(foundRestaurant);

            Assert.AreEqual(restaurant.StreetAddress, foundRestaurant.StreetAddress);
            Assert.AreEqual(restaurant.State, foundRestaurant.State);
            Assert.AreEqual(restaurant.City, foundRestaurant.City);
            Assert.AreEqual(restaurant.Zip, foundRestaurant.Zip);

            bool exceptionOccured = false;

            try
            {
                await restaurantHandler.GetRestaurant(-1);
            }
            catch (Exception ex)
            {
                exceptionOccured = true;
            }

            Assert.IsTrue(exceptionOccured, "An error should occur if a non-existing RestaurantID is given");

        }

        [TestMethod]
        public async Task TestGetRestaurantWithMenuItems()
        {

            var dbContext = testingDatabase.CreateContext();
            var menuItem = dbContext.MenuItem.First();
            var restaurantHandler = new RestaurantDbHandler(dbContext);

            Assert.IsNotNull(menuItem, "MenuItem cannot be null");

            var efRestaurant = await restaurantHandler.GetRestaurant(menuItem.RestaurantID);
            Assert.IsNotNull(efRestaurant?.MenuItems?.FirstOrDefault());

        }

        [TestMethod]
        public async Task TestPatchRestaurant()
        {

            var dbContext = testingDatabase.CreateContext();
            var restaurant = dbContext.Restaurant.First();
            var restaurantHandler = new RestaurantDbHandler(dbContext);

            await restaurantHandler.PatchRestaurant(new RestaurantPatchCommand()
            {
                RestaurantID = restaurant.RestaurantID,
                RestaurantName = "Eric's Sorbet Palace"
            });

            var efRestaurant = dbContext.Restaurant.Find(restaurant.RestaurantID);
            Assert.AreEqual("Eric's Sorbet Palace", efRestaurant?.RestaurantName);
            Assert.IsNotNull(efRestaurant?.RestaurantManagerUserID);

        }

        [TestMethod]
        public async Task TestDeleteRestaurant()
        {

            var dbContext = testingDatabase.CreateContext();
            var restaurant = dbContext.Restaurant.Where(r => r.RestaurantName == "DeleteRestaurant").First();
            var restaurantHandler = new RestaurantDbHandler(dbContext);

            Assert.IsNotNull(restaurant, "Restaurant must exist initially");

            await restaurantHandler.DeleteRestaurant(restaurant.RestaurantID);
            Assert.IsNull(dbContext.Restaurant.Find(restaurant.RestaurantID));

        }

        [TestMethod]
        public async Task TestDeleteRestaurantAndMenuItems()
        {

            var dbContext = testingDatabase.CreateContext();
            var restaurant = dbContext.Restaurant.Include(r => r.MenuItems).Where(r => r.RestaurantName == "DeleteRestaurantWithMenu").First();
            var restaurantHandler = new RestaurantDbHandler(dbContext);

            Assert.IsNotNull(restaurant, "Restaurant must exist initially");
            Assert.IsGreaterThan(0, restaurant.MenuItems?.Count ?? 0);

            await restaurantHandler.DeleteRestaurant(restaurant.RestaurantID);
            Assert.IsNull(dbContext.Restaurant.Find(restaurant.RestaurantID));
            Assert.IsNull(dbContext.MenuItem.Find(restaurant.RestaurantID));

        }

    }
}
