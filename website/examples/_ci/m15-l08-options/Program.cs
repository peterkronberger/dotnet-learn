using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

var builder = Host.CreateApplicationBuilder(args);
builder.Logging.ClearProviders();

// Settings from an in-memory source, so the program can change them while it runs
builder.Configuration.AddInMemoryCollection(new Dictionary<string, string?>
{
    ["Detection:MaxFailures"] = "5",
    ["Detection:WindowMinutes"] = "10",
});

// Bind the section "Detection" to the options class
builder.Services.Configure<DetectionOptions>(
    builder.Configuration.GetSection("Detection"));

using IHost host = builder.Build();
IServiceProvider services = host.Services;

// IOptions<T>: singleton, reads the settings once
var options = services.GetRequiredService<IOptions<DetectionOptions>>();
Console.WriteLine($"IOptions at start: {options.Value.MaxFailures}");

// IOptionsMonitor<T>: singleton, always the current value
var monitor = services.GetRequiredService<IOptionsMonitor<DetectionOptions>>();
Console.WriteLine($"IOptionsMonitor at start: {monitor.CurrentValue.MaxFailures}");

// Change the setting while the program runs and tell the configuration to reload
builder.Configuration["Detection:MaxFailures"] = "9";
((IConfigurationRoot)builder.Configuration).Reload();

Console.WriteLine($"IOptions after change: {options.Value.MaxFailures}");

// IOptionsSnapshot<T>: scoped, reads the settings again for every scope
using (IServiceScope scope = services.CreateScope())
{
    var snapshot = scope.ServiceProvider
        .GetRequiredService<IOptionsSnapshot<DetectionOptions>>();
    Console.WriteLine($"IOptionsSnapshot in a new scope: {snapshot.Value.MaxFailures}");
}

Console.WriteLine($"IOptionsMonitor current value: {monitor.CurrentValue.MaxFailures}");

class DetectionOptions
{
    public int MaxFailures { get; set; } = 5;
    public int WindowMinutes { get; set; } = 10;
}
