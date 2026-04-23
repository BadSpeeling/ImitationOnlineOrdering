using ImitationOnlineOrdering.Database;
using ImitationOnlineOrdering.Models;
using Microsoft.EntityFrameworkCore;

namespace ImitationOnlineOrderingTest
{
    [TestClass]
    public sealed class FranchiseTest
    {

        private static Guid testUserID = Guid.NewGuid();
        private TestInitializers testInitializers;

        public FranchiseTest()
        {
            testInitializers = new TestInitializers(GetContext(), testUserID);
        }

        [ClassCleanup]
        public static async Task ClassCleanup()
        {
            var dbContext = GetContext();
            await dbContext.Franchise.Where(f => f.FranchiseOwnerUserID.Equals(testUserID)).ExecuteDeleteAsync();
        }

        [TestMethod]
        public async Task TestGetFranchises()
        {
            var franchise = await testInitializers.CreateFranchise();

            var dbContext = GetContext();
            var franchiseHandler = new FranchiseDbHandler(dbContext);
            var franchises = await franchiseHandler.GetFranchises();

            Assert.IsGreaterThan(0, franchises.Count);
        }

        [TestMethod]
        public async Task TestPostFranchise()
        {
            var dbContext = GetContext();
            var franchiseHandler = new FranchiseDbHandler(dbContext);
            var franchise = new Franchise()
            {
                FranchiseName = "Eric's Franchises",
                FranchiseOwnerUserID = testUserID,
            };

            await franchiseHandler.PostFranchise(franchise);

            Assert.IsGreaterThan(0, franchise.FranchiseID);
        }

        [TestMethod]
        public async Task TestGetFranchise()
        {
            var franchise = await testInitializers.CreateFranchise();

            var dbContext = GetContext();
            var franchiseHandler = new FranchiseDbHandler(dbContext);

            var foundFranchise = await franchiseHandler.GetFranchise(franchise.FranchiseID);
            Assert.IsNotNull(foundFranchise);
        }

        [TestMethod]
        public async Task TestPutFranchise()
        {
            var franchise = await testInitializers.CreateFranchise();

            var dbContext = GetContext();
            var franchiseHandler = new FranchiseDbHandler(dbContext);

            await franchiseHandler.PatchFranchise(new Franchise()
            {
                FranchiseID = franchise.FranchiseID,
                FranchiseName = "Eric's New Franchise",
            });

            var currentFranchise = dbContext.Franchise.Find(franchise.FranchiseID);

            Assert.AreEqual("Eric's New Franchise", currentFranchise?.FranchiseName);
            Assert.IsNotNull(currentFranchise?.FranchiseOwnerUserID);

        }

        [TestMethod]
        public async Task TestDeleteFranchise()
        {
            var franchise = await testInitializers.CreateFranchise();

            var dbContext = GetContext();
            var franchiseHandler = new FranchiseDbHandler(dbContext);

            await franchiseHandler.DeleteFranchise(franchise.FranchiseID);
            Assert.IsNull(dbContext.Franchise.Find(franchise.FranchiseID));
        }

        public static OnlineOrderingDb GetContext()
        {
            return new OnlineOrderingDb(
                new DbContextOptionsBuilder<OnlineOrderingDb>()
                    .UseSqlServer("Server=DESKTOP-LREGU2K\\SQLEXPRESS;Trusted_Connection=True;TrustServerCertificate=True;Initial Catalog=ImitationOnlineOrdering")
                    .Options
            );
        }

    }
}
