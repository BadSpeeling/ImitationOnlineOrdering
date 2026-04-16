using ImitationOnlineOrdering;
using Microsoft.EntityFrameworkCore;

namespace ImitationOnlineOrdering.Database
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
