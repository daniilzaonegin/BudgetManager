using AutoMapper;
using BudgetManager.Data;
using BudgetManager.Shared;
using Microsoft.EntityFrameworkCore;

namespace BudgetManager.Services
{
    public class DbCategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public DbCategoryService(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public Task<CategoryDto[]> GetCategoriesAsync()
        {
            return _mapper.ProjectTo<CategoryDto>(_dbContext.Categories).ToArrayAsync();
        }
    }
}
