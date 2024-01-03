using BudgetManager.Shared;
using Microsoft.FluentUI.AspNetCore.Components;
using System.Net.Http.Json;

namespace BudgetManager.Client.Services;

public class ApiClient: IApiClient
{
    private readonly HttpClient _httpClient;

    public ApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<SearchEntriesResult> GetBalanceEntries(DateTime? from = null, DateTime? to = null, int rowsCount = 20)
    {
        UriBuilder uriBuilder = new UriBuilder();
        uriBuilder.Path = "api/balanceEntry";
        uriBuilder.Query = $"?from={from.ToIsoDateString()}&to={to.ToIsoDateString()}&rowCount={rowsCount}";
        var searchResult = 
            await _httpClient.GetFromJsonAsync<SearchEntriesResult>(uriBuilder.Uri.PathAndQuery);
        if(searchResult == null)
        {
            throw new Exception("Api is not available");
        }
        return searchResult;
    }
}
