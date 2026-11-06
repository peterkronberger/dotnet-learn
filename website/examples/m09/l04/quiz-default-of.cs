Console.WriteLine(Fallback<int>());
Console.WriteLine(Fallback<bool>());
Console.WriteLine(Fallback<string>() is null);

static T? Fallback<T>()
{
    return default;
}
