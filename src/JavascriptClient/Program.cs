using Duende.Bff;
using Duende.Bff.Yarp;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

[Authorize]
static IResult LocalIdentityHandler(ClaimsPrincipal user)
{
    var name = user.FindFirst("name")?.Value ?? user.FindFirst("sub")?.Value;
    return Results.Json(new { message = "Local API Success!", user = name });
}

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthorization();

builder.Services
    .AddBff()
    .AddRemoteApis();

JwtSecurityTokenHandler.DefaultMapInboundClaims = false;
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultScheme = "Cookies";
        options.DefaultChallengeScheme = "oidc";
        options.DefaultSignOutScheme = "oidc";
    })
    .AddCookie("Cookies")
    .AddOpenIdConnect("oidc", options =>
    {
        options.Authority = "https://localhost:5001";

        options.ClientId = "bff";
        options.ClientSecret = "secret";
        options.ResponseType = "code";

        options.Scope.Add("api1");
        options.Scope.Add("offline_access");
        options.Scope.Add("email-verification");
        options.Scope.Add("phone-verification");
        options.Scope.Add("color");
        options.ClaimActions.MapJsonKey("email_verified", "email_verified");
        options.ClaimActions.MapJsonKey("phone_number", "phone_number");
        options.ClaimActions.MapJsonKey("phone_number_verified", "phone_number_verified");
        options.ClaimActions.MapUniqueJsonKey("favorite_color", "favorite_color");

        options.SaveTokens = true;
        options.GetClaimsFromUserInfoEndpoint = true;
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();

app.UseBff();

app.UseAuthorization();

app.MapBffManagementEndpoints();

// Uncomment this for Controller support
// endpoints.MapControllers()
//     .AsBffApiEndpoint();

app.MapGet("/local/identity", LocalIdentityHandler)
    .AsBffApiEndpoint();

app.MapRemoteBffApiEndpoint("/remote", "https://localhost:6001")
    .RequireAccessToken(TokenType.User);

app.Run();
