using Microsoft.Extensions.Hosting;

// Runs the analysis once, then asks the host to stop
sealed class AnalysisService(
    Func<CancellationToken, Task> analysis, IHostApplicationLifetime lifetime)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            await analysis(stoppingToken);
        }
        finally
        {
            lifetime.StopApplication();
        }
    }
}
