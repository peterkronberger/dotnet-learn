// @fold-start
using System.Net;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.TestHost;
// @fold-end

// A request that the proxy at 198.51.100.1 passes on for a client
// at 203.0.113.7, who used HTTPS
Console.WriteLine("Without UseForwardedHeaders");
await Send(useMiddleware: false, sender: "198.51.100.1");

Console.WriteLine("With UseForwardedHeaders, request from the known proxy");
await Send(useMiddleware: true, sender: "198.51.100.1");

Console.WriteLine("With UseForwardedHeaders, request from another address");
await Send(useMiddleware: true, sender: "192.0.2.99");

async Task Send(bool useMiddleware, string sender)
{
    var builder = WebApplication.CreateBuilder();
    // @fold-start
    // The server runs in memory: no network and no port.
    builder.WebHost.UseTestServer();
    builder.Logging.ClearProviders();
    // @fold-end

    // The middleware reads these two headers, but only from trusted proxies
    builder.Services.Configure<ForwardedHeadersOptions>(options =>
    {
        options.ForwardedHeaders =
            ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
        options.KnownProxies.Add(IPAddress.Parse("198.51.100.1"));
    });

    var app = builder.Build();
    // @fold-start
    // Stands in for the network: sets the address of the sender.
    app.Use((context, next) =>
    {
        context.Connection.RemoteIpAddress = IPAddress.Parse(sender);
        return next(context);
    });
    // @fold-end

    // Early in the pipeline: later middleware sees the corrected values
    if (useMiddleware)
    {
        app.UseForwardedHeaders();
    }

    app.MapGet("/", (HttpContext context) =>
        $"scheme: {context.Request.Scheme}, " +
        $"client: {context.Connection.RemoteIpAddress}");
    // @fold-start
    await app.StartAsync();
    using var client = app.GetTestClient();
    // @fold-end

    using var request = new HttpRequestMessage(HttpMethod.Get, "/");
    request.Headers.Add("X-Forwarded-For", "203.0.113.7");
    request.Headers.Add("X-Forwarded-Proto", "https");
    using var response = await client.SendAsync(request);
    Console.WriteLine($"  {await response.Content.ReadAsStringAsync()}");

    await app.StopAsync();
}
