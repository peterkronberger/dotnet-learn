using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

var builder = Host.CreateApplicationBuilder(args);

// No log providers: the host prints no lifetime messages
builder.Logging.ClearProviders();

// The same key from three sources: the last source wins
IConfiguration config = builder.Configuration;
Console.WriteLine($"AppName (json only): {config["Seclog:AppName"]}");
Console.WriteLine($"Threshold (json 5, environment 7): {config["Seclog:Threshold"]}");
Console.WriteLine($"Mode (json, environment, command line): {config["Seclog:Mode"]}");

// Bind the section to an options class and read it through IOptions<T>
builder.Services.Configure<SeclogOptions>(config.GetSection("Seclog"));
builder.Services.AddHostedService<Worker>();

builder.Build().Run();

class SeclogOptions
{
    public string AppName { get; set; } = "";
    public int Threshold { get; set; }
    public string Mode { get; set; } = "";
}

// Runs once, prints one line and stops the host
class Worker(IOptions<SeclogOptions> options, IHostApplicationLifetime lifetime)
    : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        SeclogOptions value = options.Value;
        Console.WriteLine($"Worker: {value.AppName}, threshold {value.Threshold}, mode {value.Mode}");
        lifetime.StopApplication();
        return Task.CompletedTask;
    }
}
