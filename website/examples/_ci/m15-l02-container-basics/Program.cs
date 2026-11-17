using Microsoft.Extensions.DependencyInjection;

// 1. Register: tell the container which type to create for which interface
ServiceCollection services = new();
services.AddSingleton<IAlertSender, ConsoleSender>();
services.AddSingleton<AlertService>();

// 2. Build the container
using ServiceProvider provider = services.BuildServiceProvider();

// 3. Resolve: the container creates AlertService and passes the sender
AlertService service = provider.GetRequiredService<AlertService>();
service.Raise("bob", 5);

// A type that was never registered: GetService returns null ...
UnregisteredSender? missing = provider.GetService<UnregisteredSender>();
Console.WriteLine($"GetService: {(missing is null ? "null" : "found")}");

// ... and GetRequiredService throws
try
{
    provider.GetRequiredService<UnregisteredSender>();
}
catch (InvalidOperationException)
{
    Console.WriteLine("GetRequiredService: InvalidOperationException");
}

interface IAlertSender
{
    void Send(string text);
}

class ConsoleSender : IAlertSender
{
    public void Send(string text)
    {
        Console.WriteLine($"SEND: {text}");
    }
}

class UnregisteredSender
{
}

class AlertService
{
    private readonly IAlertSender sender;

    public AlertService(IAlertSender sender)
    {
        this.sender = sender;
    }

    public void Raise(string user, int failures)
    {
        sender.Send($"{user} failed {failures} times");
    }
}
