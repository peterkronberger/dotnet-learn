(string, string?)[] pairs =
[
    ("bob", "203.0.113.7"),
    ("dave", null),
];
foreach (var pair in pairs)
{
    if (pair is (var user, var ip))
    {
        Console.WriteLine($"{user}: {ip is null}");
    }
}
