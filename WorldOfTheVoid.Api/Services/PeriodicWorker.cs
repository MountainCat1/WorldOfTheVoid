using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;
using WorldOfTheVoid.Infrastructure.DbContext;
using WorldOfTheVoid.Interfaces;

public class PeriodicWorker : BackgroundService
{
    private readonly ILogger<PeriodicWorker> _logger;
    private readonly TimeSpan _interval = TimeSpan.FromSeconds(1);
    private readonly IServiceProvider _serviceProvider;

    public PeriodicWorker(ILogger<PeriodicWorker> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        _logger.LogInformation("PeriodicWorker started.");

        while (!ct.IsCancellationRequested)
        {
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<GameDbContext>();
            var periodicTasks = scope.ServiceProvider.GetServices<IPeriodicTask>();

            try
            {
                foreach (var task in periodicTasks)
                {
                    try
                    {
                        _logger.LogInformation("Executing task: {task}", task.GetType().Name);
                        await task.ExecuteAsync(ct);
                        _logger.LogInformation("Executed task: {task}", task.GetType().Name);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error executing task: {task}", task.GetType().Name);
                    }
                }

                await dbContext.SaveChangesAsync(ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in PeriodicWorker execution loop.");
            }

            await Task.Delay(_interval, ct);
        }

        _logger.LogInformation("PeriodicWorker stopped.");
    }
}