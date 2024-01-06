namespace BudgetManager.Shared;

public interface IBalanceService
{
    Task<SearchEntriesResult> GetBalanceEntriesAsync(Filter? filter);
}
