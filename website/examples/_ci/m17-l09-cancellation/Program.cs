// @fold-start
using Microsoft.AspNetCore.TestHost;
// @fold-end

var builder = WebApplication.CreateBuilder(args);
// @fold-start
// The server runs in memory: no network and no port.
builder.WebHost.UseTestServer();
builder.Logging.ClearProviders();
// @fold-end
var app = builder.Build();

// Lets the client wait until the handler has noticed the cancellation
var handlerStopped = new TaskCompletionSource();

// The parameter is bound to HttpContext.RequestAborted.
app.MapGet("/slow", async (CancellationToken cancellationToken) =>
{
    Console.WriteLine("handler starts");
    try
    {
        // Stands for a database query that takes a while
        await Task.Delay(TimeSpan.FromSeconds(30), cancellationToken);
        return "done";
    }
    catch (OperationCanceledException)
    {
        Console.WriteLine("handler was cancelled");
        handlerStopped.SetResult();
        throw;
    }
});
// @fold-start
await app.StartAsync();
using var client = app.GetTestClient();
// @fold-end

// The client gives up after 200 ms
using var source = new CancellationTokenSource(200);
try
{
    await client.GetAsync("/slow", source.Token);
}
catch (OperationCanceledException)
{
    await handlerStopped.Task;
    Console.WriteLine("client: request cancelled");
}

await app.StopAsync();
