
using BudgetManager.Data;

namespace BudgetManager.Services;

public class DatabaseSeeder : IHostedService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly Fakers _fakers;
    private readonly string[] _categoryNames = ["Lebensmittel", "Kultur", "Restaurants", "Bildung", "Nebenkosten", "Haushalt", "Reisen"];

    public DatabaseSeeder(IServiceScopeFactory scopeFactory, Fakers fakers)
    {
        _scopeFactory = scopeFactory;
        _fakers = fakers;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var serviceScope = _scopeFactory.CreateScope();
        using var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        if (!dbContext.BalanceEntries.Any())
        {
            if (!dbContext.Categories.Any())
            {
                foreach (var name in _categoryNames)
                {
                    var category = new Category() { Name = name };
                    dbContext.Categories.Add(category);
                }
                await dbContext.SaveChangesAsync();
            }
            var balanceEntryGenerator = _fakers.GetBalanceEntryFaker();
            var balanceEntries = balanceEntryGenerator.Generate(200);
            foreach (var balanceEntry in balanceEntries)
            {
                string categoryName = _fakers.GetCategoryFaker().Generate(1)?[0].Name!;
                balanceEntry.CategoryId = dbContext.Categories.FirstOrDefault(c => c.Name == categoryName)!.Id;
            }
            dbContext.BalanceEntries.AddRange(balanceEntries);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
