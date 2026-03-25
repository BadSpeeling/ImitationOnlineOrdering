using Microsoft.EntityFrameworkCore;

namespace identity_client_web_app
{
    public class OnlineOrderingDb : DbContext
    {

        public DbSet<Restaurant> Restaurant => Set<Restaurant>();
        //public DbSet<MenuItem> MenuItem => Set<MenuItem>();

        public OnlineOrderingDb(DbContextOptions<OnlineOrderingDb> options) : base(options)
        {

        }

    }
}
