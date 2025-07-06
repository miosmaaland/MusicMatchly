using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using System.Security.Claims;
using System.Net.Http.Headers;
using System.Text.Json;
using MusicMatchly.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. Add Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = "Spotify";
})
.AddCookie()
.AddOAuth("Spotify", options =>
{
    options.ClientId = "YOUR_SPOTIFY_CLIENT_ID";
    options.ClientSecret = "YOUR_SPOTIFY_CLIENT_SECRET";
    options.CallbackPath = new PathString("/callback");

    options.AuthorizationEndpoint = "https://accounts.spotify.com/authorize";
    options.TokenEndpoint = "https://accounts.spotify.com/api/token";
    options.UserInformationEndpoint = "https://api.spotify.com/v1/me";

    options.Scope.Add("user-read-email");
    options.Scope.Add("user-top-read");

    options.SaveTokens = true;

    options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
    options.ClaimActions.MapJsonKey(ClaimTypes.Name, "display_name");
    options.ClaimActions.MapJsonKey(ClaimTypes.Email, "email");

    options.Events = new OAuthEvents
    {
        OnCreatingTicket = async context =>
        {
            var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);
            var response = await context.Backchannel.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var payload = JsonDocument.Parse(await response.Content.ReadAsStringAsync()).RootElement;
            context.RunClaimActions(payload);
        }
    };
});

// 2. Add Controllers and HttpClient
builder.Services.AddControllers();
builder.Services.AddHttpClient<SpotifyService>();

var app = builder.Build();

// 3. Middleware Pipeline
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// 4. Routes
app.MapGet("/", () => "MusicMatchly API is running");

app.MapGet("/login", async context =>
{
    if (!context.User.Identity.IsAuthenticated)
    {
        await context.ChallengeAsync("Spotify");
        return;
    }
    context.Response.Redirect("/profile");
});

app.MapGet("/profile", (HttpContext context) =>
{
    if (!context.User.Identity.IsAuthenticated)
        return Results.Unauthorized();

    var userName = context.User.FindFirst(ClaimTypes.Name)?.Value ?? "Unknown";
    var userEmail = context.User.FindFirst(ClaimTypes.Email)?.Value ?? "No email";

    return Results.Json(new { Name = userName, Email = userEmail });
});

// 5. Map controller endpoints
app.MapControllers();

app.Run();
