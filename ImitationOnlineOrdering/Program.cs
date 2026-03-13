using ImitationOnlineOrdering;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<OnlineOrderingDb>(opt => opt.UseSqlServer("Server=DESKTOP-LREGU2K\\SQLEXPRESS;Trusted_Connection=True;TrustServerCertificate=True;Initial Catalog=ImitationOnlineOrdering"));

var app = builder.Build();

app.MapGet("/restaurants", async (OnlineOrderingDb db) =>
{
    return await db.Restaurant.ToListAsync();
});

app.MapGet("/restaurant/{id}", async (OnlineOrderingDb db, int id) =>
{
    
    var restaurant = await db.Restaurant.FindAsync(id);

    if (restaurant != null)
    {
        return Results.Ok(restaurant);
    }
    else
    {
        return Results.NotFound();
    }

});

app.MapPost("/restaurant", async (OnlineOrderingDb db, Restaurant restaurant) =>
{

    db.Restaurant.Add(restaurant);
    await db.SaveChangesAsync();

    return Results.Created($"/restaurants/{restaurant.RestaurantID}", restaurant);

});

app.MapGet("/restaurant/{id}/menuitems", async (OnlineOrderingDb db, int id) =>
{

    var restaurant = await db.Restaurant.FindAsync(id);

    if (restaurant == null)
    {
        return Results.NotFound();
    }

    var menuItems = await db.MenuItem
        .Where((menuItem) => menuItem.RestaurantID == id)
        .ToListAsync();

    return Results.Ok(menuItems);

});

app.MapGet("/menuitem/{id}", async (OnlineOrderingDb db, int id) =>
{

    var menuItem = await db.MenuItem.FindAsync(id);

    if (menuItem != null)
    {
        return Results.Ok(menuItem);
    }
    else
    {
        return Results.NotFound();
    }

});

app.MapPost("/menuitem", async (OnlineOrderingDb db, MenuItem menuItem) =>
{

    db.MenuItem.Add(menuItem);
    await db.SaveChangesAsync();

    return Results.Created($"/menuitem/{menuItem.MenuItemID}", menuItem);

});

app.Run();