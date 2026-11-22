app.Use(async (context, next) =>
{
    Console.WriteLine("A before");
    await next(context);
    Console.WriteLine("A after");
});

app.Use(async (context, next) =>
{
    Console.WriteLine("B before");
    await next(context);
    Console.WriteLine("B after");
});

app.MapGet("/", () =>
{
    Console.WriteLine("endpoint");
    return "ok";
});
