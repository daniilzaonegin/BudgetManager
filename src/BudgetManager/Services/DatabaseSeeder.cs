
using BudgetManager.Data;

namespace BudgetManager.Services;

public class DatabaseSeeder : IHostedService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly Fakers _fakers;
    private readonly string[] _categoryNames = ["Lebensmittel", "Kultur", "Restaurants", "Bildung", "Nebenkosten", "Haushalt", "Reisen"];
    private readonly string[] _revenueCategoryNames = ["Monatsgehalt", "Zuschlag", "Bargeld", "Sonstiges"];

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
                    var category = new Category { Name = name, ForExpenses = true };
                    dbContext.Categories.Add(category);
                }
                foreach (var name in _revenueCategoryNames)
                {
                    var category = new Category { Name = name, ForExpenses = false };
                    dbContext.Categories.Add(category);
                }
                await dbContext.SaveChangesAsync();
            }
            var balanceEntryGenerator = _fakers.GetBalanceEntryFaker();
            var balanceEntries = balanceEntryGenerator.Generate(200);
            foreach (var balanceEntry in balanceEntries)
            {
                if (balanceEntry.Amount > 0)
                {
                    string categoryName = _fakers.GetCategoryFaker(true).Generate(1)?[0].Name!;
                    balanceEntry.CategoryId = dbContext.Categories.FirstOrDefault(c => c.Name == categoryName)!.Id;
                }
                else
                {
                    string categoryName = _fakers.GetCategoryFaker(false).Generate(1)?[0].Name!;
                    balanceEntry.CategoryId = dbContext.Categories.FirstOrDefault(c => c.Name == categoryName)!.Id;
                }
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
