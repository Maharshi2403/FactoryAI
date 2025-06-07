using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using FactoryAI.Models;

namespace FactoryAI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        // Inject IHttpClientFactory (not HttpClient directly)
        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> BuildModel()
        {
            try
            {
                // Create HttpClient instance
                var httpClient = _httpClientFactory.CreateClient();
                
                // Example: Fetch Hugging Face models (replace with actual API endpoint)
                var response = await httpClient.GetAsync("https://huggingface.co/api/models");
                
                if (response.IsSuccessStatusCode)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    // Parse the JSON response into your model
                    var models = JsonSerializer.Deserialize<List<HuggingFaceModel>>(jsonContent, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    
                    return View(models);
                }
                else
                {
                    // Handle API error
                    _logger.LogError($"API call failed with status: {response.StatusCode}");
                    return View(new List<HuggingFaceModel>());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching models");
                return View(new List<HuggingFaceModel>());
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}