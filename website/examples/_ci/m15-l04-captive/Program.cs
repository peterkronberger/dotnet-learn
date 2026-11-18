using Microsoft.Extensions.DependencyInjection;

ServiceCollection services = new();
services.AddScoped<RequestInfo>();
services.AddSingleton<Dashboard>();

// Without validation the captive dependency goes unnoticed
using (ServiceProvider provider = services.BuildServiceProvider())
{
    for (int number = 1; number <= 2; number++)
    {
        using IServiceScope scope = provider.CreateScope();
        var info = scope.ServiceProvider.GetRequiredService<RequestInfo>();
        var dashboard = scope.ServiceProvider.GetRequiredService<Dashboard>();
        Console.WriteLine($"Scope {number}: RequestInfo {info.Id}, "
            + $"Dashboard uses RequestInfo {dashboard.InfoId}");
    }
}

// With validation the container reports the problem
ServiceProviderOptions options = new()
{
    ValidateScopes = true,
    ValidateOnBuild = true,
};
try
{
    using ServiceProvider checkedProvider = services.BuildServiceProvider(options);
}
catch (Exception ex)
{
    Console.WriteLine(ex.GetType().Name);
    Console.WriteLine(ex.Message);
}

class RequestInfo
{
    private static int created;
    public int Id { get; } = ++created;
}

class Dashboard(RequestInfo info)
{
    public int InfoId => info.Id;
}
