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

var app = builder.Build();

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

// Serves the document at /openapi/v1.json (m17-l12 limits this later);
// anonymous, so that the document stays reachable without a token
app.MapOpenApi().AllowAnonymous();

app.MapIncidentEndpoints();

app.Run();

// Makes the Program class visible to the test project
public partial class Program;
