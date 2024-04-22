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
        IQueryable<BalanceEntry> queryable =
            _dbContext.BalanceEntries.Include(e => e.Category).Where(predicate);
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
                queryable.Include(c => c.Category).Skip(filter?.StartIndex ?? 0).Take(rowsCount)).ToArrayAsync(),
            TotalRowCount = await _dbContext.BalanceEntries.CountAsync(predicate)
        };
    }

    public async Task<BalanceEntryDto?> GetBalanceEntryAsync(int id)
    {
        var entry = await _dbContext.BalanceEntries
            .Include(e => e.Category).FirstOrDefaultAsync(e => e.Id == id);
        if (entry == null) return null;
        return _mapper.Map<BalanceEntryDto>(entry);
    }

    public async Task<BalanceEntryDto> CreateBalanceEntryAsync(BalanceEntryDto entry)
    {
        var category = await _dbContext.Categories.FindAsync(entry.Category!.Id);

        var entryToCreate = new BalanceEntry
        {
            EntryDate = entry.EntryDate ?? DateTime.Now,
            Description = entry.Description ?? "",
            Amount = entry?.Amount ?? 0,
            Category = category
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

    public Task<SummaryData[]> GetSummaryDataAsync(DateTime from, DateTime to, string groupBy, bool expenses = true)
    {
        return _dbContext.BalanceEntries.Include(p => p.Category)
            .Where(e => expenses ? e.Amount <= 0 : e.Amount > 0)
            .Where(e => e.EntryDate >= from && e.EntryDate < to)
            .GroupBy(GetGrouping(groupBy))
            .Select(gr =>
                new SummaryData
                {
                    Category = gr.Key.ToString(),
                    Amount = gr.Sum(c => c.Amount < 0 ? -c.Amount : c.Amount )
                }).ToArrayAsync();
    }

    private Expression<Func<BalanceEntry, object>> GetGrouping(string groupBy)
    {
        return groupBy?.ToLower() switch
        {
            "category" => p => p.Category != null ? p.Category.Name : "",
            "entrydate" => p => p.EntryDate,
            _ => p => p.EntryDate,
        };
    }

    private Expression<Func<BalanceEntry, object>> GetSorting(Filter? filter)
    {
        return filter?.SortBy?.ToLower() switch
        {
            "entrydate" => p => p.EntryDate,
            "amount" => p => p.Amount,
            "description" => p => p.Description,
            "category" => p => p.Category.Name,
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
            resultPredicate = resultPredicate.And(p => p.EntryDate < filter.To.Value.AddDays(1));
        }
        return resultPredicate;
    }

}
