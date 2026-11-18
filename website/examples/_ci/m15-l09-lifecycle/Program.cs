using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

// No log providers: the host prints no lifetime messages
builder.Logging.ClearProviders();
builder.Services.AddHostedService<CounterService>();

using IHost host = builder.Build();
Console.WriteLine("Main: host built");
await host.RunAsync();
Console.WriteLine("Main: host stopped");

// Counts to three, then asks the host to stop
class CounterService(IHostApplicationLifetime lifetime) : BackgroundService
{
    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("Service: StartAsync");
        await base.StartAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("Service: ExecuteAsync starts");
        for (int step = 1; step <= 3; step++)
        {
            await Task.Delay(20, stoppingToken);
            Console.WriteLine($"Service: step {step}");
        }
        lifetime.StopApplication();
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("Service: StopAsync");
        await base.StopAsync(cancellationToken);
    }
}
