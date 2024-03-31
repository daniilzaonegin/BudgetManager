using AutoMapper;
using BudgetManager.Shared;
using Microsoft.AspNetCore.Mvc;

namespace BudgetManager.Controllers;

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
    [Produces("application/json")]
    [ProducesResponseType(200, Type = typeof(SearchEntriesResult))]
    public async Task<IActionResult> GetEntries([FromQuery] Filter filter)
    {
        var result = await _apiClient.GetBalanceEntriesAsync(filter);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    [Produces("application/json")]
    [ProducesResponseType(200, Type = typeof(BalanceEntryDto))]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetEntry(int id)
    {
        var result = await _apiClient.GetBalanceEntryAsync(id);
        if (result == null)
        {
            return NotFound();
        }
        return Ok(result);
    }

    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(200, Type = typeof(BalanceEntryDto))]
    public async Task<IActionResult> CreateEntry(BalanceEntryDto balanceEntry)
    {
        var result = await _apiClient.CreateBalanceEntryAsync(balanceEntry);
        return Ok(result);
    }

    [HttpPut("{id:int}")]
    [Produces("application/json")]
    [ProducesResponseType(200, Type = typeof(BalanceEntryDto))]
    [ProducesResponseType(404, Type = typeof(int))]
    public async Task<IActionResult> EditEntry(int id, BalanceEntryDto balanceEntry)
    {
        var result = await _apiClient.EditEntryAsync(id, balanceEntry);
        if (result == null)
        {
            return NotFound(id);
        }
        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> DeleteEntry(int id)
    {
        try
        {
            await _apiClient.DeleteEntryAsync(id);
        }
        catch (ArgumentException)
        {
            return NotFound($"Entry with specified id={id} was not found");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        return Ok();
    }
}
