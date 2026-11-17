using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

ServiceCollection services = new();
services.AddSingleton<IAlertSender, ConsoleSender>();
// A factory lambda creates the object and can pass arguments
services.AddSingleton<IAlertSender>(sp => new PrefixSender("[ALERT]"));
// TryAdd registers only if the interface has no registration yet: no effect here
services.TryAddSingleton<IAlertSender, ConsoleSender>();
services.AddSingleton<BroadcastService>();
services.AddSingleton<AlertService>();

using ServiceProvider provider = services.BuildServiceProvider();

// One sender requested: the last registration wins
IAlertSender single = provider.GetRequiredService<IAlertSender>();
single.Send("one sender requested");

// All senders requested: every registration, in registration order
foreach (IAlertSender sender in provider.GetServices<IAlertSender>())
{
    sender.Send("every registered sender");
}

// A constructor can ask for one sender or for all of them
provider.GetRequiredService<AlertService>().Raise("bob", 5);
provider.GetRequiredService<BroadcastService>().Raise("dave", 3);

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

    public AlertService(IAlertSender sender)
    {
        this.sender = sender;
    }

    public void Raise(string user, int failures)
    {
        sender.Send($"One: {user} failed {failures} times");
    }
}

class BroadcastService
{
    private readonly IEnumerable<IAlertSender> senders;

    public BroadcastService(IEnumerable<IAlertSender> senders)
    {
        this.senders = senders;
    }

    public void Raise(string user, int failures)
    {
        foreach (IAlertSender sender in senders)
        {
            sender.Send($"All: {user} failed {failures} times");
        }
    }
}
