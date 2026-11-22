// @fold-start
using Microsoft.AspNetCore.TestHost;
// @fold-end

// The same two middleware components in two orders.
Console.WriteLine("== 1. Exception handler first ==");
await Run(handlerFirst: true);
Console.WriteLine("== 2. Exception handler last ==");
await Run(handlerFirst: false);

async Task Run(bool handlerFirst)
{
    var app = CreateApp();

    if (handlerFirst)
        app.UseExceptionHandler();

    // This middleware fails for every request
    app.Use((HttpContext context, RequestDelegate next) =>
        throw new InvalidOperationException("Database is down"));

    if (!handlerFirst)
        app.UseExceptionHandler();

    await app.StartAsync();
    using var client = app.GetTestClient();
    try
    {
        using var response = await client.GetAsync("/");
        Console.WriteLine(
            $"< {(int)response.StatusCode} {response.ReasonPhrase}");
    }
    catch (InvalidOperationException ex)
    {
        Console.WriteLine($"< exception reached the server: {ex.Message}");
    }
    Console.WriteLine();
    await app.StopAsync();
}

// @fold-start
// Creates an app that runs in memory and has a problem details service.
static WebApplication CreateApp()
{
    var builder = WebApplication.CreateBuilder();
    builder.WebHost.UseTestServer();
    builder.Logging.ClearProviders();
    builder.Services.AddProblemDetails();
    return builder.Build();
}
// @fold-end
