using Microsoft.EntityFrameworkCore;
using SecLog.Api;
using SecLog.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<SecLogDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("SecLog")));

var app = builder.Build();

app.MapIncidentEndpoints();

app.Run();

// Makes the Program class visible to the test project
public partial class Program;
