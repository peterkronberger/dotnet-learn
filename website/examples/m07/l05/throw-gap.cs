// Add the three guard clauses in Open with throw helpers.
// Wanted output: ArgumentNullException, ArgumentException,
// ArgumentOutOfRangeException, Port 8080
string?[] hosts = [null, "", "ok", "ok"];
int[] ports = [1, 1, -1, 8080];
for (int i = 0; i < hosts.Length; i++)
{
    try
    {
        Open(hosts[i], ports[i]);
        Console.WriteLine($"Port {ports[i]}");
    }
    catch (ArgumentException ex)
    {
        Console.WriteLine(ex.GetType().Name);
    }
}

void Open(string? host, int port)
{
    // Reject null
    // Reject the empty text
    // Reject a negative port
}
