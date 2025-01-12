using DynaDevFE.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using DynaDevFE.Models;
using DynaDevAPI.Models;
using System.Text;


namespace DynaDevFE.Controllers
{
    public class ProductsController : Controller
    {

        private readonly HttpClient _httpClient;

        public ProductsController(HttpClient httpClient)
        {
            _httpClient = httpClient;
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
        public IActionResult Create()
        {
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
                    TenSanPham = productViewModel.TenSanPham,
                    Gia = productViewModel.Gia,
                    MoTa = productViewModel.MoTa,
                    SoLuongTrongKho = productViewModel.SoLuongTrongKho,
                    NgayThem = DateTime.UtcNow,
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

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var response = await _httpClient.GetAsync($"api/Products/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return NotFound("Không tìm thấy sản phẩm.");
            }

            var productJson = await response.Content.ReadAsStringAsync();
            var product = JsonSerializer.Deserialize<SanPhamViewModel>(productJson, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return View(product);
        }


        // POST: Products/Edit/{id}
        [HttpPost]
        public async Task<IActionResult> Edit(SanPhamViewModel productViewModel, string[] RemovedImages, IFormFileCollection AnhSPs)
        {

            if (!ModelState.IsValid)
            {        
                return View(productViewModel);
            }


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

            // 3. Cập nhật sản phẩm
            var jsonData = JsonSerializer.Serialize(productViewModel);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"api/Products/{productViewModel.MaSP}", content);
            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Cập nhật sản phẩm thất bại.");
                return View(productViewModel);
            }

            return RedirectToAction("Index");
        }

  

    }
}

