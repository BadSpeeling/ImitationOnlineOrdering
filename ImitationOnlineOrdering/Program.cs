using ImitationOnlineOrdering;
using ImitationOnlineOrdering.Database;
using ImitationOnlineOrdering.Infrastructure;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<OnlineOrderingDb>(opt => opt.UseSqlServer("Server=DESKTOP-LREGU2K\\SQLEXPRESS;Trusted_Connection=True;TrustServerCertificate=True;Initial Catalog=ImitationOnlineOrdering"));
builder.Services.AddScoped<IIdentity, CloudIdentity>();
builder.Services.AddScoped<RestaurantDbHandler>();
builder.Services.AddScoped<FranchiseDbHandler>();

// This is required to be instantiated before the OpenIdConnectOptions starts getting configured.
// By default, the claims mapping will map claim names in the old format to accommodate older SAML applications.
// This flag ensures that the ClaimsIdentity claims collection will be built from the claims in the token
JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

// Sign-in users with the Microsoft identity platform
builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(builder.Configuration)
    .EnableTokenAcquisitionToCallDownstreamApi()
    .AddInMemoryTokenCaches();

builder.Services.Configure<OpenIdConnectOptions>(OpenIdConnectDefaults.AuthenticationScheme, options =>
{
    // The claim in the Jwt token where App roles are available.
    options.TokenValidationParameters.RoleClaimType = "roles";
});

// Adding authorization policies that enforce authorization using Azure AD roles.
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(AppRoles.AuthorizationPolicies.AssignmentToFranchiseOwnerRequired, policy => policy.RequireRole(AppRoles.AppRole.FranchiseOwner));
});

builder.Services.AddControllersWithViews(options =>
{
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
    options.Filters.Add(new AuthorizeFilter(policy));
}).AddMicrosoftIdentityUI();

builder.Services.AddAutoMapper(cfg => { }, typeof(AutoMapperProfiles));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "CreateRestaurant",
    pattern: "Restaurant/Create/{franchiseID}",
    defaults: new { controller = "Restaurant", action = "Create" }
);

app.MapControllerRoute(
    name: "EditRestaurant",
    pattern: "Restaurant/Edit/{id}",
    defaults: new { controller = "Restaurant", action = "Edit" }
);

app.MapControllerRoute(
    name: "DetailsRestaurant",
    pattern: "Restaurant/Details/{id}",
    defaults: new { controller = "Restaurant", action = "Details" }
);

app.MapControllerRoute(
    name: "DeleteRestaurant",
    pattern: "Restaurant/Delete/{id}",
    defaults: new { controller = "Restaurant", action = "Delete" }
);

app.MapControllerRoute(
    name: "FranchiseDefault",
    pattern: "Franchise/{action=Index}/{id?}",
    defaults: new { controller = "Franchise" }
);

app.MapControllerRoute(
    name: "Home",
    pattern: "Home/{action=Index}",
    defaults: new { controller = "Home" }
);

app.MapControllerRoute(
    name: "Initial",
    pattern: "",
    defaults: new { controller = "Franchise", action = "Index" }
);

app.Run();