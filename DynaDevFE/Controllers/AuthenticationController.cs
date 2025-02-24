﻿using DynaDevFE.Models;
using Microsoft.AspNetCore.Mvc;
using System;

using System.Net.Http;
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
                    TinhTrang = "",
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

 

        // Đăng nhập
        [HttpPost]
        public async Task<IActionResult> Login(Login model)

        {
            if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
            {
                return BadRequest(new { success = false, message = "Email và mật khẩu không được để trống" });
            }


            var content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");

            var loginModel = new Login
            {
                Email = model.Email,
                Password = model.Password
            };

            var content = new StringContent(JsonSerializer.Serialize(loginModel), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://localhost:7101/api/Authentication/login", content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();

                Console.WriteLine("🔹 Response từ API: " + responseContent); // ✅ Kiểm tra API trả về gì

                // ✅ Deserialize JSON từ API
                var result = JsonSerializer.Deserialize<TokenResponse>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true // ✅ Cho phép không phân biệt hoa-thường
                });

                if (result == null || string.IsNullOrEmpty(result.Token) || string.IsNullOrEmpty(result.MaKH))
                {
                    return BadRequest(new { success = false, message = "Lỗi xác thực, không tìm thấy MaKH hoặc Token!" });
                }

                var token = result.Token;
                var role = result.Role ?? "User";
                var maKH = result.MaKH; // ✅ Lấy MaKH từ API


                Console.WriteLine("🔹 Response từ API: " + responseContent);
                var result = JsonSerializer.Deserialize<TokenResponse>(responseContent);

                if (result == null || string.IsNullOrEmpty(result.Token))
                {
                    return BadRequest(new { success = false, message = "Lỗi xác thực, token rỗng!" });
                }

                var token = result.Token;
                var role = result.Role ?? "User"; // Nếu không có role thì mặc định là User

                // ✅ Lưu token vào Cookie

                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTime.UtcNow.AddMinutes(30)
                };


                // ✅ Lưu token, role và MaKH vào Cookie
                Response.Cookies.Append("JwtToken", token, cookieOptions);
                Response.Cookies.Append("UserRole", role, cookieOptions);
                Response.Cookies.Append("MaKH", maKH, cookieOptions);

                return Ok(new { success = true, message = "Đăng nhập thành công!", MaKH = maKH });

                Response.Cookies.Append("JwtToken", token, cookieOptions);
                Response.Cookies.Append("UserRole", role, cookieOptions);

                // ✅ Kiểm tra vai trò và trả về thông báo phù hợp
                if (role == "Admin")
                {
                    return Ok(new { success = true, message = "Đăng nhập thành công! Chào mừng Admin!" });
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            return BadRequest(new { success = false, message = "Đăng nhập thất bại!" });
        }
    }
}
