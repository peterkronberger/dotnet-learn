var app = WebApplication.CreateBuilder(args).Build();

// Only whole numbers should match {id}.
app.MapGet("/incidents/{id:___}", (int id) => $"incident {id}");

app.Run();
