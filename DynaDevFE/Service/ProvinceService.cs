using DynaDevFE.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

public class ProvinceService
{
    private readonly HttpClient _httpClient;

    public ProvinceService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<Province>> GetProvincesAsync()
    {
        var response = await _httpClient.GetAsync("https://provinces.open-api.vn/api/?depth=1");
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<Province>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }
}
