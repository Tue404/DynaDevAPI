using DynaDevAPI.Models;
using DynaDevFE.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Json;

namespace DynaDevFE.Controllers
{
    public class ProductDetailsController : Controller
    {
        private readonly HttpClient _httpClient;

        public ProductDetailsController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7101/"); // Đảm bảo đường dẫn API đúng
        }

        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound("Mã sản phẩm không hợp lệ.");
            }

            try
            {
                var response = await _httpClient.GetAsync($"/api/ProductDetails/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var sanPhamDto = await response.Content.ReadFromJsonAsync<SanPhamDto>();
                    if (sanPhamDto == null)
                    {
                        ViewBag.ErrorMessage = "Không tìm thấy sản phẩm.";
                        return View("Error");
                    }

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
                        DanhSachAnh = sanPhamDto.DanhSachAnh ?? new List<string>(),
                        DanhGiaSanPham = sanPhamDto.DanhGiaSanPham?.Select(d => new DanhGiaViewModel
                        {
                            MaDanhGia = d.MaDanhGia,
                            MaKH = d.MaKH,   // Hiển thị MaKH
                            Email = d.Email,  // Hiển thị Email
                            DiemDanhGia = d.DiemDanhGia,
                            BinhLuan = d.BinhLuan,
                            TrangThai = d.TrangThai,
                            NgayDanhGia = d.NgayDanhGia
                        }).ToList() ?? new List<DanhGiaViewModel>()
                    };

                    return View(sanPhamViewModel);
                }
                else
                {
                    ViewBag.ErrorMessage = "Không thể lấy dữ liệu sản phẩm.";
                    return View("Error");
                }
            }
            catch
            {
                ViewBag.ErrorMessage = "Lỗi kết nối server.";
                return View("Error");
            }
        }
    }
}
