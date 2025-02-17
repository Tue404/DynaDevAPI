using DynaDevAPI.Models;
using DynaDevFE.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace DynaDevFE.Controllers
{
    public class ProductDetailsController : Controller
    {
        private readonly HttpClient _httpClient;

        public ProductDetailsController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7101/");
        }
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound("Mã sản phẩm không hợp lệ.");
            }

            try
            {
                // Gọi API backend để lấy dữ liệu sản phẩm
                var response = await _httpClient.GetAsync($"/api/ProductDetails/{id}");

                if (response.IsSuccessStatusCode)
                {
                    // Đọc dữ liệu từ API
                    var sanPhamDto = await response.Content.ReadFromJsonAsync<SanPhamDto>();

                    // Chuyển DTO sang ViewModel
                    var sanPhamViewModel = new SanPhamViewModel
                    {
                        MaSP = sanPhamDto.MaSP,
                        MaLoai = sanPhamDto.MaLoai,
                        MaNCC = sanPhamDto.MaNCC,   
                        TenSanPham = sanPhamDto.TenSanPham,
                        TacGia = sanPhamDto.TacGia,
                        TenNCC = sanPhamDto.TenNCC,
                        NamXuatBan = sanPhamDto.NamXuatBan,
                        Gia = sanPhamDto.Gia,
                        MoTa = sanPhamDto.MoTa,
                        TinhTrang = sanPhamDto.TinhTrang,
                        SoLuongTrongKho = sanPhamDto.SoLuongTrongKho,
                        DanhSachAnh = sanPhamDto.DanhSachAnh
                    };

                    return View(sanPhamViewModel);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    ViewBag.ErrorMessage = "Không tìm thấy sản phẩm.";
                    return View("Error");
                }
                else
                {
                    ViewBag.ErrorMessage = "Đã xảy ra lỗi khi lấy dữ liệu sản phẩm.";
                    return View("Error");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi gọi API: {ex.Message}");
                ViewBag.ErrorMessage = "Đã xảy ra lỗi kết nối đến server.";
                return View("Error");
            }
        }
    }
}
