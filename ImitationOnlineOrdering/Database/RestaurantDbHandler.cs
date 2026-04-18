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

        public async Task<Restaurant?> GetRestaurant (int id)
        {
            return await dbContext.Restaurant.Include(r => r.MenuItems).FirstOrDefaultAsync(r => r.RestaurantID == id);
        }

        public async Task PostRestaurant (Restaurant restaurant)
        {

            dbContext.Restaurant.Add(restaurant);
            await dbContext.SaveChangesAsync();

        }

        public async Task PutRestaurant (Restaurant restaurant) 
        {

            if (!(await RestaurantExists(restaurant.RestaurantID)))
            {
                throw new Exception($"Cannot update Restaurant {restaurant.RestaurantID} that does not exist");
            }

            dbContext.Restaurant.Update(restaurant);            
            await dbContext.SaveChangesAsync();

        }

        public async Task DeleteRestaurant (int restaurantID)
        {

            var restaurant = await GetRestaurant(restaurantID);

            if (restaurant == null)
            {
                throw new Exception(restaurantID + " does not exist");
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
