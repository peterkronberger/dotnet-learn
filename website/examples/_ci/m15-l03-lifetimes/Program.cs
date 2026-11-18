using Microsoft.Extensions.DependencyInjection;

ServiceCollection services = new();
services.AddSingleton<SingletonService>();
services.AddScoped<ScopedService>();
services.AddTransient<TransientService>();

using ServiceProvider provider = services.BuildServiceProvider();

for (int number = 1; number <= 2; number++)
{
    using IServiceScope scope = provider.CreateScope();
    IServiceProvider sp = scope.ServiceProvider;
    Console.WriteLine($"Scope {number}");

    var singleton1 = sp.GetRequiredService<SingletonService>();
    var singleton2 = sp.GetRequiredService<SingletonService>();
    Console.WriteLine($"  singleton: id {singleton1.Id}, same: {singleton1 == singleton2}");

    var scoped1 = sp.GetRequiredService<ScopedService>();
    var scoped2 = sp.GetRequiredService<ScopedService>();
    Console.WriteLine($"  scoped: id {scoped1.Id}, same: {scoped1 == scoped2}");

    var transient1 = sp.GetRequiredService<TransientService>();
    var transient2 = sp.GetRequiredService<TransientService>();
    Console.WriteLine($"  transient: ids {transient1.Id}/{transient2.Id}, same: {transient1 == transient2}");
}

class SingletonService
{
    private static int created;
    public int Id { get; } = ++created;
}

class ScopedService
{
    private static int created;
    public int Id { get; } = ++created;
}

class TransientService
{
    private static int created;
    public int Id { get; } = ++created;
}
