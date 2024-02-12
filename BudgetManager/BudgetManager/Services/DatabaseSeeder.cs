
using BudgetManager.Data;

namespace BudgetManager.Services;

public class DatabaseSeeder: IHostedService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly Fakers _fakers;

    public DatabaseSeeder(IServiceScopeFactory scopeFactory, Fakers fakers)
    {
        _scopeFactory = scopeFactory;
        _fakers = fakers;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var serviceScope = _scopeFactory.CreateScope();
        using var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        if(!dbContext.BalanceEntries.Any())
        {
            var balanceEntryGenerator = _fakers.GetBalanceEntryFaker();
            var balanceEntries = balanceEntryGenerator.Generate(200);
            dbContext.BalanceEntries.AddRange(balanceEntries);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
