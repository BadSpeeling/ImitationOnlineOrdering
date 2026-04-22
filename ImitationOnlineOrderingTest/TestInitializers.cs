using ImitationOnlineOrdering.Database;
using ImitationOnlineOrdering.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImitationOnlineOrderingTest
{
    public class TestInitializers
    {

        private OnlineOrderingDb dbContext;
        private Guid testUserID;
        
        public TestInitializers(OnlineOrderingDb dbContext, Guid testUserID)
        {
            this.dbContext = dbContext;
            this.testUserID = testUserID;
        }

        public async Task<Restaurant> CreateRestaurant()
        {

            var franchise = await CreateFranchise();

            var efRestaurant = new Restaurant()
            {
                RestaurantName = "Eric's Sundae Palace",
                RestaurantManagerUserID = testUserID,
                FranchiseID = franchise.FranchiseID,
            };

            dbContext.Restaurant.Add(efRestaurant);
            await dbContext.SaveChangesAsync();

            return efRestaurant;

        }

        public async Task<Restaurant> CreateRestaurantWithMenuItems()
        {

            var efRestaurant = await CreateRestaurant();

            var efMenuItem = new MenuItem()
            {
                MenuItemName = "Hamburger",
                Price = 5.99M,
                RestaurantID = efRestaurant.RestaurantID
            };

            dbContext.MenuItem.Add(efMenuItem);
            await dbContext.SaveChangesAsync();

            return efRestaurant;

        }

        public async Task<Franchise> CreateFranchise()
        {
            var efFranchise = new Franchise()
            {
                FranchiseName = "Eric's Franchise",
                FranchiseOwnerUserID = testUserID,
            };

            dbContext.Franchise.Add(efFranchise);
            await dbContext.SaveChangesAsync();

            return efFranchise;
        }

    }
}
