foreach (string name in new[] { "bob", "bobby", "alice" })
{
    string result = name switch
    {
        var text when text.StartsWith("bo") => "starts with bo",
        "bob" => "exactly bob",
        _ => "other",
    };
    Console.WriteLine(result);
}
