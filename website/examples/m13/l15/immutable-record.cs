// Workers only read an immutable record; a change makes a new object.
Limits limits = new(3, "warn");

Task<string>[] readers =
[
    Task.Run(() => $"A reads {limits}"),
    Task.Run(() => $"B reads {limits}"),
];
foreach (string line in await Task.WhenAll(readers))
{
    Console.WriteLine(line);
}

Limits stricter = limits with { MaxFailures = 1 };
Console.WriteLine($"Old: {limits}");
Console.WriteLine($"New: {stricter}");

record Limits(int MaxFailures, string Level);
