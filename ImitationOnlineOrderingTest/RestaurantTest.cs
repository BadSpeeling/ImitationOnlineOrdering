using ImitationOnlineOrdering.Database;
using ImitationOnlineOrdering.Models;
using Microsoft.EntityFrameworkCore;

namespace ImitationOnlineOrderingTest
{
    [TestClass]
    public sealed class RestaurantTest
    {

        private static Guid testUserID = Guid.NewGuid();
        private TestInitializers testInitializers;

        public RestaurantTest ()
        {
            testInitializers = new TestInitializers(GetContext(), testUserID);
        }

        [ClassCleanup]
        public static async Task ClassCleanup()
        {
            var dbContext = GetContext();
            await dbContext.Restaurant.Where(r => r.RestaurantManagerUserID.Equals(testUserID)).ExecuteDeleteAsync();
            await dbContext.Franchise.Where(r => r.FranchiseOwnerUserID.Equals(testUserID)).ExecuteDeleteAsync();
        }

        [TestMethod]
        public async Task TestGetRestaurants()
        {

            var restaurant = await testInitializers.CreateRestaurant();

            var dbContext = GetContext();
            var restaurantHandler = new RestaurantDbHandler(dbContext);
            var restaurants = await restaurantHandler.GetRestaurants();

            Assert.IsGreaterThan(0, restaurants.Count);

        }

        [TestMethod]
        public async Task TestPostRestaurant()
        {

            var franchise = await testInitializers.CreateFranchise();

            var dbContext = GetContext();
            var restaurantHandler = new RestaurantDbHandler(dbContext);
            var restaurant = new Restaurant()
            {
                RestaurantName = "Eric's Icecream Palace",
                RestaurantManagerUserID = testUserID,
                FranchiseID = franchise.FranchiseID
            };

            await restaurantHandler.PostRestaurant(restaurant);

            Assert.IsGreaterThan(0, restaurant.RestaurantID);

        }

        [TestMethod]
        public async Task TestGetRestaurant()
        {

            var restaurant = await testInitializers.CreateRestaurant();

            var dbContext = GetContext();
            var restaurantHandler = new RestaurantDbHandler(dbContext);

            var foundRestaurant = await restaurantHandler.GetRestaurant(restaurant.RestaurantID);
            Assert.IsNotNull(foundRestaurant);

        }

        [TestMethod]
        public async Task TestGetRestaurantMenuItems()
        {

            var restaurant = await testInitializers.CreateRestaurantWithMenuItems();

            var dbContext = GetContext();
            var restaurantHandler = new RestaurantDbHandler(dbContext);

            var efRestaurant = await restaurantHandler.GetRestaurant(restaurant.RestaurantID);
            Assert.IsNotNull(efRestaurant?.MenuItems?.FirstOrDefault());

        }

        [TestMethod]
        public async Task TestPatchRestaurant()
        {

            var restaurant = await testInitializers.CreateRestaurant();

            var dbContext = GetContext();
            var restaurantHandler = new RestaurantDbHandler(dbContext);

            await restaurantHandler.PatchRestaurant(new Restaurant() { 
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

            var restaurant = await testInitializers.CreateRestaurant();

            var dbContext = GetContext();
            var restaurantHandler = new RestaurantDbHandler(dbContext);

            await restaurantHandler.DeleteRestaurant(restaurant.RestaurantID);            
            Assert.IsNull(dbContext.Restaurant.Find(restaurant.RestaurantID));

        }

        [TestMethod]
        public async Task TestDeleteRestaurantAndMenuItems()
        {

            var restaurant = await testInitializers.CreateRestaurantWithMenuItems();

            var dbContext = GetContext();
            var restaurantHandler = new RestaurantDbHandler(dbContext);

            await restaurantHandler.DeleteRestaurant(restaurant.RestaurantID);
            Assert.IsNull(dbContext.MenuItem.Find(restaurant.RestaurantID));

        }

        public static OnlineOrderingDb GetContext ()
        {
            return new OnlineOrderingDb(
                new DbContextOptionsBuilder<OnlineOrderingDb>()
                    .UseSqlServer("Server=DESKTOP-LREGU2K\\SQLEXPRESS;Trusted_Connection=True;TrustServerCertificate=True;Initial Catalog=ImitationOnlineOrdering")
                    .Options
            );
        }

    }
}
