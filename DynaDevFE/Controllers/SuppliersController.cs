using DynaDevAPI.Models;
using DynaDevFE.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DynaDevFE.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    public class SuppliersController : Controller
    {
        private readonly HttpClient _httpClient;

        public SuppliersController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // GET: Suppliers/Index
        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync("https://localhost:7101/api/Suppliers");

            if (response.IsSuccessStatusCode)
            {
                var nhaCungCaps = await response.Content.ReadFromJsonAsync<List<NhaCungCapViewModel>>();
                return View(nhaCungCaps);
            }

            ModelState.AddModelError(string.Empty, "Không thể tải danh sách nhà cung cấp.");
            return View();
        }

        // GET: Suppliers/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(NhaCungCapViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var response = await _httpClient.PostAsJsonAsync("https://localhost:7101/api/Suppliers", model);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Thêm nhà cung cấp thành công!";
                return RedirectToAction("Index");
            }

            var errorMessage = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, $"Lỗi từ API: {errorMessage}");

            return View(model);
        }

        // GET: Suppliers/Edit/{id}
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response = await _httpClient.GetAsync($"https://localhost:7101/api/Suppliers/{id}");
            if (response.IsSuccessStatusCode)
            {
                var nhaCungCap = await response.Content.ReadFromJsonAsync<NhaCungCapViewModel>();
                return View(nhaCungCap);
            }

            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, NhaCungCapViewModel model)
        {
            if (id != model.MaNCC)
            {
                return BadRequest("Mã nhà cung cấp không khớp.");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var response = await _httpClient.PutAsJsonAsync($"https://localhost:7101/api/Suppliers/{id}", model);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Cập nhật nhà cung cấp thành công!";
                return RedirectToAction(nameof(Index));
            }

            var errorMessage = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, $"Lỗi từ API: {errorMessage}");

            return View(model);
        }


        // GET: Suppliers/Delete/{id}
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response = await _httpClient.GetAsync($"https://localhost:7101/api/Suppliers/{id}");
            if (response.IsSuccessStatusCode)
            {
                var nhaCungCap = await response.Content.ReadFromJsonAsync<NhaCungCapViewModel>();
                return View(nhaCungCap);
            }

            return NotFound();
        }

        // POST: Suppliers/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var response = await _httpClient.DeleteAsync($"https://localhost:7101/api/Suppliers/{id}");
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Không thể xóa nhà cung cấp.");
            }

            return View();
        }
    }

}
