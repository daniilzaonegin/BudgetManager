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
        private readonly IBalanceService _apiClient;
        private readonly IMapper _mapper;

        public BalanceEntryController(IBalanceService apiClient,
            IMapper mapper)
        {
            _apiClient = apiClient;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(BalanceEntry))]
        public async Task<IActionResult> GetEntries([FromQuery] Filter filter)
        {
            var result = await _apiClient.GetBalanceEntriesAsync(filter);
            return Ok(result);
        }
    }
}
