Console.WriteLine(Describe<int>());
Console.WriteLine(Describe<bool>());
Console.WriteLine(Describe<string>());

static string Describe<T>()
{
    T? value = default;
    return value is null ? "null" : value.ToString() ?? "?";
}
