using Microsoft.EntityFrameworkCore;
using SecLog.Api;
using SecLog.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<SecLogDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("SecLog")));

// Checks data annotations of parameters and bodies before a handler runs
builder.Services.AddValidation();

var app = builder.Build();

app.MapIncidentEndpoints();

app.Run();

// Makes the Program class visible to the test project
public partial class Program;
