using Microsoft.Extensions.DependencyInjection;

ServiceCollection services = new();
// The same interface, registered twice with different keys
services.AddKeyedSingleton<IAlertSender, ConsoleSender>("console");
services.AddKeyedSingleton<IAlertSender>(
    "prefix", (sp, key) => new PrefixSender("[ALERT]"));
services.AddSingleton<AlertService>();

using ServiceProvider provider = services.BuildServiceProvider();

// Ask for a service by its key
IAlertSender console = provider.GetRequiredKeyedService<IAlertSender>("console");
console.Send("asked by key");

// The attribute on a constructor parameter selects the key
provider.GetRequiredService<AlertService>().Raise("bob", 5);

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

class PrefixSender : IAlertSender
{
    private readonly string prefix;

    public PrefixSender(string prefix)
    {
        this.prefix = prefix;
    }

    public void Send(string text)
    {
        Console.WriteLine($"{prefix} {text}");
    }
}

class AlertService
{
    private readonly IAlertSender sender;

    public AlertService([FromKeyedServices("prefix")] IAlertSender sender)
    {
        this.sender = sender;
    }

    public void Raise(string user, int failures)
    {
        sender.Send($"{user} failed {failures} times");
    }
}
