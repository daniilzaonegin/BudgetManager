using AutoMapper;
using BudgetManager.Data;
using BudgetManager.Shared;
using BudgetManager.Utils;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BudgetManager.Services;

public class DbDataApiClinet: IApiClient
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public DbDataApiClinet(ApplicationDbContext dbContext,
        IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<SearchEntriesResult> GetBalanceEntries(DateTime? from, DateTime? to, int rowsCount)
    {

        var predicate = GetFilterPredicate(from, to);
        return new SearchEntriesResult
        {
            Items = await _mapper.ProjectTo<BalanceEntryDto>(_dbContext.BalanceEntries.Where(predicate).Take(rowsCount)).ToArrayAsync(),
            TotalRowCount = await _dbContext.BalanceEntries.CountAsync()
        };
    }

    private Expression<Func<BalanceEntry, bool>> GetFilterPredicate(DateTime? from, DateTime? to)
    {
        var resultPredicate = PredicateBuilder.True<BalanceEntry>();
        if (from != null)
        {
            resultPredicate = resultPredicate.And(p => p.EntryDate >= from);
        }
        if (to != null)
        {
            resultPredicate = resultPredicate.And(p => p.EntryDate <= to);
        }
        return resultPredicate;
    }
}
