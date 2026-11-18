using Microsoft.Extensions.Configuration;

// Three sources in this order: the last source wins
IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .AddCommandLine(args)
    .Build();

// Read single values by key: ":" separates the levels
Console.WriteLine($"Mode: {configuration["Detection:Mode"]}");
Console.WriteLine($"MaxFailures: {configuration.GetValue<int>("Detection:MaxFailures")}");
Console.WriteLine($"WindowMinutes: {configuration["Detection:WindowMinutes"]}");

// Read a missing key: the indexer gives null, GetValue gives a default
Console.WriteLine($"Missing: {configuration["Detection:Missing"] ?? "(null)"}");
Console.WriteLine($"Missing as int: {configuration.GetValue<int>("Detection:Missing")}");

// Read a whole section
IConfigurationSection section = configuration.GetSection("Detection");
foreach (IConfigurationSection child in section.GetChildren())
{
    Console.WriteLine($"{child.Key} = {child.Value}");
}
