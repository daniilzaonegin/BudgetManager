using BudgetManager.Data;
using BudgetManager.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BudgetManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class SummaryDataController : ControllerBase
    {
        private readonly IBalanceService _balanceService;

        public SummaryDataController(IBalanceService balanceService)
        {
            _balanceService = balanceService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SummaryData[]))]
        public async Task<IActionResult> GetData(DateTime from, DateTime to, string groupBy, bool expenses = true)
        {
            return Ok(await _balanceService.GetSummaryDataAsync(from, to, groupBy, expenses));
        }
    }
}
