
using System.Net.Http.Json;
using CarRentalApplication.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalApplication.Controllers
{
    public class CustomersController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CustomersController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient("GatewayClient");
            var customers = await client.GetFromJsonAsync<List<Customer>>("customers-service/api/customers");
            return View(customers ?? new List<Customer>());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var client = _httpClientFactory.CreateClient("GatewayClient");
            var customer = await client.GetFromJsonAsync<Customer>($"customers-service/api/customers/{id}");

            if (customer == null)
                return NotFound();

            return View(customer);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Phone,Email")] Customer customer)
        {
            if (!ModelState.IsValid)
                return View(customer);

            var client = _httpClientFactory.CreateClient("GatewayClient");
            var response = await client.PostAsJsonAsync("customers-service/api/customers", customer);

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = await response.Content.ReadAsStringAsync();
                return View(customer);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var client = _httpClientFactory.CreateClient("GatewayClient");
            var customer = await client.GetFromJsonAsync<Customer>($"customers-service/api/customers/{id}");

            if (customer == null)
                return NotFound();

            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Phone,Email")] Customer customer)
        {
            if (id != customer.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(customer);

            var client = _httpClientFactory.CreateClient("GatewayClient");
            var response = await client.PutAsJsonAsync($"customers-service/api/customers/{id}", customer);

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = await response.Content.ReadAsStringAsync();
                return View(customer);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var client = _httpClientFactory.CreateClient("GatewayClient");
            var customer = await client.GetFromJsonAsync<Customer>($"customers-service/api/customers/{id}");

            if (customer == null)
                return NotFound();

            return View(customer);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client = _httpClientFactory.CreateClient("GatewayClient");
            var response = await client.DeleteAsync($"customers-service/api/customers/{id}");

            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = await response.Content.ReadAsStringAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}