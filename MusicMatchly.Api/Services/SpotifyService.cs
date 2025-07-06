using System.Net.Http.Headers;

namespace MusicMatchly.Api.Services
{
    public class SpotifyService
    {
        private readonly HttpClient _http;

        public SpotifyService(HttpClient http)
        {
            _http = http;
        }

        public async Task<string?> GetTopArtistsAsync(string accessToken)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "https://api.spotify.com/v1/me/top/artists");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await _http.SendAsync(request);
            if (!response.IsSuccessStatusCode) return null;

            return await response.Content.ReadAsStringAsync();
        }
    }
}
