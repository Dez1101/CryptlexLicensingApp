using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace CryptlexLicensingApp.Pages
{
    public class LoginModel(ILogger<LoginModel> logger, HttpClient httpClient) : PageModel
    {
        private readonly ILogger<LoginModel> _logger = logger;
        private readonly HttpClient _httpClient = httpClient;

        [BindProperty]
        public string Email { get; set; }
        [BindProperty]
        public string Password { get; set; }
        [BindProperty]
        public string AccountId { get; set; }

        public void OnGet()
        {
            if (User.Identity.IsAuthenticated)
            {
                Response.Redirect("/Index");
            }
        }

        public async Task<IActionResult> OnPost()
        {
            if (!string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(Password) && !string.IsNullOrEmpty(AccountId))
            {
                var requestData = new
                {
                    accountId = AccountId,
                    email = Email,
                    password = Password
                };

                var jsonContent = JsonSerializer.Serialize(requestData);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                _logger.LogInformation("Sending authentication request: {RequestData}", jsonContent);

                var response = await _httpClient.PostAsync("https://api.cryptlex.com/v3/accounts/login", content);

                if (response.IsSuccessStatusCode)
                {
                    var objJson = await response.Content.ReadAsStringAsync();
                    JsonSerializerOptions options = new() { PropertyNameCaseInsensitive = true };
                    AuthResponse authResponse = JsonSerializer.Deserialize<AuthResponse>(objJson, options);
                    _logger.LogInformation("Authentication successful. Access Token: {AccessToken}", authResponse.AccessToken);

                    var claims = new List<Claim> { new(ClaimTypes.Name, Email), new(ClaimTypes.Authentication, authResponse.AccessToken) };
                    var identity = new ClaimsIdentity(claims, "CookieAuthentication");
                    ClaimsPrincipal principal = new(identity);

                    await HttpContext.SignInAsync("CookieAuthentication", principal);

                    return RedirectToPage("/Index");
                }
            }

            return Page();
        }
        private class AuthResponse
        {
            public string AccessToken { get; set; }
        }
    }
}
