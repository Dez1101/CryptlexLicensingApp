using CryptlexLicensingApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading.Tasks;

namespace CryptlexLicensingApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(ILogger<HomeController> logger,
            IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Authenticate([FromBody] LoginModel model)
        {
            if (model == null)
            {
                return BadRequest("Invalid request");
            }

            var client = new HttpClient();

            var requestData = new
            {
                accountId = model.AccountId,
                email = model.Email,
                password = model.Password
            };

            var jsonContent = System.Text.Json.JsonSerializer.Serialize(requestData);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            _logger.LogInformation("Sending authentication request: {RequestData}", jsonContent);

            var response = await client.PostAsync("https://api.cryptlex.com/v3/accounts/login", content);

            if (response.IsSuccessStatusCode)
            {
                var objJson = await response.Content.ReadAsStringAsync();
                var authResponse = JsonConvert.DeserializeObject<AuthResponse>(objJson);
                _logger.LogInformation("Authentication successful. Access Token: {AccessToken}", authResponse.AccessToken);

                return RedirectToAction("Index");
            }
            else
            {
                _logger.LogError($"Authentication failed with status code {response.StatusCode}");
                ViewBag.ErrorMessage = "Authentication failed. Please check your credentials.";
                return View("Licensing");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Products()
        {
            var authorizationHeader = HttpContext.Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
            {
                return Unauthorized("Access token is required.");
            }

            var accessToken = authorizationHeader.Substring("Bearer ".Length).Trim();

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            var response = await client.GetAsync("https://api.cryptlex.com/v3/products");

            if (response.IsSuccessStatusCode)
            {
                var productsJson = await response.Content.ReadAsStringAsync();
                var products = JsonConvert.DeserializeObject<List<ProductModel>>(productsJson);
                return View(products); 
            }
            else
            {
                _logger.LogError($"Failed to retrieve products with status code {response.StatusCode}");
                var errorContent = await response.Content.ReadAsStringAsync();
                return StatusCode((int)response.StatusCode, $"Failed to retrieve products. Error: {errorContent}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductModel model)
        {
            if (model == null)
            {
                return BadRequest("Invalid product data");
            }

            //retrieve the access token from the Authorization header
            var authorizationHeader = HttpContext.Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
            {
                return Unauthorized("Access token is required.");
            }

            // Extract the access token value
            var accessToken = authorizationHeader.Substring("Bearer ".Length).Trim();

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            var jsonContent = JsonConvert.SerializeObject(model);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://api.cryptlex.com/v3/products", content);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Product created successfully.");
                return RedirectToAction("Products");
            }
            else
            {
                _logger.LogError($"Failed to create product with status code {response.StatusCode}");
                return View("Error", new ErrorViewModel { RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Licenses()
        {
            // Attempt to retrieve the access token from the Authorization header
            var authorizationHeader = HttpContext.Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
            {
                return Unauthorized("Access token is required.");
            }

            // Extract the access token value
            var accessToken = authorizationHeader.Substring("Bearer ".Length).Trim();

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            var response = await client.GetAsync("https://api.cryptlex.com/v3/licenses");

            if (response.IsSuccessStatusCode)
            {
                var licensesJson = await response.Content.ReadAsStringAsync();
                var licenses = JsonConvert.DeserializeObject<List<LicenseRequestModel>>(licensesJson);
                return View(licenses); // Assuming you have a 'Licenses' view ready to display the list
            }
            else
            {
                _logger.LogError($"Failed to retrieve licenses with status code {response.StatusCode}");
                var errorContent = await response.Content.ReadAsStringAsync();
                return StatusCode((int)response.StatusCode, $"Failed to retrieve licenses. Error: {errorContent}");
            }
        }


        public IActionResult ActivateLicense()
        {
            var model = new LicensingModel
            {
                Message = "Enter your license key to activate the product"
            };
            ViewData["Title"] = "License Activation";
            return View(model);
        }




        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private class AuthResponse


        {
            public string AccessToken { get; set; }
        }
    }
}