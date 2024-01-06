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

    public async Task<SearchEntriesResult> GetBalanceEntriesAsync(Filter? filter)
    {
        UriBuilder uriBuilder = new UriBuilder();
        uriBuilder.Path = "api/balanceEntry";
        uriBuilder.Query = BuildQueryString(filter?.From, 
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

    private string BuildQueryString(DateTime? from = null,
        DateTime? to = null, int rowsCount = 20, string? sortBy = null, string? order = null, int startIndex = 0)
    {
        string query = $"?rowsCount={rowsCount}&order={order ?? "asc"}&startIndex={startIndex}&";

        if(from != null)
        {
            query += $"from={from.ToIsoDateString()}&";
        }
        if (to != null)
        {
            query += $"to={to.ToIsoDateString()}&";
        }
        if(!string.IsNullOrEmpty(sortBy))
        {
            query += $"sortBy={sortBy}&";
        }

        return query.TrimEnd('&');
    }
}
