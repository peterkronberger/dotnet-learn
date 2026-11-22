using Microsoft.AspNetCore.Mvc;

var app = WebApplication.CreateBuilder(args).Build();

// The analyst name comes from the header X-Analyst.
app.MapGet("/whoami", ([___(Name = "X-Analyst")] string analyst) =>
    $"analyst={analyst}");

app.Run();
