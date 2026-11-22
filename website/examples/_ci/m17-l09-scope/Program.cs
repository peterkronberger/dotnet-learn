// @fold-start
using Microsoft.AspNetCore.TestHost;
// @fold-end

var builder = WebApplication.CreateBuilder(args);
// @fold-start
// The server runs in memory: no network and no port.
builder.WebHost.UseTestServer();
builder.Logging.ClearProviders();
// @fold-end

// One instance per request (scoped) and one for the whole app (singleton)
builder.Services.AddScoped<RequestCounter>();
builder.Services.AddSingleton<AppCounter>();

var app = builder.Build();

// The middleware and the handler both ask for a RequestCounter.
app.Use(async (context, next) =>
{
    var counter = context.RequestServices.GetRequiredService<RequestCounter>();
    counter.Lines.Add($"middleware sees instance {counter.Number}");
    await next(context);
});

app.MapGet("/count", (RequestCounter counter, AppCounter total) =>
{
    counter.Lines.Add($"handler sees instance {counter.Number}");
    total.Requests++;
    return string.Join("\n", counter.Lines) +
        $"\nrequests so far: {total.Requests}";
});
// @fold-start
await app.StartAsync();
using var client = app.GetTestClient();
// @fold-end

for (var i = 1; i <= 3; i++)
{
    Console.WriteLine($"== Request {i} ==");
    Console.WriteLine(await client.GetStringAsync("/count"));
}

await app.StopAsync();

// Every new instance gets the next number
class RequestCounter
{
    private static int _created;

    public int Number { get; } = Interlocked.Increment(ref _created);
    public List<string> Lines { get; } = [];
}

class AppCounter
{
    public int Requests { get; set; }
}
