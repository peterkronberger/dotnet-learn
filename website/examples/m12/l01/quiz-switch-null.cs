static string Kind(object? value) => value switch
{
    string => "string",
    int => "int",
    _ => "other",
};

string? missing = null;
Console.WriteLine(Kind("a"));
Console.WriteLine(Kind(missing));
Console.WriteLine(Kind(1));
