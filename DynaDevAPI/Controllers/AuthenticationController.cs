using DynaDevAPI.Data;
using DynaDevAPI.Models;
using DynaDevAPI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DynaDevAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        public AuthenticationController(IConfiguration configuration, ApplicationDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }


        [HttpGet]
        public IActionResult GetMaKH()
        {
            string maKH = Request.Cookies["MaKH"];

            if (string.IsNullOrEmpty(maKH))
            {
                return NotFound(new { success = false, message = "Không tìm thấy MaKH trong cookie!" });
            }

            return Ok(new { success = true, MaKH = maKH });
        }


        // Đăng ký người dùng
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] KhachHangVM khach)
        {
            Random rd = new Random();
            int random = rd.Next(0, 1000);
            string MaKH = "KH" + random.ToString("D3");
            if (await _context.KhachHangs.AnyAsync(k => k.Email == khach.Email))
            {
                return BadRequest("Email đã tồn tại.");
            }
            try
            {
                var newKhach = new KhachHang
                {
                    MaKH = MaKH,
                    TenKH = khach.TenKH,
                    Email = khach.Email,
                    MatKhau = BCrypt.Net.BCrypt.HashPassword(khach.MatKhau),  // Mã hóa mật khẩu
                    SDT = khach.SDT,
                    DiaChi = khach.DiaChi,
                    TinhTrang = "Active",
                    NgayDangKy = DateTime.Now
                };
                _context.KhachHangs.Add(newKhach);
                await _context.SaveChangesAsync();

                return Ok("Đăng ký thành công");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Đăng ký thất bại", error = ex.Message });
            }
        }
        // Hàm tạo JWT Token
        private string GenerateJwtToken(KhachHang user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"];
            var issuer = jwtSettings["Issuer"];
            var audience = jwtSettings["Audience"];
            var expireMinutes = int.Parse(jwtSettings["ExpireMinutes"]);

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.MaKH),
        new Claim(ClaimTypes.Email, user.Email),
        new Claim("Role", "User")
    };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(expireMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // Đăng nhập người dùng
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login loginModel)
        {

            // 🔹 Tìm user trong bảng KhachHang theo Email

            var user = await _context.KhachHangs.FirstOrDefaultAsync(k => k.Email == loginModel.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginModel.Password, user.MatKhau))
            {
                return Unauthorized(new { success = false, message = "Email hoặc mật khẩu không đúng." });
            }

            // ✅ Kiểm tra MaKH
            if (string.IsNullOrEmpty(user.MaKH))
            {
                return BadRequest(new { success = false, message = "Không tìm thấy MaKH trong database!" });
            }

            var token = GenerateJwtToken(user);

            // ✅ Trả về Token + MaKH + Role
            return Ok(new
            {
                success = true,
                message = "Đăng nhập thành công",
                token = token,
                MaKH = user.MaKH, // ✅ Trả về MaKH
            });
        }

        [HttpGet("GetUserInfo")]
        public IActionResult GetUserInfo()
        {
            var token = Request.Cookies["JwtToken"];
            var role = Request.Cookies["UserRole"];

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized(new { success = false, message = "Bạn chưa đăng nhập!" });
            }

            return Ok(new { success = true, token, role, isAdmin = (role == "Admin") });
        }
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("JwtToken");
            Response.Cookies.Delete("UserRole");
            Response.Cookies.Delete("MaKH");

            return Ok(new { success = true, message = "Đăng xuất thành công!" });
        }

    }
}
