using BudgetManager.Client.Pages;
using BudgetManager.Shared;
using Microsoft.FluentUI.AspNetCore.Components;
using System.Net.Http.Json;

namespace BudgetManager.Client.Services;

public class ApiBalanceService : IBalanceService
{
    private readonly HttpClient _httpClient;

    public ApiBalanceService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<BalanceEntryDto> CreateBalanceEntryAsync(BalanceEntryDto entry)
    {
        const string path = "api/balanceEntry";
        var response = await _httpClient.PostAsJsonAsync(path, entry);
        response.EnsureSuccessStatusCode();
        BalanceEntryDto? createdEntry = await response.Content.ReadFromJsonAsync<BalanceEntryDto>();
        if (createdEntry == null)
        {
            throw new Exception("Error occured during balance entry creation!");
        }
        return createdEntry;
    }

    public async Task<BalanceEntryDto?> GetBalanceEntryAsync(int id)
    {
        string path = $"api/balanceEntry/{id}";
        BalanceEntryDto? response = null;
        try
        {
            response = await _httpClient.GetFromJsonAsync<BalanceEntryDto?>(path);
        }
        catch (Exception)
        {
            return null;
        }
        return response;
    }

    public async Task<SearchEntriesResult> GetBalanceEntriesAsync(Filter? filter)
    {
        UriBuilder uriBuilder = new UriBuilder();
        uriBuilder.Path = "api/balanceEntry";
        uriBuilder.Query = BuildSearchQueryString(filter?.From,
            filter?.To, filter?.RowCount ?? 20,
            filter?.SortBy, filter?.Order,
            filter?.StartIndex ?? 0);
        var searchResult =
            await _httpClient.GetFromJsonAsync<SearchEntriesResult>(uriBuilder.Uri.PathAndQuery);
        if (searchResult == null)
        {
            throw new Exception("Api is not available");
        }
        return searchResult;
    }

    public async Task<BalanceEntryDto?> EditEntryAsync(int id,
        BalanceEntryDto balanceEntry)
    {
        string path = $"api/balanceEntry/{id}";
        BalanceEntryDto? result = null;
        try
        {
            HttpResponseMessage response =
                await _httpClient.PutAsJsonAsync(path, balanceEntry);
            response.EnsureSuccessStatusCode();
            result =
                await response.Content.ReadFromJsonAsync<BalanceEntryDto?>();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Exception during Edit of an entry: {e}");
            throw;
        }
        return result;
    }

    public async Task DeleteEntryAsync(int id)
    {
        string path = $"api/balanceEntry/{id}";
        try
        {
            HttpResponseMessage response =
                await _httpClient.DeleteAsync(path);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Exception during Edit of an entry: {e}");
            throw;
        }
    }

    public async Task<SummaryData[]> GetSummaryDataAsync(DateTime from, DateTime to, string groupBy, bool expenses = false)
    {
        UriBuilder uriBuilder = new UriBuilder();
        uriBuilder.Path = "api/SummaryData";
        uriBuilder.Query = $"?from={from.ToString("s")}&to={to.ToString("s")}&groupBy={groupBy}&expenses={expenses.ToString()}";

        var result = await _httpClient.GetFromJsonAsync<SummaryData[]>(uriBuilder.Uri.PathAndQuery);
        if (result == null)
        {
            throw new Exception("Api is not available");
        }

        return result;
    }

    private string BuildSearchQueryString(DateTime? from = null,
        DateTime? to = null, int rowsCount = 20, string? sortBy = null,
        string? order = null, int startIndex = 0)
    {
        string query = $"?rowCount={rowsCount}&order={order ?? "asc"}&startIndex={startIndex}&";

        if (from != null)
        {
            query += $"from={from.ToIsoDateString()}&";
        }
        if (to != null)
        {
            query += $"to={to.ToIsoDateString()}&";
        }
        if (!string.IsNullOrEmpty(sortBy))
        {
            query += $"sortBy={sortBy}&";
        }

        return query.TrimEnd('&');
    }
}
