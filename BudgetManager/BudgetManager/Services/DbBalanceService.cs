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

    public async Task<BalanceEntryDto?> GetBalanceEntryAsync(int id)
    {
        var entry = await _dbContext.BalanceEntries.FindAsync(id);
        if (entry == null) return null;
        return _mapper.Map<BalanceEntryDto>(entry);
    }

    public async Task<BalanceEntryDto> CreateBalanceEntryAsync(BalanceEntryDto entry)
    {
        var entryToCreate = new BalanceEntry
        {
            EntryDate = entry.EntryDate ?? DateTime.Now,
            Description = entry.Description ?? "",
            Amount = entry.Amount,
        };
        _dbContext.BalanceEntries.Add(entryToCreate);
        await _dbContext.SaveChangesAsync();
        return _mapper.Map<BalanceEntryDto>(entryToCreate);
    }

    public async Task<BalanceEntryDto?> EditEntryAsync(int id, BalanceEntryDto balanceEntry)
    {
        var entry = await _dbContext.BalanceEntries.FindAsync(id);
        if (entry == null)
        {
            return null;
        }
        var editedEntry = _mapper.Map<BalanceEntry>(balanceEntry);
        entry.Description = editedEntry.Description;
        entry.Amount = editedEntry.Amount;
        entry.EntryDate = editedEntry.EntryDate;
        entry.Description = editedEntry.Description;
        await _dbContext.SaveChangesAsync();
        return _mapper.Map<BalanceEntryDto>(entry);
    }

    public async Task DeleteEntryAsync(int id)
    {
        var entry = await _dbContext.BalanceEntries.FindAsync(id);
        if (entry == null)
            throw new ArgumentException($"Entry with id {id} doesn't exits");

        _dbContext.BalanceEntries.Remove(entry);
        await _dbContext.SaveChangesAsync();
    }

    private Expression<Func<BalanceEntry, object>> GetSorting(Filter? filter)
    {
        return filter?.SortBy?.ToLower() switch
        {
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
