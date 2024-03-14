using BudgetManager.Shared;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http.Json;

namespace BudgetManager.Client.Services;

public class ApiCategoryService : ICategoryService
{
    private readonly HttpClient _httpClient;

    public ApiCategoryService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<CategoryDto[]> GetCategoriesAsync()
    {
        string path = "api/category";

        var categories = await _httpClient.GetFromJsonAsync<CategoryDto[]>(path);

        return categories ?? [];
    }
}
