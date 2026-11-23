// Log lines of the sample data
string[] lines =
[
    "2026-03-02T08:00:12Z login-success user=alice ip=192.0.2.10",
    "2026-03-02T08:01:40Z login-failed user=bob ip=203.0.113.7",
    "2026-03-02T08:02:05Z login-failed user=bob ip=203.0.113.7",
    "2026-03-02T08:03:31Z login-success user=carol ip=198.51.100.23",
    "2026-03-02T08:07:20Z login-success user=dave ip=198.51.100.8",
];

foreach (var line in LogMasker.MaskLines(lines))
{
    Console.WriteLine(line);
}

public static class LogMasker
{
    public static string[] MaskLines(string[] lines)
    {
        return lines.Select(MaskLine).ToArray();
    }

    // Task: after masking, a line has no user name and no IP address
    private static string MaskLine(string line)
    {
        var parts = line.Split(' ');
        for (int i = 0; i < parts.Length; i++)
        {
            if (parts[i].StartsWith("user="))
            {
                parts[i] = "user=<USER>";
            }
        }

        return string.Join(' ', parts);
    }
}
