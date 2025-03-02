using DynaDevFE.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System;

using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DynaDevFE.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly HttpClient _httpClient;

        public AuthenticationController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public IActionResult Index()
        {
            return View();
        }

        // Đăng ký
        [HttpPost]
        public async Task<IActionResult> Register(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return Json(new { success = false, message = "Email và mật khẩu không được để trống" });
            }

            Random rd = new Random();
            int random = rd.Next(0, 1000);
            string MaKH = "KH" + random.ToString("D3");
            try
            {
                var khachHang = new KhachHangViewModel
                {
                    MaKH = MaKH,
                    Email = email,
                    MatKhau = password,
                    TenKH = "",
                    SDT = "",
                    DiaChi = "",
                    TinhTrang = "Hoạt động",
                    NgayDangKy = DateTime.Now
                };

                var content = new StringContent(JsonSerializer.Serialize(khachHang), Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("https://localhost:7101/api/Authentication/register", content);

                try
                {
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["SuccessMessage"] = "Đăng ký thành công! Bạn có thể đăng nhập ngay.";
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Đăng ký thất bại. Vui lòng thử lại!";
                        return RedirectToAction("Register"); // Quay lại trang đăng ký
                    }
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "Đăng ký thất bại do lỗi hệ thống: " + ex.Message;
                    return RedirectToAction("Register");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Đăng ký thất bại", error = ex.Message });
            }
        }


        // Đăng nhập
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
            {
                return BadRequest(new { success = false, message = "Email và mật khẩu không được để trống" });
            }

            var content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("https://localhost:7101/api/Authentication/login", content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine("🔹 Response từ API: " + responseContent); // Log để kiểm tra

                // Deserialize JSON từ API
                var result = JsonSerializer.Deserialize<TokenResponse>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (result == null || string.IsNullOrEmpty(result.Token) || string.IsNullOrEmpty(result.MaKH))
                {
                    return BadRequest(new { success = false, message = "Lỗi xác thực, không tìm thấy MaKH hoặc Token!" });
                }

                var token = result.Token;
                var role = result.Role ?? "User"; // Lấy role từ API
                var maKH = result.MaKH;

                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTime.UtcNow.AddMinutes(30)
                };

                // Lưu cookie
                Response.Cookies.Append("JwtToken", token, cookieOptions);
                Response.Cookies.Append("UserRole", role, cookieOptions);
                Response.Cookies.Append("MaKH", maKH, cookieOptions);

                // Tạo ClaimsIdentity và đăng nhập
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, maKH),
            new Claim(ClaimTypes.Role, role)
        };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
                });

                TempData["dangNhap"] = "Đăng nhập thành công!";

                // Chuyển hướng dựa trên role
                if (string.Equals(role, "Admin", StringComparison.OrdinalIgnoreCase))
                {
                    return RedirectToAction("Index", "HomeAdmin");
                }

                TempData["dangNhap"] = "Đăng nhập thành công! Bạn có thể mua hàng.";
                return RedirectToAction("Index", "Home");
            }

            TempData["dangNhap"] = "Đăng nhập thất bại. Vui lòng thử lại!";
            return RedirectToAction("Index", "Home");
        }
        // Kiểm tra trạng thái đăng nhập
        public IActionResult CheckLoginStatus()
        {
            var token = Request.Cookies["JwtToken"];
            var role = Request.Cookies["UserRole"];
            var maKH = Request.Cookies["MaKH"];
            var claims = User.Claims.Select(c => new { c.Type, c.Value }); // Log claims
            Console.WriteLine($"Claims: {JsonSerializer.Serialize(claims)}");
            if (string.IsNullOrEmpty(token))
            {
                return Json(new { isLoggedIn = false });
            }
            return Json(new { isLoggedIn = true, role, maKH, isAdmin = (role == "Admin") });
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            var response = await _httpClient.PostAsync("https://localhost:7101/api/Authentication/logout", null);

            if (response.IsSuccessStatusCode)
            {
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTime.UtcNow.AddDays(-1) // Đặt thời gian hết hạn về quá khứ để xóa cookie
                };

                // Xóa các cookie tùy chỉnh
                Response.Cookies.Delete("JwtToken", cookieOptions);
                Response.Cookies.Delete("UserRole", cookieOptions);
                Response.Cookies.Delete("MaKH", cookieOptions);

                // Đăng xuất khỏi Cookie Authentication (xóa AspNetCore.Cookies)
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                TempData["dangXuat"] = "Đăng xuất thành công!";
                return RedirectToAction("Index", "Home");
            }

            return BadRequest(new { success = false, message = "Đăng xuất thất bại!" });
        }

    }
}
