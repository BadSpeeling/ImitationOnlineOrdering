using ImitationOnlineOrdering.Database;
using ImitationOnlineOrdering.Models;
using Microsoft.EntityFrameworkCore;
using ImitationOnlineOrdering.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

public class TestingDatabase
{

    private readonly string ConnectionString = "Server=np:\\\\.\\pipe\\LOCALDB#EE27A6C8\\tsql\\query;TrustServerCertificate=True;Initial Catalog=ImitationOnlineOrderingTest";

    private static readonly object _lock = new();
    private static bool _databaseInitialized = false;

    public TestingDatabase(IIdentity authenticatedIdentity)
    {

        lock (_lock)
        {

            if (!_databaseInitialized)
            {

                using (var context = CreateContext())
                {
                    context.Database.EnsureDeleted();
                    context.Database.EnsureCreated();

                    var franchise = new Franchise { FranchiseName = "Blog1", FranchiseOwnerUserID = authenticatedIdentity.GetUserID() };

                    context.AddRange(
                        franchise,
                        new Franchise { FranchiseName = "PatchTest", FranchiseOwnerUserID = Guid.NewGuid() },
                        new Franchise { FranchiseName = "DeleteTest", FranchiseOwnerUserID = Guid.NewGuid() }
                    );
                    context.SaveChanges();

                    var restaurant = new Restaurant { RestaurantManagerUserID = authenticatedIdentity.GetUserID(), FranchiseID = franchise.FranchiseID, Franchise = franchise, StreetAddress = "1 Main St.", State = "MD", Zip = "99999", City = "Springfield" };
                    var deleteRestaurant = new Restaurant { RestaurantManagerUserID = authenticatedIdentity.GetUserID(), FranchiseID = franchise.FranchiseID, Franchise = franchise, StreetAddress = "1 Delete St.", State = "MD", Zip = "99999", City = "Springfield" };
                    var deleteRestaurantWithMenu = new Restaurant { RestaurantManagerUserID = authenticatedIdentity.GetUserID(), FranchiseID = franchise.FranchiseID, Franchise = franchise, StreetAddress = "1 Delete Menu St.", State = "MD", Zip = "99999", City = "Springfield" };

                    context.AddRange(
                        restaurant,
                        deleteRestaurant,
                        deleteRestaurantWithMenu
                    );
                    context.SaveChanges();

                    context.AddRange(
                        new MenuItem { MenuItemName = "Hot Dog", Price = 6.99M, RestaurantID = restaurant.RestaurantID, Restaurant = restaurant },
                        new MenuItem { MenuItemName = "Delete Dog", Price = 6.99M, RestaurantID = deleteRestaurantWithMenu.RestaurantID, Restaurant = deleteRestaurantWithMenu }

                    );
                    context.SaveChanges();

                }

                _databaseInitialized = true;
            }

        }

    }

    public OnlineOrderingDb CreateContext()
    {
        return new OnlineOrderingDb(
            new DbContextOptionsBuilder<OnlineOrderingDb>()
                .UseSqlServer(ConnectionString)
                .Options
        );
    }

}