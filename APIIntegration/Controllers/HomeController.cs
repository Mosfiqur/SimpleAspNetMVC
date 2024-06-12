
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using APIIntegration.Models;
using System.Diagnostics;
using Newtonsoft.Json;

namespace APIIntegration.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(_configuration[PaymentWebConstants.BaseAddress]);
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public async Task<IActionResult> TransferFund(string email, double amount)
        {
            try
            {
                var formData = new { email, amount };
                var content = new StringContent(JsonConvert.SerializeObject(formData), Encoding.UTF8, "application/json");
                var apiKey = _configuration[PaymentWebConstants.ApiKeyConfigName]; 
                var apiUrl = _configuration[PaymentWebConstants.FundTransferAPI];
                _httpClient.DefaultRequestHeaders.Add(PaymentWebConstants.ApiKeyHeader, apiKey);
                var response = await _httpClient.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    // Transfer successful
                    return RedirectToAction("Index");
                }
                else
                {
                    // Transfer failed, handle error
                    return RedirectToAction("Index"); // Redirect to some error page or show a message
                }
            }
            catch (Exception ex)
            {
                // Exception occurred, handle it
                return RedirectToAction("Index"); // Redirect to some error page or show a message
            }
        }
    }
}
