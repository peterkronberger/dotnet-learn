int plain = Missing<int>();
string? text = Missing<string>();
int? maybe = NoValue<int>();
string? name = NoReference<string>();

Console.WriteLine(plain);
Console.WriteLine(text is null);
Console.WriteLine(maybe.HasValue);
Console.WriteLine(name is null);

static T? Missing<T>()
{
    return default;
}

static T? NoValue<T>() where T : struct
{
    return null;
}

static T? NoReference<T>() where T : class
{
    return null;
}
