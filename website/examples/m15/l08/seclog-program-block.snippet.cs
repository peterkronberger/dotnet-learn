// Read the thresholds of the brute-force rule: the environment beats appsettings.json
IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();
DetectionOptions detection = configuration
    .GetSection(DetectionOptions.SectionName).Get<DetectionOptions>() ?? new();
// ...
FailedLoginRule failedRule = new(Options.Create(detection));
// ...
// Options: bind the section to the options class and check its attributes
services.AddOptions<DetectionOptions>()
    .Bind(configuration.GetSection(DetectionOptions.SectionName))
    .ValidateDataAnnotations();
// ...
// Read the options once, so invalid settings fail right at the start
_ = provider.GetRequiredService<IOptions<DetectionOptions>>().Value;
