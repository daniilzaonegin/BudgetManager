using AutoMapper;
using BudgetManager.Data;
using BudgetManager.Shared;
using BudgetManager.Utils;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BudgetManager.Services;

public class DbBalanceService : IBalanceService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public DbBalanceService(ApplicationDbContext dbContext,
        IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<SearchEntriesResult> GetBalanceEntriesAsync(Filter? filter)
    {
        var predicate = GetFilterPredicate(filter);
        int rowsCount = filter?.RowCount ?? 20;
        var orderBy = GetSorting(filter);
        IQueryable<BalanceEntry> queryable = _dbContext.BalanceEntries.Where(predicate);
        if (filter?.Order == "desc")
        {
            queryable = queryable.OrderByDescending(orderBy);
        }
        else
        {
            queryable = queryable.OrderBy(orderBy);
        }

        return new SearchEntriesResult
        {
            Items = await _mapper.ProjectTo<BalanceEntryDto>(
                queryable.Skip(filter?.StartIndex ?? 0).Take(rowsCount)).ToArrayAsync(),
            TotalRowCount = await _dbContext.BalanceEntries.CountAsync(predicate)
        };
    }

    private Expression<Func<BalanceEntry, object>> GetSorting(Filter? filter)
    {
        return filter?.SortBy?.ToLower() switch
        {
            "isexpense" => p => p.IsExpense,
            "entryDate" => p => p.EntryDate,
            "amount" => p => p.Amount,
            "description" => p => p.Description,
            _ => p => p.Id
        };
    }

    private Expression<Func<BalanceEntry, bool>> GetFilterPredicate(Filter? filter)
    {
        var resultPredicate = PredicateBuilder.True<BalanceEntry>();
        if (filter?.From != null)
        {
            resultPredicate = resultPredicate.And(p => p.EntryDate >= filter.From);
        }
        if (filter?.To != null)
        {
            resultPredicate = resultPredicate.And(p => p.EntryDate <= filter.To);
        }
        return resultPredicate;
    }
}
