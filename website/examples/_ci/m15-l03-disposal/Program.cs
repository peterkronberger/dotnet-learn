using Microsoft.Extensions.DependencyInjection;

ServiceCollection services = new();
services.AddSingleton<SingletonResource>();
services.AddScoped<ScopedResource>();
services.AddTransient<TransientResource>();
services.AddSingleton(new ReadyResource());

using ServiceProvider provider = services.BuildServiceProvider();
provider.GetRequiredService<SingletonResource>();
provider.GetRequiredService<ReadyResource>();

using (IServiceScope scope = provider.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<ScopedResource>();
    scope.ServiceProvider.GetRequiredService<TransientResource>();
    Console.WriteLine("Resources used, scope still open");
}

Console.WriteLine("Scope closed");
provider.Dispose();
Console.WriteLine("Provider disposed");

class SingletonResource : IDisposable
{
    public void Dispose() => Console.WriteLine("  disposed: singleton");
}

class ScopedResource : IDisposable
{
    public void Dispose() => Console.WriteLine("  disposed: scoped");
}

class TransientResource : IDisposable
{
    public void Dispose() => Console.WriteLine("  disposed: transient");
}

class ReadyResource : IDisposable
{
    public void Dispose() => Console.WriteLine("  disposed: ready object");
}
