using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using SecLog.Api;
using SecLog.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<SecLogDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("SecLog")));

// One service instance per request, like the DbContext it uses
builder.Services.AddScoped<IIncidentService, IncidentService>();

// Builds an OpenAPI document from the endpoints and their metadata
builder.Services.AddOpenApi();

// Checks data annotations of parameters and bodies before a handler runs
builder.Services.AddValidation();

// Writes every error response in the problem details format
builder.Services.AddProblemDetails();

// Reads bearer tokens; issuer and audience come from the configuration
// section Authentication:Schemes:Bearer (appsettings.json)
builder.Services.AddAuthentication().AddJwtBearer();

// Named rule: only the role responder may change a status
builder.Services.AddAuthorizationBuilder()
    .AddPolicy("ChangeIncidentStatus", policy =>
        policy.RequireRole("responder"));

// Behind a reverse proxy: take the client address and the scheme
// from the headers that the proxy adds
builder.Services.Configure<ForwardedHeadersOptions>(options =>
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto);

var app = builder.Build();

// First in the pipeline, so that every later middleware sees the
// corrected client address and scheme
app.UseForwardedHeaders();

// Adds the protection header to every response, also to error responses
app.Use(async (context, next) =>
{
    // The exception handler clears headers that are set earlier,
    // so the header is set just before the response starts.
    context.Response.OnStarting(() =>
    {
        context.Response.Headers.XContentTypeOptions = "nosniff";
        return Task.CompletedTask;
    });
    await next(context);
});

// An unhandled exception becomes a 500 problem response without details
app.UseExceptionHandler();
// An empty error response (for example 404) gets a problem body
app.UseStatusCodePages();

// Both middleware run before the endpoints: first who, then may they
app.UseAuthentication();
app.UseAuthorization();

// The document at /openapi/v1.json exists only in development
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapIncidentEndpoints();

app.Run();

// Makes the Program class visible to the test project
public partial class Program;
