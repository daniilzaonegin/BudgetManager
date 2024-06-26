﻿
namespace BudgetManager.Shared;

public interface IBalanceService
{
    Task<SearchEntriesResult> GetBalanceEntriesAsync(Filter? filter);
    Task<BalanceEntryDto> CreateBalanceEntryAsync(BalanceEntryDto entry);
    Task<BalanceEntryDto?> GetBalanceEntryAsync(int id);
    Task<BalanceEntryDto?> EditEntryAsync(int id, BalanceEntryDto balanceEntry);
    Task DeleteEntryAsync(int id);
    Task<SummaryData[]> GetSummaryDataAsync(DateTime from, DateTime to, string groupBy, bool expenses = true);
}
