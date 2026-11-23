// "bob" should print "exactly bob". Fix the order of the arms.
foreach (string name in new[] { "bob", "bobby", "alice" })
{
    string result = name switch
    {
        var text when text.StartsWith("bo", StringComparison.Ordinal) => "starts with bo",
        "bob" => "exactly bob",
        _ => "other",
    };
    Console.WriteLine(result);
}
