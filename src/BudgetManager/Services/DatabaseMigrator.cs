
using BudgetManager.Data;
using Microsoft.EntityFrameworkCore;

namespace BudgetManager.Services;

public class DatabaseMigrator : IHostedService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger _logger;
    private readonly IHostApplicationLifetime _hostLifetime;

    public DatabaseMigrator(IServiceScopeFactory scopeFactory, 
        ILogger<DatabaseMigrator> logger,
        IHostApplicationLifetime hostLifetime)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
        _hostLifetime = hostLifetime;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            _logger.LogInformation("Applying database migrations");
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await dbContext.Database.MigrateAsync(cancellationToken);
            _logger.LogInformation("Database migrations successfully migrated.");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred");
            _hostLifetime.StopApplication();
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
