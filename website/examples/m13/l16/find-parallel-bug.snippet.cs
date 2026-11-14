// Collect the failed lines of a log in parallel.
string[] lines = LoadLines();
List<string> failed = [];

Parallel.ForEach(lines, line =>
{
    if (line.Contains("login-failed"))
    {
        failed.Add(line);
    }
});

Console.WriteLine(failed.Count);

string[] LoadLines() => ["login-failed user=bob", "logout user=carol"];
