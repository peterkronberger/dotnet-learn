using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

// The same invalid settings (MaxFailures = 0, WindowMinutes = 100), once without and once with ValidateOnStart
RunWith(validateOnStart: false);
RunWith(validateOnStart: true);

void RunWith(bool validateOnStart)
{
    Console.WriteLine($"== ValidateOnStart: {validateOnStart}");
    var builder = Host.CreateApplicationBuilder();
    builder.Logging.ClearProviders();
    builder.Configuration.AddInMemoryCollection(new Dictionary<string, string?>
    {
        ["Detection:MaxFailures"] = "0",
        ["Detection:WindowMinutes"] = "100",
    });

    // Bind the section, check the attributes and one more rule with a lambda
    var optionsBuilder = builder.Services
        .AddOptions<DetectionOptions>()
        .Bind(builder.Configuration.GetSection("Detection"))
        .ValidateDataAnnotations()
        .Validate(o => o.WindowMinutes <= 60, "WindowMinutes must be at most 60.");
    if (validateOnStart)
    {
        optionsBuilder.ValidateOnStart();
    }

    using IHost host = builder.Build();
    try
    {
        host.Start();
        Console.WriteLine("Host started");
        _ = host.Services.GetRequiredService<IOptions<DetectionOptions>>().Value;
    }
    catch (OptionsValidationException ex)
    {
        Console.WriteLine($"{ex.GetType().Name}: {ex.Failures.Count()} failures");
        foreach (string failure in ex.Failures)
        {
            Console.WriteLine($"  {failure}");
        }
    }
}

class DetectionOptions
{
    [Range(1, int.MaxValue)]
    public int MaxFailures { get; set; } = 5;

    [Range(1, int.MaxValue)]
    public int WindowMinutes { get; set; } = 10;
}
