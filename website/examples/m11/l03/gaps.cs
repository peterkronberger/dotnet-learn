string[] hosts = ["web-02", "db-01", "web-01", "cache-01", "web-03"];

// Build the list of web hosts, sorted by name
List<string> webHosts = hosts
    .Where(h => h.StartsWith("web", StringComparison.Ordinal))
    .OrderBy(h => h, StringComparer.Ordinal)
    .ToList();

// Print the first two web hosts in upper case
Console.WriteLine(string.Join(", ", webHosts.Take(2).Select(h => h.ToUpper())));
