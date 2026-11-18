using Microsoft.Extensions.DependencyInjection;

ServiceCollection services = new();
services.AddSingleton<FailureCounter>();
services.AddScoped<Worker>();

ServiceProviderOptions options = new()
{
    ValidateScopes = true,
    ValidateOnBuild = true,
};
using ServiceProvider provider = services.BuildServiceProvider(options);

// Four workers, each in its own scope, share the one counter
List<Task> tasks = [];
for (int number = 1; number <= 4; number++)
{
    tasks.Add(Task.Run(() =>
    {
        using IServiceScope scope = provider.CreateScope();
        scope.ServiceProvider.GetRequiredService<Worker>().Run(1000);
    }));
}
await Task.WhenAll(tasks);

Console.WriteLine($"Total: {provider.GetRequiredService<FailureCounter>().Total}");

class FailureCounter
{
    private int total;
    public int Total => total;
    public void Add() => Interlocked.Increment(ref total);
}

class Worker(FailureCounter counter)
{
    public void Run(int failures)
    {
        for (int i = 0; i < failures; i++)
        {
            counter.Add();
        }
    }
}
