using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using CarRentalApplication.Models;

namespace CarRentalApplication.Controllers
{
    public class MaintenanceController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public MaintenanceController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public IActionResult History()
        {
            return View(new List<RepairHistoryViewModel>());
        }

        [HttpPost]
        public async Task<IActionResult> History(int vehicleId)
        {
            var client = _httpClientFactory.CreateClient("MaintenanceApi");

            var repairs = await client.GetFromJsonAsync<List<RepairHistoryViewModel>>(
                $"api/maintenance/vehicles/{vehicleId}/repairs");

            return View(repairs ?? new List<RepairHistoryViewModel>());
        }

        [HttpGet]
        public async Task<IActionResult> Usage()
        {
            var client = _httpClientFactory.CreateClient("MaintenanceApi");

            try
            {
                var response = await client.GetAsync("api/maintenance/usage");

                if (!response.IsSuccessStatusCode)
                {
                    var msg = await response.Content.ReadAsStringAsync();
                    ViewBag.Error = $"API Error ({(int)response.StatusCode}): {msg}";
                    return View(null);
                }

                var json = await response.Content.ReadAsStringAsync();
                return View((object)json);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "API call failed: " + ex.Message;
                return View(null);
            }
        }


    }
}
