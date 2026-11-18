// Read the thresholds of the brute-force rule: the environment beats appsettings.json
IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();
int maxFailures = configuration.GetValue<int>("Detection:MaxFailures");
int windowMinutes = configuration.GetValue<int>("Detection:WindowMinutes");
Console.WriteLine(
    $"Detection settings: {maxFailures} failures in {windowMinutes} minutes");
TimeSpan window = TimeSpan.FromMinutes(windowMinutes);
// ...
FailedLoginRule failedRule = new(maxFailures);
