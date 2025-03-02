using DynaDevFE.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using DynaDevFE.Models;
using DynaDevAPI.Models;
using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;


namespace DynaDevFE.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    public class ProductsController : Controller
    {

        private readonly HttpClient _httpClient;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(HttpClient httpClient, ILogger<ProductsController> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _httpClient.BaseAddress = new Uri("https://localhost:7101/");
        }

        public async Task<IActionResult> Index()
        {
            // URL của API để lấy danh sách sản phẩm
            var apiUrl = "https://localhost:7101/api/Products";
            var response = await _httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();
                var products = JsonSerializer.Deserialize<List<SanPhamViewModel>>(jsonData, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                // Gọi API để lấy danh sách ảnh cho từng sản phẩm
                foreach (var product in products)
                {
                    var imagesResponse = await _httpClient.GetAsync($"https://localhost:7101/api/Products/GetImagesByProduct/{product.MaSP}");
                    if (imagesResponse.IsSuccessStatusCode)
                    {
                        var imagesJson = await imagesResponse.Content.ReadAsStringAsync();
                        var imageObjects = JsonSerializer.Deserialize<List<AnhSP>>(imagesJson, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                        // Chỉ lấy đường dẫn ảnh
                        product.DanhSachAnh = imageObjects.Select(img => img.TenAnh).ToList();
                    }

                }
                return View(products);
            }

            // Nếu có lỗi, trả về View rỗng
            return View(new List<SanPhamViewModel>());
        }


        public async Task<JsonResult> Search(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return Json(new List<SanPhamViewModel>());
            }

            try
            {
                var response = await _httpClient.GetAsync($"api/Products/Search?query={Uri.EscapeDataString(query)}");
                if (response.IsSuccessStatusCode)
                {
                    var jsonData = await response.Content.ReadAsStringAsync();
                    var customers = JsonSerializer.Deserialize<List<SanPhamViewModel>>(jsonData, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    return Json(customers);
                }

                return Json(new List<SanPhamViewModel>()); // Trả về danh sách rỗng nếu không tìm thấy
            }
            catch (Exception)
            {
                return Json(new List<SanPhamViewModel>()); // Xử lý lỗi bằng cách trả về danh sách rỗng
            }
        }




        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound("Mã sản phẩm không hợp lệ.");
            }

            try
            {
                // Gọi API để lấy chi tiết sản phẩm
                var response = await _httpClient.GetAsync($"/api/Products/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var sanPhamDto = await response.Content.ReadFromJsonAsync<SanPhamDto>();

                    // Đóng gói sản phẩm vào danh sách
                    var sanPhamList = new List<SanPhamViewModel>
            {
                new SanPhamViewModel
                {
                    MaSP = sanPhamDto.MaSP,
                    MaLoai = sanPhamDto.MaLoai,
                    TenSanPham = sanPhamDto.TenSanPham,
                    TacGia = sanPhamDto.TacGia,
                    TenNCC = sanPhamDto.TenNCC,
                    NamXuatBan = sanPhamDto.NamXuatBan,
                    Gia = sanPhamDto.Gia,
                    MoTa = sanPhamDto.MoTa,
                    TinhTrang = sanPhamDto.TinhTrang,
                    SoLuongTrongKho = sanPhamDto.SoLuongTrongKho,
                    DanhSachAnh = sanPhamDto.DanhSachAnh
                }
            };

                    return View(sanPhamList); // Truyền danh sách vào View
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


        // GET: Products/Create
        public async Task<IActionResult> Create()
        {
            var response = await _httpClient.GetAsync("api/Suppliers");
            if (response.IsSuccessStatusCode)
            {
                var nhaCungCaps = await response.Content.ReadFromJsonAsync<List<NhaCungCap>>();
                ViewBag.NhaCungCaps = new SelectList(nhaCungCaps, "MaNCC", "TenNCC");
            }
            else
            {
                ViewBag.NhaCungCaps = new SelectList(new List<NhaCungCap>(), "MaNCC", "TenNCC");
                TempData["Error"] = "Không thể lấy danh sách nhà cung cấp.";
            }
            return View();
        }


        // POST: Products/Create
        [HttpPost]
        public async Task<IActionResult> Create(SanPhamViewModel productViewModel)
        {
            var random = new Random();
            int randomvalue = random.Next(0, 999);
            string ma = "SP" + randomvalue.ToString("D3");

            // Thêm sản phẩm vào database
            if (ModelState.IsValid)
            {

                // Ánh xạ từ SanPhamViewModel sang SanPham (model API)
                var product = new SanPham
                {
                    MaSP = ma,
                    MaLoai = productViewModel.MaLoai,
                    MaNCC = productViewModel.MaNCC,
                    TenSanPham = productViewModel.TenSanPham,
                    TacGia = productViewModel.TacGia,
                    NamXuatBan = productViewModel.NamXuatBan,
                    Gia = productViewModel.Gia,
                    MoTa = productViewModel.MoTa,
                    SoLuongTrongKho = productViewModel.SoLuongTrongKho,
                    NgayThem = DateTime.Now,
                    TinhTrang = productViewModel.TinhTrang,
                    AnhSPs = productViewModel.DanhSachAnh.Select((path, index) => new AnhSP
                    {
                        MaAnh = Guid.NewGuid().ToString(), // Tạo mã ảnh tự động
                        MaSP = productViewModel.MaSP, // Gắn mã sản phẩm
                        TenAnh = path,
                    }).ToList()
                };

                // Serialize và gửi dữ liệu tới API
                var jsonData = JsonSerializer.Serialize(product);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("api/Products", content);
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"API Error: {response.StatusCode} - {errorContent}");
                    ModelState.AddModelError("", $"Lỗi từ API: {errorContent}");
                    return View(productViewModel);
                }

                // 1. Upload ảnh sản phẩm
                var uploadedImagePaths = new List<string>();
                if (productViewModel.AnhSPs != null && productViewModel.AnhSPs.Any())
                {
                    using (var formData = new MultipartFormDataContent())
                    {
                        productViewModel.MaSP = product.MaSP;
                        foreach (var file in productViewModel.AnhSPs)
                        {
                            var streamContent = new StreamContent(file.OpenReadStream());
                            streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);
                            formData.Add(streamContent, "files", file.FileName);
                        }

                        // Gọi API UploadImages
                        var uploadResponse = await _httpClient.PostAsync($"https://localhost:7101/api/Products/UploadImages?maSP={productViewModel.MaSP}", formData);
                        if (uploadResponse.IsSuccessStatusCode)
                        {
                            var uploadedPathsJson = await uploadResponse.Content.ReadAsStringAsync();
                            uploadedImagePaths = JsonSerializer.Deserialize<List<string>>(uploadedPathsJson);
                            productViewModel.DanhSachAnh = uploadedImagePaths;
                            TempData["Success"] = "Thêm sản phẩm thành công!";
                        }
                        else
                        {
                            ModelState.AddModelError("", "Không thể upload ảnh sản phẩm.");
                            return View(productViewModel);
                        }
                    }
                }
            }

            return RedirectToAction("Index");
        }



        //[HttpGet]
        //public async Task<IActionResult> Edit(string id)
        //{
        //    var response = await _httpClient.GetAsync($"api/Products/{id}");
        //    if (!response.IsSuccessStatusCode)
        //    {
        //        return NotFound("Không tìm thấy sản phẩm.");
        //    }

        //    var productJson = await response.Content.ReadAsStringAsync();
        //    var product = JsonSerializer.Deserialize<SanPhamViewModel>(productJson, new JsonSerializerOptions
        //    {
        //        PropertyNameCaseInsensitive = true
        //    });

        //    // Lấy danh sách nhà cung cấp
        //    var supplierResponse = await _httpClient.GetAsync("api/Suppliers");
        //    if (supplierResponse.IsSuccessStatusCode)
        //    {
        //        var suppliers = await supplierResponse.Content.ReadFromJsonAsync<List<NhaCungCap>>();
        //        ViewBag.NhaCungCaps = new SelectList(suppliers, "MaNCC", "TenNCC", product.MaNCC);
        //        var suppliersJson = JsonSerializer.Serialize(suppliers);
        //        ViewBag.SuppliersJson = suppliersJson; // Truyền JSON
        //        Console.WriteLine("Suppliers JSON: " + suppliersJson); // Kiểm tra log
        //    }
        //    else
        //    {
        //        ViewBag.NhaCungCaps = new SelectList(new List<NhaCungCap>(), "MaNCC", "TenNCC");
        //        TempData["Error"] = "Không thể lấy danh sách nhà cung cấp.";
        //    }

        //    return View(product);
        //}

        //// POST: Products/Edit/{id}
        //[HttpPost]
        //public async Task<IActionResult> Edit(SanPhamViewModel productViewModel, string[] RemovedImages, IFormFileCollection AnhSPs)
        //{
        //    // 1. Xóa ảnh hiện tại nếu có
        //    if (RemovedImages != null && RemovedImages.Any())
        //    {
        //        var deleteResponse = await _httpClient.PostAsJsonAsync("api/Products/DeleteImages", RemovedImages);
        //        if (!deleteResponse.IsSuccessStatusCode)
        //        {
        //            ModelState.AddModelError("", "Không thể xóa ảnh.");
        //            return View(productViewModel);
        //        }
        //    }

        //    // 2. Thêm ảnh mới nếu có
        //    if (AnhSPs != null && AnhSPs.Any())
        //    {
        //        using (var formData = new MultipartFormDataContent())
        //        {
        //            foreach (var file in AnhSPs)
        //            {
        //                var streamContent = new StreamContent(file.OpenReadStream());
        //                streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);
        //                formData.Add(streamContent, "files", file.FileName);
        //            }

        //            var uploadResponse = await _httpClient.PostAsync($"api/Products/UploadImages?maSP={productViewModel.MaSP}", formData);
        //            if (!uploadResponse.IsSuccessStatusCode)
        //            {
        //                ModelState.AddModelError("", "Không thể upload ảnh mới.");
        //                return View(productViewModel);
        //            }
        //        }
        //    }

        //    // 3. Cập nhật sản phẩm
        //    var jsonData = JsonSerializer.Serialize(productViewModel, new JsonSerializerOptions { WriteIndented = true });
        //    _logger.LogInformation("Gửi dữ liệu cập nhật cho sản phẩm {MaSP}: {JsonData}", productViewModel.MaSP, jsonData);
        //    var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

        //    var response = await _httpClient.PutAsync($"api/Products/{productViewModel.MaSP}", content);
        //    var errorContent = await response.Content.ReadAsStringAsync();
        //    _logger.LogInformation("Phản hồi từ API cho sản phẩm {MaSP}: {StatusCode} - {ErrorContent}", productViewModel.MaSP, response.StatusCode, errorContent);

        //    if (!response.IsSuccessStatusCode)
        //    {
        //        try
        //        {
        //            var error = JsonSerializer.Deserialize<Dictionary<string, string>>(errorContent);
        //            if (error != null && error.ContainsKey("message"))
        //            {
        //                ModelState.AddModelError("", error["message"]);
        //            }
        //            else
        //            {
        //                ModelState.AddModelError("", $"Cập nhật thất bại. Mã lỗi: {response.StatusCode}, Chi tiết: {errorContent}");
        //            }
        //        }
        //        catch (JsonException)
        //        {
        //            ModelState.AddModelError("", $"Cập nhật thất bại. Mã lỗi: {response.StatusCode}, Chi tiết: {errorContent}");
        //        }
        //        return View(productViewModel);
        //    }

        //    return RedirectToAction("Index");
        //}

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var response = await _httpClient.GetAsync($"api/Products/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return NotFound("Không tìm thấy sản phẩm.");
            }

            var productJson = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Phản hồi JSON từ API cho {id}: {productJson}"); // Log toàn bộ JSON để debug
            var product = JsonSerializer.Deserialize<SanPhamViewModel>(productJson, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            // Kiểm tra và log danh sách ảnh
            if (product.DanhSachAnh == null)
            {
                Console.WriteLine($"DanhSachAnh cho sản phẩm {id} là null.");
                product.DanhSachAnh = new List<string>(); // Khởi tạo rỗng nếu null
            }
            else
            {
                Console.WriteLine($"Danh sách ảnh của sản phẩm {id}: {string.Join(", ", product.DanhSachAnh)}");
            }

            // Lấy danh sách nhà cung cấp
            var supplierResponse = await _httpClient.GetAsync("api/Suppliers");
            if (supplierResponse.IsSuccessStatusCode)
            {
                var suppliers = await supplierResponse.Content.ReadFromJsonAsync<List<NhaCungCap>>();
                ViewBag.NhaCungCaps = new SelectList(suppliers, "MaNCC", "TenNCC", product.MaNCC);
                var suppliersJson = JsonSerializer.Serialize(suppliers);
                ViewBag.SuppliersJson = suppliersJson; // Truyền JSON
                Console.WriteLine("Suppliers JSON: " + suppliersJson); // Kiểm tra log
            }
            else
            {
                ViewBag.NhaCungCaps = new SelectList(new List<NhaCungCap>(), "MaNCC", "TenNCC");
                TempData["Error"] = "Không thể lấy danh sách nhà cung cấp.";
            }

            return View(product);
        }

        // POST: Products/Edit/{id}
        [HttpPost]
        public async Task<IActionResult> Edit(SanPhamViewModel productViewModel, string[] RemovedImages, IFormFileCollection AnhSPs)
        {
            // 1. Xóa ảnh hiện tại nếu có
            if (RemovedImages != null && RemovedImages.Any())
            {
                var deleteResponse = await _httpClient.PostAsJsonAsync("api/Products/DeleteImages", RemovedImages);
                if (!deleteResponse.IsSuccessStatusCode)
                {
                    ModelState.AddModelError("", "Không thể xóa ảnh.");
                    return View(productViewModel);
                }
            }

            // 2. Thêm ảnh mới nếu có
            if (AnhSPs != null && AnhSPs.Any())
            {
                using (var formData = new MultipartFormDataContent())
                {
                    foreach (var file in AnhSPs)
                    {
                        var streamContent = new StreamContent(file.OpenReadStream());
                        streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);
                        formData.Add(streamContent, "files", file.FileName);
                    }

                    var uploadResponse = await _httpClient.PostAsync($"api/Products/UploadImages?maSP={productViewModel.MaSP}", formData);
                    if (!uploadResponse.IsSuccessStatusCode)
                    {
                        ModelState.AddModelError("", "Không thể upload ảnh mới.");
                        return View(productViewModel);
                    }
                }
            }

            // 3. Chuẩn bị dữ liệu để cập nhật sản phẩm
            // Tạo một bản sao của productViewModel và loại bỏ AnhSPs nếu không có file
            var productToUpdate = new SanPhamViewModel
            {
                MaSP = productViewModel.MaSP,
                TenSanPham = productViewModel.TenSanPham,
                TacGia = productViewModel.TacGia,
                MaNCC = productViewModel.MaNCC,
                TenNCC = productViewModel.TenNCC,
                NamXuatBan = productViewModel.NamXuatBan,
                MaLoai = productViewModel.MaLoai,
                Gia = productViewModel.Gia,
                MoTa = productViewModel.MoTa,
                SoLuongTrongKho = productViewModel.SoLuongTrongKho,
                TinhTrang = productViewModel.TinhTrang,
                DanhSachAnh = productViewModel.DanhSachAnh ?? new List<string>(), // Không để null
                AnhSPs = productViewModel.AnhSPs?.Any() == true ? productViewModel.AnhSPs : null // Chỉ gửi nếu có ảnh mới
            };


            var jsonData = JsonSerializer.Serialize(productToUpdate, new JsonSerializerOptions { WriteIndented = true });
            _logger.LogInformation("Gửi dữ liệu cập nhật cho sản phẩm {MaSP}: {JsonData}", productViewModel.MaSP, jsonData);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"api/Products/{productViewModel.MaSP}", content);
            var errorContent = await response.Content.ReadAsStringAsync();
            _logger.LogInformation("Phản hồi từ API cho sản phẩm {MaSP}: {StatusCode} - {ErrorContent}", productViewModel.MaSP, response.StatusCode, errorContent);

            if (!response.IsSuccessStatusCode)
            {
                try
                {
                    var error = JsonSerializer.Deserialize<Dictionary<string, string>>(errorContent);
                    if (error != null && error.ContainsKey("message"))
                    {
                        ModelState.AddModelError("", error["message"]);
                    }
                    else
                    {
                        ModelState.AddModelError("", $"Cập nhật thất bại. Mã lỗi: {response.StatusCode}, Chi tiết: {errorContent}");
                    }
                }
                catch (JsonException)
                {
                    ModelState.AddModelError("", $"Cập nhật thất bại. Mã lỗi: {response.StatusCode}, Chi tiết: {errorContent}");
                }
                return View(productViewModel);
            }
            TempData["Success"] = "Sửa sản phẩm thành công!";
            return RedirectToAction("Index");
        }
    }
}

