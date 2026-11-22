// @fold-start
using Microsoft.AspNetCore.TestHost;
// @fold-end

// The same app, started once per environment name
await Run(null);
await Run("Development");
await Run("Production");

async Task Run(string? environmentName)
{
    var builder = WebApplication.CreateBuilder(
        new WebApplicationOptions { EnvironmentName = environmentName });
    // @fold-start
    // The server runs in memory: no network and no port.
    builder.WebHost.UseTestServer();
    builder.Logging.ClearProviders();
    // @fold-end
    builder.Services.AddOpenApi();

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        // The OpenAPI document exists only in development. The developer
        // exception page is added automatically in development.
        app.MapOpenApi();
    }
    else
    {
        // Elsewhere an error becomes an empty 500 response
        app.UseExceptionHandler(errors => errors.Run(context =>
        {
            context.Response.StatusCode = 500;
            return Task.CompletedTask;
        }));
    }

    app.MapGet("/incidents", () => "incidents");
    app.MapGet("/fail", () =>
    {
        throw new InvalidOperationException("Database is down");
    });
    // @fold-start
    await app.StartAsync();
    using var client = app.GetTestClient();
    // @fold-end

    Console.WriteLine(
        $"Environment: {app.Environment.EnvironmentName} " +
        $"(requested: {environmentName ?? "nothing"})");
    Console.WriteLine($"  IsDevelopment: {app.Environment.IsDevelopment()}");

    using var document = await client.GetAsync("/openapi/v1.json");
    Console.WriteLine(
        $"  GET /openapi/v1.json: {(int)document.StatusCode}");

    using var failure = await client.GetAsync("/fail");
    var body = await failure.Content.ReadAsStringAsync();
    Console.WriteLine($"  GET /fail: {(int)failure.StatusCode}");
    Console.WriteLine(
        $"  exception message in body: {body.Contains("Database is down")}");

    await app.StopAsync();
}
