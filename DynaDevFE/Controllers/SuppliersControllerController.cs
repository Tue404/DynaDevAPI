using DynaDevAPI.Models;
using DynaDevFE.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace DynaDevFE.Controllers
{
    public class SuppliersController : Controller
    {
        private readonly HttpClient _httpClient;

        public SuppliersController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // GET: Suppliers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Suppliers/Create
        [HttpPost]
        public async Task<IActionResult> Create(NhaCungCapViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Gửi yêu cầu POST đến API để thêm nhà cung cấp
                var response = await _httpClient.PostAsJsonAsync("https://localhost:7101/api/Suppliers", model);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index"); // Điều hướng về trang danh sách nhà cung cấp
                }

                ModelState.AddModelError(string.Empty, "Không thể thêm nhà cung cấp.");
            }

            return View(model);
        }


        // GET: Suppliers/Edit/{id}
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


        // POST: Suppliers/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MaNCC,TenNCC,SDT,Email,DiaChi,TinhTrang")] NhaCungCapViewModel model)
        {
            if (id != model.MaNCC)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var response = await _httpClient.PutAsJsonAsync($"https://localhost:7101/api/Suppliers/{id}", model);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Không thể cập nhật nhà cung cấp.");
                }
            }

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
