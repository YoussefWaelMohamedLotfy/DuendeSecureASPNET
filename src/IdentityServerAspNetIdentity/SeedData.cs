using IdentityModel;
using IdentityServerAspNetIdentity.Data;
using IdentityServerAspNetIdentity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Security.Claims;

namespace IdentityServerAspNetIdentity;

public sealed class SeedData
{
    public static async void EnsureSeedData(WebApplication app)
    {
        using var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
        //scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

        //var context = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
        //context.Database.Migrate();
        var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
        context.Database.EnsureDeleted();
        context.Database.Migrate();

        //if (!context.Clients.Any())
        //{
        //    foreach (var client in Config.Clients)
        //    {
        //        context.Clients.Add(client.ToEntity());
        //    }
        //    context.SaveChanges();
        //}

        //if (!context.IdentityResources.Any())
        //{
        //    foreach (var resource in Config.IdentityResources)
        //    {
        //        context.IdentityResources.Add(resource.ToEntity());
        //    }
        //    context.SaveChanges();
        //}

        //if (!context.ApiScopes.Any())
        //{
        //    foreach (var resource in Config.ApiScopes)
        //    {
        //        context.ApiScopes.Add(resource.ToEntity());
        //    }
        //    context.SaveChanges();
        //}

        var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var alice = await userMgr.FindByNameAsync("alice");

        if (alice is null)
        {
            alice = new ApplicationUser
            {
                UserName = "alice",
                Email = "AliceSmith@email.com",
                EmailConfirmed = true,
                PhoneNumber = "01234",
                PhoneNumberConfirmed = true,
                FavoriteColor = "Red"
            };
            var result = await userMgr.CreateAsync(alice, "Pass123$");

            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }

            result = await userMgr.AddClaimsAsync(alice, new Claim[] {
                            new Claim(JwtClaimTypes.Name, "Alice Smith"),
                            new Claim(JwtClaimTypes.GivenName, "Alice"),
                            new Claim(JwtClaimTypes.FamilyName, "Smith"),
                            new Claim(JwtClaimTypes.WebSite, "http://alice.com"),
                        });

            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }

            Log.Debug("alice created");
        }
        else
        {
            Log.Debug("alice already exists");
        }

        var bob = await userMgr.FindByNameAsync("bob");

        if (bob is null)
        {
            bob = new ApplicationUser
            {
                UserName = "bob",
                Email = "BobSmith@email.com",
                EmailConfirmed = false,
                PhoneNumber = "578910",
                PhoneNumberConfirmed = false,
                FavoriteColor = "Blue"
            };
            var result = await userMgr.CreateAsync(bob, "Pass123$");

            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }

            result = await userMgr.AddClaimsAsync(bob, new Claim[] {
                            new Claim(JwtClaimTypes.Name, "Bob Smith"),
                            new Claim(JwtClaimTypes.GivenName, "Bob"),
                            new Claim(JwtClaimTypes.FamilyName, "Smith"),
                            new Claim(JwtClaimTypes.WebSite, "http://bob.com"),
                            new Claim("location", "somewhere")
                        });

            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }

            Log.Debug("bob created");
        }
        else
        {
            Log.Debug("bob already exists");
        }
    }
}
