using DynaDevAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.WebSockets;
using System.Text.Json;

namespace DynaDevAPI.Data
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public ProductsController(ApplicationDbContext db)
        {
            _db = db;
        }
        [HttpGet]
        public async Task<IActionResult> GetSanPhams()
        {
            var sanPhams = await _db.SanPhams
                                     .Include(sp => sp.LoaiSP)
                                     .Include(sp => sp.AnhSPs)
                                     .ToListAsync();

            // Ánh xạ từ SanPham sang SanPhamDto
            var sanPhamDtos = sanPhams.Select(sp => new SanPhamDto
            {
                MaSP = sp.MaSP,
                TenSanPham = sp.TenSanPham,
                MaLoai = sp.MaLoai,
                TacGia = sp.TacGia,
                MaNCC = sp.MaNCC,
                //NhaXuatBan = sp.NhaXuatBan,
                NamXuatBan = sp.NamXuatBan,
                SoLuongTrongKho = sp.SoLuongTrongKho,
                Gia = sp.Gia,
                MoTa = sp.MoTa,
                TinhTrang = sp.TinhTrang,
                DanhSachAnh = sp.AnhSPs?.Select(a => a.TenAnh).ToList(),
                LoaiSP = sp.LoaiSP?.TenLoai
            }).ToList();

            return Ok(sanPhamDtos);
        }
        //Get chitietsanpham


        // GET: api/SanPham/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSanPham(string id)
        {
            var sanPham = await _db.SanPhams
                                   .Include(sp => sp.AnhSPs)
                                   .Include(sp => sp.NhaCungCap)// Include bảng AnhSPs
                                   .FirstOrDefaultAsync(sp => sp.MaSP == id);

            if (sanPham == null)
            {
                return NotFound("Không tìm thấy sản phẩm.");
            }

            // Tạo DTO trả về
            var sanPhamDto = new SanPhamDto
            {
                MaSP = sanPham.MaSP,
                MaLoai = sanPham.MaLoai,
                SoLuongTrongKho = sanPham.SoLuongTrongKho,
                TenSanPham = sanPham.TenSanPham,
                TacGia = sanPham.TacGia,
                TenNCC = sanPham.NhaXuatBan,
                MaNCC = sanPham.MaNCC,
                NamXuatBan = sanPham.NamXuatBan,
                Gia = sanPham.Gia,
                MoTa = sanPham.MoTa,
                TinhTrang = sanPham.TinhTrang,
                DanhSachAnh = sanPham.AnhSPs.Select(a => a.TenAnh).ToList()
            };

            return Ok(sanPhamDto);
        }


        [HttpGet("GetImagesByProduct/{maSP}")]
        public async Task<IActionResult> GetImagesByProduct(string maSP)
        {
            // Lấy danh sách ảnh theo mã sản phẩm
            var images = await _db.AnhSPs
                                  .Where(a => a.MaSP == maSP)
                                  .Select(a => new
                                  {
                                      a.MaAnh,
                                      a.TenAnh
                                  })
                                  .ToListAsync();

            if (!images.Any())
            {
                return NotFound($"Không tìm thấy ảnh cho sản phẩm có mã: {maSP}");
            }

            return Ok(images); // Trả về danh sách ảnh
        }






        [HttpGet("Search")]
        public async Task<ActionResult<IEnumerable<SanPham>>> SearchSanPham([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest(new { message = "Từ khóa tìm kiếm không hợp lệ." });
            }

            try
            {
                // Chuyển query về dạng chữ thường để tìm kiếm không phân biệt hoa thường
                var lowercaseQuery = query.ToLower();

                // Truy vấn tìm kiếm các sản phẩm
                var results = await _db.SanPhams
                    .Where(kh =>
                        kh.MaSP.ToLower().Contains(lowercaseQuery) ||
                        kh.TenSanPham.ToLower().Contains(lowercaseQuery) ||
                        kh.MoTa.ToLower().Contains(lowercaseQuery) ||
                        kh.TacGia.ToLower().Contains(lowercaseQuery) ||
                        kh.Gia.ToString().ToLower().Contains(lowercaseQuery) ||
                        kh.TinhTrang.ToLower().Contains(lowercaseQuery) ||
                        kh.MaNCC.ToLower().Contains(lowercaseQuery))
                    .ToListAsync();

                // Nếu không có kết quả
                if (results == null || !results.Any())
                {
                    return NotFound(new { message = "Không tìm thấy sản phẩm nào phù hợp." });
                }

                // Lấy danh sách ảnh cho mỗi sản phẩm
                var result = results.Select(kh => new
                {
                    kh.MaSP,
                    kh.TenSanPham,
                    kh.TacGia,
                    kh.Gia,
                    kh.MoTa,
                    kh.TinhTrang,
                    kh.NhaXuatBan,
                    kh.NamXuatBan,
                    DanhSachAnh = _db.AnhSPs
                        .Where(a => a.MaSP == kh.MaSP)
                        .Select(a => a.TenAnh)
                        .ToList()
                }).ToList();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi tìm kiếm sản phẩm.", error = ex.Message });
            }
        }





        [HttpPost("UploadImages")]
        public async Task<IActionResult> UploadImages(List<IFormFile> files, string maSP)
        {
            try
            {
                var uploadedPaths = new List<string>();

                if (files == null || !files.Any())
                {
                    Console.WriteLine("No files received.");
                    return BadRequest("Không có file nào được tải lên.");
                }

                if (string.IsNullOrEmpty(maSP))
                {
                    Console.WriteLine("No maSP provided.");
                    return BadRequest("Mã sản phẩm không được cung cấp.");
                }

                // Xác định đường dẫn tới DynaDevFE/wwwroot/Products
                var fePath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName, "DynaDevFE");
                var folderPath = Path.Combine(fePath, "wwwroot/Products");

                // Tạo thư mục nếu chưa tồn tại
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                foreach (var file in files)
                {
                    if (file.ContentType.StartsWith("image"))
                    {
                        var sanitizedFileName = Path.GetFileNameWithoutExtension(file.FileName)
                                              .Replace(" ", "_")
                                              .Replace(":", "-")
                                              .Replace("/", "-") + Path.GetExtension(file.FileName);

                        var filePath = Path.Combine(folderPath, $"{Guid.NewGuid()}_{sanitizedFileName}");

                        try
                        {
                            // Lưu file vào thư mục
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }

                            // Lưu đường dẫn tương đối để trả về cho Frontend
                            var relativePath = $"/Products/{Path.GetFileName(filePath)}";
                            uploadedPaths.Add(relativePath);

                            // Tạo đối tượng AnhSP và lưu vào database
                            var anhSP = new AnhSP
                            {
                                MaAnh = Guid.NewGuid().ToString(),
                                MaSP = maSP,
                                TenAnh = relativePath
                            };

                            Console.WriteLine($"Adding AnhSP: MaAnh={anhSP.MaAnh}, MaSP={anhSP.MaSP}, TenAnh={anhSP.TenAnh}");
                            _db.AnhSPs.Add(anhSP);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error saving file: {ex.Message}");
                            return StatusCode(StatusCodes.Status500InternalServerError, $"Lỗi khi lưu file: {ex.Message}");
                        }
                    }
                }

                // Lưu thông tin vào database
                try
                {
                    await _db.SaveChangesAsync();
                    Console.WriteLine("Saved changes to database successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error saving data to database: {ex.Message}");
                    return StatusCode(StatusCodes.Status500InternalServerError, $"Lỗi khi lưu dữ liệu ảnh: {ex.Message}");
                }

                return Ok(uploadedPaths); // Trả về danh sách đường dẫn file đã upload
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unhandled error: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Unhandled error: {ex.Message}");
            }
        }



        // POST: api/SanPham
        [HttpPost]
        public async Task<IActionResult> CreateSanPham([FromBody] SanPham sanPham)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }               


            //if (sanPham.AnhSPs != null && sanPham.AnhSPs.Any())
            //{
            //    foreach (var anh in sanPham.AnhSPs)
            //    {
            //        anh.MaSP = sanPham.MaSP; // Gắn mã sản phẩm cho ảnh
            //        _db.AnhSPs.Add(anh);
            //    }
            //}

            var nhaCungCap = await _db.NhaCungCaps.FirstOrDefaultAsync(ncc => ncc.MaNCC == sanPham.MaNCC);
            if (nhaCungCap == null)
            {
                return BadRequest(new { success = false, message = "Nhà cung cấp không tồn tại." });
            }

            // Gán Tên Nhà Cung Cấp làm Nhà Xuất Bản
            sanPham.NhaXuatBan = nhaCungCap.TenNCC;
            sanPham.NgayThem = DateTime.Now;

            try
            {
                _db.SanPhams.Add(sanPham);
                await _db.SaveChangesAsync();
                return Ok(new { success = true, message = "Sản phẩm đã được thêm thành công!" });
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, new { success = false, message = "Lỗi nội bộ máy chủ.", error = ex.Message });
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSanPham(string id, [FromBody] SanPham updatedSanPham)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            // Tìm sản phẩm theo mã sản phẩm
            var existingSanPham = await _db.SanPhams.Include(s => s.AnhSPs).FirstOrDefaultAsync(sp => sp.MaSP == id);
            if (existingSanPham == null)
            {
                return NotFound($"Không tìm thấy sản phẩm với mã {id}");
            }

            // Cập nhật thông tin sản phẩm
            existingSanPham.TenSanPham = updatedSanPham.TenSanPham;
            existingSanPham.TacGia = updatedSanPham.TacGia;
            //existingSanPham.NhaXuatBan = updatedSanPham.NhaXuatBan;
            existingSanPham.NamXuatBan = updatedSanPham.NamXuatBan;
            existingSanPham.MaLoai = updatedSanPham.MaLoai;
            existingSanPham.Gia = updatedSanPham.Gia;
            existingSanPham.MoTa = updatedSanPham.MoTa;
            existingSanPham.SoLuongTrongKho = updatedSanPham.SoLuongTrongKho;
            existingSanPham.TinhTrang = updatedSanPham.TinhTrang;

            // Xử lý danh sách ảnh
            if (updatedSanPham.AnhSPs != null && updatedSanPham.AnhSPs.Any())
            {
                // Xóa các ảnh cũ khỏi hệ thống tệp
                foreach (var oldImage in existingSanPham.AnhSPs)
                {
                    var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", oldImage.TenAnh.TrimStart('/'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                // Xóa các ảnh cũ khỏi cơ sở dữ liệu
                _db.AnhSPs.RemoveRange(existingSanPham.AnhSPs);

                // Thêm các ảnh mới
                foreach (var anh in updatedSanPham.AnhSPs)
                {
                    anh.MaSP = id; // Gắn mã sản phẩm cho ảnh
                    anh.MaAnh = Guid.NewGuid().ToString(); // Tạo mã ảnh mới
                    _db.AnhSPs.Add(anh);
                }
            }

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Ghi log lỗi đầy đủ (nên sử dụng công cụ logging, ví dụ: Serilog, NLog, hoặc ILogger)
                Console.WriteLine($"Error updating product: {ex.Message}");
                return StatusCode(500, "Lỗi khi cập nhật sản phẩm.");
            }

            return Ok(existingSanPham);
        }

        [HttpPost("DeleteImages")]
        public async Task<IActionResult> DeleteImages([FromBody] List<string> images)
        {
            if (images == null || !images.Any())
            {
                return BadRequest("Danh sách ảnh không hợp lệ.");
            }

            // Lọc danh sách ảnh trong cơ sở dữ liệu
            var imageEntities = await _db.AnhSPs.Where(a => images.Contains(a.TenAnh)).ToListAsync();

            foreach (var imageEntity in imageEntities)
            {
                try
                {
                    // Xóa file khỏi hệ thống
                    var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imageEntity.TenAnh.TrimStart('/'));
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }

                    // Xóa bản ghi trong cơ sở dữ liệu
                    _db.AnhSPs.Remove(imageEntity);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Lỗi khi xóa ảnh: {imageEntity.TenAnh}. Chi tiết lỗi: {ex.Message}");
                }
            }

            await _db.SaveChangesAsync();
            return Ok("Đã xóa ảnh thành công.");
        }

        [HttpDelete("{maSP}")]
        public IActionResult Delete(string maSP)
        {
            try
            {
                var product = _db.SanPhams.FirstOrDefault(p => p.MaSP == maSP);
                if (product == null)
                {
                    return NotFound(new { message = "Sản phẩm không tồn tại" });
                }

                _db.SanPhams.Remove(product);
                _db.SaveChanges();

                return Ok(new { message = "Xóa sản phẩm thành công" });
            }
            catch (Exception ex)
            {
                // Trả về lỗi dưới dạng JSON thay vì exception
                return StatusCode(500, new { message = "Đã xảy ra lỗi trong quá trình xóa sản phẩm.", error = ex.Message });
            }
        }

    }
}
