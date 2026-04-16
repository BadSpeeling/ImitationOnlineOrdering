using ImitationOnlineOrdering;
using ImitationOnlineOrdering.Database;
using Microsoft.EntityFrameworkCore;

namespace ImitationOnlineOrderingTest
{
    [TestClass]
    public sealed class RestaurantTest
    {

        private static Guid testUserID = Guid.NewGuid();

        [ClassCleanup]
        public static async Task ClassCleanup()
        {
            var dbContext = GetContext();
            await dbContext.Restaurant.Where(r => r.UserID.Equals(testUserID)).ExecuteDeleteAsync();
        }

        [TestMethod]
        public async Task TestGetRestaurants()
        {

            var restaurant = await CreateRestaurant();

            var dbContext = GetContext();
            var restaurantHandler = new RestaurantDbHandler(dbContext);
            var restaurants = await restaurantHandler.GetRestaurants();

            Assert.IsGreaterThan(0, restaurants.Count);

        }

        [TestMethod]
        public async Task TestPostRestaurant()
        {

            var dbContext = GetContext();
            var restaurantHandler = new RestaurantDbHandler(dbContext);
            var restaurant = new Restaurant()
            {
                RestaurantName = "Eric's Icecream Palace",
                UserID = testUserID,
            };

            await restaurantHandler.PostRestaurant(restaurant);

            Assert.IsGreaterThan(0, restaurant.RestaurantID);

        }

        [TestMethod]
        public async Task TestGetRestaurant()
        {

            var restaurant = await CreateRestaurant();

            var dbContext = GetContext();
            var restaurantHandler = new RestaurantDbHandler(dbContext);

            var foundRestaurant = await restaurantHandler.GetRestaurant(restaurant.RestaurantID);
            Assert.IsNotNull(foundRestaurant);

        }

        [TestMethod]
        public async Task TestPutRestaurant()
        {

            var restaurant = await CreateRestaurant();

            var dbContext = GetContext();
            var restaurantHandler = new RestaurantDbHandler(dbContext);

            await restaurantHandler.PutRestaurant(new Restaurant() { 
                RestaurantID = restaurant.RestaurantID,
                RestaurantName = "Eric's Sorbet Palace",
                UserID = restaurant.UserID
            });

            Assert.AreEqual("Eric's Sorbet Palace", dbContext.Restaurant.Find(restaurant.RestaurantID)?.RestaurantName);

        }

        [TestMethod]
        public async Task TestDeleteRestaurant()
        {

            var restaurant = await CreateRestaurant();

            var dbContext = GetContext();
            var restaurantHandler = new RestaurantDbHandler(dbContext);

            await restaurantHandler.DeleteRestaurant(restaurant.RestaurantID);            
            Assert.IsNull(dbContext.Restaurant.Find(restaurant.RestaurantID));

        }

        public async Task<Restaurant> CreateRestaurant ()
        {

            var efRestaurant = new Restaurant()
            {
                RestaurantName = "Eric's Sundae Palace",
                UserID = testUserID
            };

            var dbContext = GetContext();
            dbContext.Restaurant.Add(efRestaurant);
            await dbContext.SaveChangesAsync();

            return new Restaurant()
            {
                RestaurantID = efRestaurant.RestaurantID,
                RestaurantName = efRestaurant.RestaurantName,
                UserID = efRestaurant.UserID
            };

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
