using Microsoft.AspNetCore.Authentication;

namespace MusicMatchly.Api.Helpers
{
    public static class TokenHelper
    {
        public static async Task<string?> GetAccessTokenAsync(HttpContext context)
        {
            return await context.GetTokenAsync("access_token");
        }
    }
}
