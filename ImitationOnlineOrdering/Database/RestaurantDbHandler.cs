using Microsoft.EntityFrameworkCore;
using ImitationOnlineOrdering.Models;

namespace ImitationOnlineOrdering.Database
{
    public class RestaurantDbHandler
    {

        private OnlineOrderingDb dbContext;
        
        public RestaurantDbHandler (OnlineOrderingDb dbContext) 
        {
            this.dbContext = dbContext;
        }

        public async Task<List<Restaurant>> GetRestaurants ()
        {
            return await dbContext.Restaurant.ToListAsync();
        }

        public async Task<Restaurant> GetRestaurant (int id)
        {
            return await dbContext.Restaurant
                .Include(r => r.Franchise)
                .Include(r => r.MenuItems)
                .FirstAsync(r => r.RestaurantID == id);
        }

        public async Task PostRestaurant (Restaurant restaurant)
        {

            dbContext.Restaurant.Add(restaurant);
            await dbContext.SaveChangesAsync();

        }

        public async Task PatchRestaurant (RestaurantPatchCommand restaurant) 
        {

            Restaurant efRestaurant;
            
            try { 
                efRestaurant = await GetRestaurant(restaurant.RestaurantID);
            }
            catch (Exception ex)
            {
                throw new Exception($"Could not get Restaurant {restaurant.RestaurantID} for PATCH: {ex.Message}");
            }


            if (restaurant.StreetAddress != null && !restaurant.StreetAddress.Equals(efRestaurant.StreetAddress))
            {
                efRestaurant.StreetAddress = restaurant.StreetAddress;
            }

            if (restaurant.City != null && !restaurant.City.Equals(efRestaurant.City))
            {
                efRestaurant.City = restaurant.City;
            }

            if (restaurant.State != null && !restaurant.State.Equals(efRestaurant.State))
            {
                efRestaurant.State = restaurant.State;
            }

            if (restaurant.Zip != null && !restaurant.Zip.Equals(efRestaurant.Zip))
            {
                efRestaurant.Zip = restaurant.Zip;
            }

            await dbContext.SaveChangesAsync();

        }

        public async Task DeleteRestaurant (int restaurantID)
        {

            Restaurant restaurant;

            try { 
                restaurant = await GetRestaurant(restaurantID);
            }
            catch (Exception ex)
            {
                throw new Exception($"Could not get Restaurant {restaurantID} for DELETE: {ex.Message}");
            }

            dbContext.Restaurant.Remove(restaurant);
            await dbContext.SaveChangesAsync();

        }

        public async Task<bool> RestaurantExists (int restaurantID)
        {
            return await dbContext.Restaurant.AnyAsync(e => e.RestaurantID == restaurantID);
        }

    }
}
