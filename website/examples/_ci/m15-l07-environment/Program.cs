using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = Host.CreateApplicationBuilder(args);
builder.Logging.ClearProviders();

// The environment name comes from DOTNET_ENVIRONMENT (default: Production)
Console.WriteLine($"Environment: {builder.Environment.EnvironmentName}");

// appsettings.json first, then appsettings.<Environment>.json on top
IConfiguration configuration = builder.Configuration;
Console.WriteLine($"MaxFailures: {configuration["Detection:MaxFailures"]}");
Console.WriteLine($"WindowMinutes: {configuration["Detection:WindowMinutes"]}");
