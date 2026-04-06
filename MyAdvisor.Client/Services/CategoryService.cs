using MyAdvisor.Client.Models.Category;
using MyAdvisor.Client.Models.Common;
using System.Net.Http.Json;

namespace MyAdvisor.Client.Services;

public class CategoryService(HttpClient http)
{
    public async Task<List<CategoryModel>> GetAllAsync()
    {
        var res = await http.GetAsync("/api/category");
        if (!res.IsSuccessStatusCode)
        {
            var data = await res.Content.ReadFromJsonAsync<ErrorResponse>();
            throw new Exception(data?.Error ?? "Failed to load categories.");
        }
        return await res.Content.ReadFromJsonAsync<List<CategoryModel>>() ?? [];
    }
}
