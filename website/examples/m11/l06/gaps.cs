List<string> hosts = ["web-1", "db-1", "web-22"];

// 1. Filter the web hosts
// 2. Project each host into an anonymous object
// 3. Print the rows
var rows = hosts
    .Where(h => h.StartsWith("web"))
    .Select(h => new { Host = h, h.Length });

foreach (var row in rows)
{
    Console.WriteLine($"{row.Host}: {row.Length}");
}
