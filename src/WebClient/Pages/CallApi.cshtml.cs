using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Text.Json;

namespace WebClient.Pages
{
    public class CallApiModel : PageModel
    {
        public string Json = string.Empty;
        private readonly JsonSerializerOptions jsonOptions = new() { WriteIndented = true };
        private static readonly HttpClient client = new();

        public async Task OnGet()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var content = await client.GetStringAsync("https://localhost:6001/identity");

            var parsed = JsonDocument.Parse(content);
            var formatted = JsonSerializer.Serialize(parsed, jsonOptions);

            Json = formatted;
        }
    }
}
