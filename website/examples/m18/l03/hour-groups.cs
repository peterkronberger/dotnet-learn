using System.Globalization;

// Three failed logins; the second one carries the offset +01:00
string[] texts =
[
    "2026-03-02T08:50:00Z",
    "2026-03-02T09:30:00+01:00",
    "2026-03-02T09:10:00Z",
];
DateTimeOffset[] times = texts
    .Select(text => DateTimeOffset.Parse(text, CultureInfo.InvariantCulture))
    .ToArray();

Print("Own offset", times.GroupBy(t => t.Hour));
Print("UTC", times.GroupBy(t => t.UtcDateTime.Hour));

static void Print(string title, IEnumerable<IGrouping<int, DateTimeOffset>> groups)
{
    Console.WriteLine(title);
    foreach (var group in groups.OrderBy(g => g.Key))
    {
        Console.WriteLine($"  hour {group.Key}: {group.Count()}");
    }
}
