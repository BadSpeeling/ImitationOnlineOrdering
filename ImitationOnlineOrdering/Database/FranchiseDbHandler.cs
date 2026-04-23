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

        public async Task PatchFranchise(Franchise franchise)
        {

            var efFranchise = await GetFranchise(franchise.FranchiseID);

            if (efFranchise == null)
            {
                throw new Exception($"Cannot update Franchise {franchise.FranchiseID} that does not exist");
            }

            if (franchise.FranchiseName != null && !franchise.FranchiseName.Equals(efFranchise.FranchiseName)) { 
                efFranchise.FranchiseName = franchise.FranchiseName;
            }

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
