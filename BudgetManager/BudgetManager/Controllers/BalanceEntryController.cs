using AutoMapper;
using BudgetManager.Data;
using BudgetManager.Shared;
using BudgetManager.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BudgetManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BalanceEntryController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public BalanceEntryController(ApplicationDbContext dbContext,
            IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(BalanceEntry))]
        public async Task<IActionResult> GetEntries([FromQuery] Filter filter)
        {
            var predicate = GetFilterPredicate(filter);
            var result = new SearchEntriesResult
            {
                Items = await _mapper.ProjectTo<BalanceEntryDto>(_dbContext.BalanceEntries.Where(predicate).Take(filter.RowCount)).ToArrayAsync(),
                TotalRowCount = await _dbContext.BalanceEntries.CountAsync()
            };
            return Ok(result);
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

        //if (from != null)
        //{
        //    resultPredicate = resultPredicate.And(p => p.EntryDate >= from);
        //}
        //if (to != null)
        //{
        //    resultPredicate = resultPredicate.And(p => p.EntryDate <= to);
        //}
    }
}
