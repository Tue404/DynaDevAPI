using DynaDevFE.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Linq;

namespace DynaDevFE.Controllers
{
    public class StatisticalController : Controller
    {
        private readonly HttpClient _httpClient;

        public StatisticalController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7101/");
        }

        public async Task<IActionResult> Index(int? year, int? month, DateTime? date)
        {
            List<DoanhThuTheoNgayThangNamViewModel> doanhThu = new List<DoanhThuTheoNgayThangNamViewModel>();
            try
            {
                // Gọi API để lấy dữ liệu doanh thu
                var response = await _httpClient.GetAsync($"api/Statistical/DoanhThuTheoNgayThangNam?year={year}&month={month}&date={date}");
                if (response.IsSuccessStatusCode)
                {
                    var jsonData = await response.Content.ReadAsStringAsync();
                    doanhThu = JsonSerializer.Deserialize<List<DoanhThuTheoNgayThangNamViewModel>>(jsonData, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }

                // Xử lý logic lọc theo năm, tháng và ngày (tương tự logic JS bạn muốn chuyển)
                if (year.HasValue && month.HasValue)
                {
                    doanhThu = doanhThu
                        .Where(d => d.Nam == year.Value && d.Thang == month.Value)
                        .ToList();
                }
                else if (year.HasValue)
                {
                    doanhThu = doanhThu
                        .Where(d => d.Nam == year.Value)
                        .ToList();
                }
                else if (date.HasValue)
                {
                    doanhThu = doanhThu
                        .Where(d => d.Nam == date.Value.Year && d.Thang == date.Value.Month && d.Ngay == date.Value.Day)
                        .ToList();
                }

                // Nếu không có dữ liệu, bạn có thể xử lý thêm như gửi thông báo lỗi
                if (!doanhThu.Any())
                {
                    ViewBag.Message = "Không có dữ liệu phù hợp.";
                }

                return View(doanhThu);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Đã xảy ra lỗi: {ex.Message}";
                return View(doanhThu);
            }
        }
    }
}
