using Microsoft.EntityFrameworkCore;
using ImitationOnlineOrdering.Models;

namespace ImitationOnlineOrdering.Database
{
    public class FranchiseDbHandler
    {

        private OnlineOrderingDb dbContext;

        public FranchiseDbHandler(OnlineOrderingDb dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<Franchise>> GetFranchises()
        {
            return await dbContext.Franchise.ToListAsync();
        }

        public async Task<Franchise?> GetFranchise(int id)
        {
            return await dbContext.Franchise.FirstOrDefaultAsync(f => f.FranchiseID == id);
        }

        public async Task PostFranchise(Franchise franchise)
        {
            dbContext.Franchise.Add(franchise);
            await dbContext.SaveChangesAsync();
        }

        public async Task PutFranchise(Franchise franchise)
        {
            if (!(await FranchiseExists(franchise.FranchiseID)))
            {
                throw new Exception($"Cannot update Franchise {franchise.FranchiseID} that does not exist");
            }

            dbContext.Franchise.Update(franchise);
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteFranchise(int franchiseID)
        {
            var franchise = await GetFranchise(franchiseID);

            if (franchise == null)
            {
                throw new Exception(franchiseID + " does not exist");
            }

            dbContext.Franchise.Remove(franchise);
            await dbContext.SaveChangesAsync();
        }

        public async Task<bool> FranchiseExists(int franchiseID)
        {
            return await dbContext.Franchise.AnyAsync(e => e.FranchiseID == franchiseID);
        }

    }
}
