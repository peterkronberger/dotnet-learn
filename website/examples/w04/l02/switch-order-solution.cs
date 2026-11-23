// "bob" prints "exactly bob": the narrow arm comes first
foreach (string name in new[] { "bob", "bobby", "alice" })
{
    string result = name switch
    {
        "bob" => "exactly bob",
        var text when text.StartsWith("bo", StringComparison.Ordinal) => "starts with bo",
        _ => "other",
    };
    Console.WriteLine(result);
}
