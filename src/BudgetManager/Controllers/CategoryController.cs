using BudgetManager.Data;
using BudgetManager.Shared;
using Microsoft.AspNetCore.Mvc;

namespace BudgetManager.Controllers;

[Route("api/[controller]")]
[Produces("application/json")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CategoryDto[]))]
    public async Task<IActionResult> GetCategoriesAsync(bool expenses = true)
    {
        return Ok(await _categoryService.GetCategoriesAsync(expenses));
    }
}
