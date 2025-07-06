using Microsoft.AspNetCore.Mvc;
using MusicMatchly.Api.Helpers;
using MusicMatchly.Api.Services;

namespace MusicMatchly.Api.Controllers
{
    [ApiController]
    [Route("spotify")]
    public class SpotifyController : ControllerBase
    {
        private readonly SpotifyService _spotifyService;

        public SpotifyController(SpotifyService spotifyService)
        {
            _spotifyService = spotifyService;
        }

        [HttpGet("top-artists")]
        public async Task<IActionResult> GetTopArtists()
        {
            var accessToken = await TokenHelper.GetAccessTokenAsync(HttpContext);
            if (accessToken == null) return Unauthorized();

            var data = await _spotifyService.GetTopArtistsAsync(accessToken);
            if (data == null) return StatusCode(500, "Failed to fetch top artists");

            return Content(data, "application/json");
        }
    }
}
