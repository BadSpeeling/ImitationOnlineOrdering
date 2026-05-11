using ImitationOnlineOrdering.Controllers;
using ImitationOnlineOrdering.Database;
using ImitationOnlineOrdering.Infrastructure;
using ImitationOnlineOrdering.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace ImitationOnlineOrderingTest
{
    [TestClass]
    public sealed class FranchiseTest
    {
        
        private TestingDatabase testingDatabase;
        private FakeIdentity authenticatedIdentity;

        public FranchiseTest()
        {
            authenticatedIdentity = new FakeIdentity();
            testingDatabase = new TestingDatabase(authenticatedIdentity);            
        }

        [TestMethod]
        public async Task TestGetFranchises()
        {
            var dbContext = testingDatabase.CreateContext();
            var franchiseHandler = new FranchiseDbHandler(dbContext);
            var franchises = await franchiseHandler.GetFranchises(authenticatedIdentity.GetUserID());

            Assert.HasCount(1, franchises, $"Expect only 1 franchise, has {franchises.Count}");
        }

        [TestMethod]
        public async Task TestPostFranchise()
        {
            var dbContext = testingDatabase.CreateContext();
            var franchiseHandler = new FranchiseDbHandler(dbContext);         
            var franchise = new Franchise()
            {
                FranchiseName = "Eric's Franchises",
                FranchiseOwnerUserID = Guid.NewGuid(),
            };

            await franchiseHandler.PostFranchise(franchise);

            Assert.IsGreaterThan(0, franchise.FranchiseID);
        }

        [TestMethod]
        public async Task TestGetFranchise()
        {
            var dbContext = testingDatabase.CreateContext();
            var franchise = dbContext.Franchise.First();

            var franchiseHandler = new FranchiseDbHandler(dbContext);

            var foundFranchise = await franchiseHandler.GetFranchise(franchise.FranchiseID);
            Assert.IsNotNull(foundFranchise);
        }

        [TestMethod]
        public async Task TestPatchFranchise()
        {
            var dbContext = testingDatabase.CreateContext();
            var franchise = dbContext.Franchise.Where(f => f.FranchiseName == "PatchTest").First();

            var franchiseHandler = new FranchiseDbHandler(dbContext);

            await franchiseHandler.PatchFranchise(new FranchisePatchCommand()
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

            var dbContext = testingDatabase.CreateContext();
            var franchise = dbContext.Franchise.Where(f => f.FranchiseName == "DeleteTest").First();

            Assert.IsNotNull(franchise);

            var franchiseHandler = new FranchiseDbHandler(dbContext);

            await franchiseHandler.DeleteFranchise(franchise.FranchiseID);
            Assert.IsNull(dbContext.Franchise.Find(franchise.FranchiseID));

        }

        [TestMethod]
        public async Task TestFranchiseLoadsRestaurants()
        {

            var dbContext = testingDatabase.CreateContext();
            var franchiseID = dbContext.Franchise.Where(f => f.FranchiseName == "Blog1").FirstOrDefault()?.FranchiseID ?? 0;

            Assert.AreNotEqual(0, franchiseID, "Franchise lookup must work");

            var franchiseHandler = new FranchiseDbHandler(dbContext);

            var franchise = await franchiseHandler.GetFranchise(franchiseID);
            Assert.IsGreaterThan(0, franchise?.Restaurants?.Count ?? 0, "Included Restaurants must be more than 0");

        }

        [TestMethod]
        public async Task TestIsFranchiseOwner()
        {

            var dbContext = testingDatabase.CreateContext();
            var franchise = dbContext.Franchise.Where(f => f.FranchiseOwnerUserID == authenticatedIdentity.GetUserID()).First();

            Assert.IsNotNull(franchise);

            var franchiseHandler = new FranchiseDbHandler(dbContext);

            Assert.IsTrue(await franchiseHandler.IsFranchiseOwner(franchise.FranchiseID, authenticatedIdentity.GetUserID()));
            Assert.IsFalse(await franchiseHandler.IsFranchiseOwner(franchise.FranchiseID, Guid.NewGuid()));

        }

    }
}
