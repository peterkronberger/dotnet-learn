using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

// No log providers: the host prints no lifetime messages
builder.Logging.ClearProviders();
builder.Services.AddHostedService<FailingService>();

using IHost host = builder.Build();
try
{
    await host.RunAsync();
    Console.WriteLine("Main: RunAsync returned");
}
catch (Exception ex)
{
    Console.WriteLine($"Main: {ex.GetType().Name}: {ex.Message}");
}

// Throws after the first await
class FailingService : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("Service: ExecuteAsync starts");
        await Task.Delay(20, stoppingToken);
        throw new InvalidOperationException("Log source not reachable");
    }
}
