// Count the failed logins of two sources with two tasks.
int failures = 0;

void CountIn(string[] lines)
{
    foreach (string line in lines)
    {
        if (line.Contains("login-failed"))
        {
            failures++;
        }
    }
}

Task web = Task.Run(() => CountIn(logSources["web"]));
Task vpn = Task.Run(() => CountIn(logSources["vpn"]));
await Task.WhenAll(web, vpn);
Console.WriteLine($"Failures: {failures}");
