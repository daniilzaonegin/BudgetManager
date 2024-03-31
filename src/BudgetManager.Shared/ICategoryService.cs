namespace BudgetManager.Shared
{
    public interface ICategoryService
    {
        Task<CategoryDto[]> GetCategoriesAsync(bool expenses = true);
    }
}
